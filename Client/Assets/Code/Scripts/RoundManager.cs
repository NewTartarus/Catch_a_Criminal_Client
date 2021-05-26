using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScotlandYard.Scripts.Helper;
using ScotlandYard.Scripts.Street;
using ScotlandYard.Scripts.PlayerScripts;
using ScotlandYard.Interface;
using ScotlandYard.Enums;
using TMPro;
using ScotlandYard.Events;
using ScotlandYard.Scripts.UI;
using UnityEngine.SceneManagement;

namespace ScotlandYard.Scripts
{
    public class RoundManager : MonoBehaviour
    {
        private const string GAME_TURN_STARTED = "game_turn_started";

        [SerializeField] private StreetController STREET_CONTROLLER;
        [SerializeField] private PlayerController PLAYER_CONTROLLER;

        [SerializeField] private RoundMessage roundMessage;
        [SerializeField] private TicketChooser ticketChooser;

        protected int round = 1;
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
        protected int playerIndex = 0;
        protected int[] detectionRounds = new int[] { 3, 8, 13, 18, 24};

        protected ERound roundState;

        protected void Start()
        {
            GameEvents.Current.OnPlayerMoveFinished += Current_OnPlayerMoveFinished;
            GameEvents.Current.OnDestinationSelected += OnlyHighlightDestination;
            GameEvents.Current.OnTicketSelection_Canceled += Current_OnTicketSelection_Canceled;
            GameEvents.Current.OnTicketSelection_Approved += Current_OnTicketSelection_Approved;

            GameEvents.Current.OnDetectivesWon += Current_OnDetectivesWon;
            GameEvents.Current.OnMisterXWon += Current_OnMisterXWon;

            StartCoroutine(nameof(StartInit));
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

        protected IEnumerator StartInit()
        {
            roundState = ERound.INITIALIZATION;

            Cursor.visible = true;

            STREET_CONTROLLER.Init();
            PLAYER_CONTROLLER.Init();
            ticketChooser.Init();

            PLAYER_CONTROLLER.SetPlayerStartingPosition(STREET_CONTROLLER.GetRandomPositions(PLAYER_CONTROLLER.GetPlayerAmount()));


            yield return new WaitForSeconds(0.5f);

            PlayRound();
        }

        protected void PlayRound()
        {
            if(Round <= 24)
            {
                if(playerIndex >= PLAYER_CONTROLLER.GetPlayerAmount())
                {
                    Debug.Log($"Round {Round} ended.");
                    playerIndex = 0;
                    Round ++;
                }

                if(!PLAYER_CONTROLLER.GetPlayer(playerIndex).HasLost)
                {
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

        private void Current_OnPlayerMoveFinished(object sender, PlayerEventArgs e)
        {
            Debug.Log($"{e.Name}'s turn ended [{round}]");
            HighlightBehavior.UnmarkPreviouslyHighlightedPoints();

            if (!PLAYER_CONTROLLER.CheckIfPlayerHasLost(playerIndex) || !PLAYER_CONTROLLER.HaveAllDetectivesLost())
            {
                roundState = ERound.TURN_END;

                playerIndex++;
                PlayRound();
            }
            else
            {
                GameEvents.Current.MisterXWon(null, null);
            }
        }

        protected IEnumerator BeginPlayerRound(int index)
        {
            if(roundState != ERound.MISTER_X_TURN && roundState != ERound.DETECTIVE_TURN)
            {
                Agent player = PLAYER_CONTROLLER.GetPlayer(index);

                if (player.PlayerType == EPlayerType.MISTERX)
                {
                    roundState = ERound.MISTER_X_TURN;
                }
                else
                {
                    roundState = ERound.DETECTIVE_TURN;
                }

                roundMessage.DisplayMessage(GAME_TURN_STARTED, player.AgentName);

                yield return new WaitForSeconds(2f);

                roundMessage.HideMessage();

                PLAYER_CONTROLLER.GetPlayer(index).BeginRound();
            }
            
        }

        protected void Current_OnMisterXWon(object sender, System.EventArgs e)
        {
            StopAllCoroutines();
            roundState = ERound.END;

            roundMessage.DisplayMessage("game_misterX_won", "game_game_end", () => GoBackToStart());
        }

        protected void Current_OnDetectivesWon(object sender, System.EventArgs e)
        {
            StopAllCoroutines();
            roundState = ERound.END;

            roundMessage.DisplayMessage("game_detectives_won", "game_game_end", () => GoBackToStart());
        }

        protected void GoBackToStart()
        {
            GameEvents.Reset();
            SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
        }
    }
}