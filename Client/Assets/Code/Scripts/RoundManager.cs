﻿namespace ScotlandYard.Scripts
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Controller;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Helper;
    using ScotlandYard.Scripts.PlayerScripts;
    using ScotlandYard.Scripts.Street;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class RoundManager : MonoBehaviour
    {
        #region Members
        [SerializeField] protected StreetController STREET_CONTROLLER;
        [SerializeField] protected PlayerController PLAYER_CONTROLLER;
        [SerializeField] protected HistoryController HISTORY_CONTROLLER;
        
        protected int round = 1;
        protected int playerIndex = 0;
        protected int[] detectionRounds = new int[] { 3, 8, 13, 18, 24 };

        protected ERound roundState;
        #endregion

        #region Properties
        public int Round
        {
            get => round;
            set
            {
                if(round != value)
                {
                    round = value;
                    GameEvents.Current.RoundHasEnded(this, round);
                }
            }
        }
        #endregion


        protected void Start()
        {
            GameEvents.Current.OnPlayerMoveFinished += Current_OnPlayerMoveFinished;
            GameEvents.Current.OnPlayerResigned += Current_OnPlayerResigned;
            GameEvents.Current.OnDestinationSelected += OnlyHighlightDestination;
            GameEvents.Current.OnTicketSelection_Canceled += Current_OnTicketSelection_Canceled;
            GameEvents.Current.OnTicketSelection_Approved += Current_OnTicketSelection_Approved;

            GameEvents.Current.OnDetectivesWon += Current_OnDetectivesWon;
            GameEvents.Current.OnMisterXWon += Current_OnMisterXWon;

            GameEvents.Current.OnPlayerItemClicked += Current_OnPlayerItemClicked;

            StartCoroutine(nameof(StartInit));
        }

        protected IEnumerator StartInit()
        {
            roundState = ERound.INITIALIZATION;

            Debug.Log($"Initialization ...");
            Cursor.visible = true;

            STREET_CONTROLLER.Init();
            PLAYER_CONTROLLER.Init();

            AssignStartingPositions(STREET_CONTROLLER.GetAllStreetPoints(), PLAYER_CONTROLLER.GetAllAgents());

            HISTORY_CONTROLLER.Init(PLAYER_CONTROLLER.GetAllAgents(), detectionRounds);

            yield return new WaitForSeconds(0.5f);

            UIEvents.Current.RoundAdded(null, Round.ToString());
            Debug.Log($"Round {Round} started.");
            PlayRound();
        }

        protected void AssignStartingPositions(List<StreetPoint> streetPoints, List<Agent> agents)
        {
            System.Random random = new System.Random();
            List<IStreetPoint> tempPoints = streetPoints.Select(sp => (IStreetPoint)sp).ToList();

            foreach (Agent agent in agents)
            {
                IStreetPoint temp = tempPoints[random.Next(0, tempPoints.Count-1)];

                Debug.Log($"{agent.Data.AgentName} assigned to Position {temp?.ToString()}");

                agent.Data.CurrentPosition = temp;
                agent.GetTransform().position = temp.GetTransform().position;
                tempPoints.Remove(temp);

                if (agent.Data.PlayerRole == EPlayerRole.MISTERX)
                {
                    HashSet<IStreetPoint> neighbors = STREET_CONTROLLER.GetNeighboringStreetPoints(temp, 2, true);
                    if((neighbors.Count + agents.Count) > tempPoints.Count)
                    {
                        neighbors = STREET_CONTROLLER.GetNeighboringStreetPoints(temp, 1, true);
                    }

                    tempPoints = tempPoints.Except(neighbors).ToList();
                }
            }
        }

        protected void PlayRound()
        {
            if(Round <= 24)
            {
                if(playerIndex >= PLAYER_CONTROLLER.GetPlayerAmount())
                {
                    playerIndex = 0;
                    Round ++;
                    UIEvents.Current.RoundAdded(null, Round.ToString());
                    Debug.Log($"Round {Round} started.");
                }

                if(!PLAYER_CONTROLLER.GetPlayer(playerIndex).Data.HasLost)
                {
                    // Hide/Show all gameobjects of MisterX
                    Agent currentAgent = PLAYER_CONTROLLER.GetPlayer(playerIndex);
                    if(!currentAgent.GetType().Name.Equals(typeof(Player)) && currentAgent.Data.PlayerRole == EPlayerRole.MISTERX)
                    {
                        PLAYER_CONTROLLER.HidePlayer(currentAgent, detectionRounds.Contains(Round));
                    }

                    StartCoroutine(nameof(BeginPlayerRound), playerIndex);
                }
                else
                {
                    playerIndex++;
                    PlayRound();
                }
            }
            else
            {
                GameEvents.Current.MisterXWon(this, null);
            }
        }

        protected IEnumerator BeginPlayerRound(int index)
        {
            if(roundState != ERound.MISTER_X_TURN && roundState != ERound.DETECTIVE_TURN)
            {
                Agent player = PLAYER_CONTROLLER.GetPlayer(index);
                player.SetActive(true);

                if (player.Data.PlayerRole == EPlayerRole.MISTERX)
                {
                    roundState = ERound.MISTER_X_TURN;
                }
                else
                {
                    roundState = ERound.DETECTIVE_TURN;
                }

                UIEvents.Current.ShowRoundMessage(null, player.Data.AgentName);

                yield return new WaitForSeconds(2f);

                UIEvents.Current.HideRoundMessage(null, player.Data.AgentName);

                PLAYER_CONTROLLER.GetPlayer(index).BeginRound();
            }
            
        }

        #region Event recievers
        private void Current_OnPlayerMoveFinished(object sender, PlayerEventArgs e)
        {
            if (!e.IsActive) { return; }
            
            HighlightBehavior.UnmarkPreviouslyHighlightedPoints();

            bool playerLost = PLAYER_CONTROLLER.CheckIfPlayerHasLost(playerIndex);
            Agent player = PLAYER_CONTROLLER.GetPlayer(playerIndex);
            player.SetActive(false);

            if (PLAYER_CONTROLLER.HasMisterXLost())
            {
                GameEvents.Current.DetectivesWon(null, null);
            }
            else if (!playerLost || !PLAYER_CONTROLLER.HaveAllDetectivesLost())
            {
                HISTORY_CONTROLLER.AddHistoryItem(Round, player);
                roundState = ERound.TURN_END;

                playerIndex++;
                PlayRound();
            }
            else
            {
                GameEvents.Current.MisterXWon(null, null);
            }
        }

        private void Current_OnPlayerResigned(object sender, bool e)
        {
            PLAYER_CONTROLLER.GetAllAgents().FindAll(a => a is Player).ForEach(p => p.Data.HasLost = true);

            Agent activePlayer = PLAYER_CONTROLLER.GetAllAgents().FirstOrDefault(a => a is Player && a.Data.IsActive);
            if(activePlayer != null)
            {
                GameEvents.Current.PlayerMoveFinished(null, new PlayerEventArgs(activePlayer.Data));
            }
        }

        private void Current_OnTicketSelection_Approved(object sender, TicketEventArgs e)
        {
            var player = PLAYER_CONTROLLER.GetPlayer(playerIndex);
            player.StreetPath = e.Street;
            player.RemoveTicket(e.Ticket);
        }

        private void Current_OnTicketSelection_Canceled(object sender, MovementEventArgs e)
        {
            HighlightBehavior.HighlightAccesPoints(e.Player);
        }

        protected virtual void OnlyHighlightDestination(object sender, MovementEventArgs e)
        {
            HighlightBehavior.HighlightOnlyOne(e.TargetPosition);
        }

        protected void Current_OnMisterXWon(object sender, System.EventArgs e)
        {
            StopAllCoroutines();
            roundState = ERound.END;
        }

        protected void Current_OnDetectivesWon(object sender, System.EventArgs e)
        {
            StopAllCoroutines();
            roundState = ERound.END;
        }

        private void Current_OnPlayerItemClicked(object sender, string e)
        {
            Agent agent = PLAYER_CONTROLLER.GetAgentById(e);

            if (agent != null && (agent.Data.PlayerRole != EPlayerRole.MISTERX || detectionRounds.Contains(Round) || agent.GetType() == typeof(Player)))
            {
                Vector3 agentPosition = agent.GetTransform().position;
                CameraController.instance.SetPosition(new Vector3(agentPosition.x, 0f, agentPosition.z));
            }
        }
        #endregion

        protected void OnDestroy()
        {
            GameEvents.Current.OnPlayerMoveFinished -= Current_OnPlayerMoveFinished;
            GameEvents.Current.OnPlayerResigned -= Current_OnPlayerResigned;
            GameEvents.Current.OnDestinationSelected -= OnlyHighlightDestination;
            GameEvents.Current.OnTicketSelection_Canceled -= Current_OnTicketSelection_Canceled;
            GameEvents.Current.OnTicketSelection_Approved -= Current_OnTicketSelection_Approved;

            GameEvents.Current.OnDetectivesWon -= Current_OnDetectivesWon;
            GameEvents.Current.OnMisterXWon -= Current_OnMisterXWon;

            GameEvents.Current.OnPlayerItemClicked -= Current_OnPlayerItemClicked;
        }
    }
}