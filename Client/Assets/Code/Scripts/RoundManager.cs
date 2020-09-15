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
                    GameEvents.current.RoundHasEnded(this, round);
                }
            }
        }
        protected int playerIndex = 0;
        protected int[] detectionRounds = new int[] { 3, 8, 13, 18, 24};

        protected ERound roundState;

        protected void Start()
        {
            GameEvents.current.OnPlayerMoveFinished += Current_OnPlayerMoveFinished;
            GameEvents.current.OnDestinationSelected += OnlyHighlightDestination;
            GameEvents.current.OnTicketSelection_Canceled += Current_OnTicketSelection_Canceled;
            GameEvents.current.OnTicketSelection_Approved += Current_OnTicketSelection_Approved;

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
                    playerIndex = 0;
                    Round ++;
                }

                StartCoroutine(nameof(BeginPlayerRound), playerIndex);
            }
            else
            {
                GameEvents.current.MisterXWon(this, null);
            }
        }

        private void Current_OnPlayerMoveFinished(object sender, PlayerEventArgs e)
        {
            Debug.Log($"{e.Name}'s turn ended [{round}]");
            HighlightBehavior.UnmarkPreviouslyHighlightedPoints();
            roundState = ERound.TURN_END;

            playerIndex++;
            PlayRound();
        }

        protected IEnumerator BeginPlayerRound(int index)
        {
            if(roundState != ERound.MISTER_X_TURN && roundState != ERound.DETECTIVE_TURN)
            {
                Player player = PLAYER_CONTROLLER.GetPlayer(index);

                if (player.type == EPlayerType.MISTERX)
                {
                    roundState = ERound.MISTER_X_TURN;
                }
                else
                {
                    roundState = ERound.DETECTIVE_TURN;
                }

                roundMessage.DisplayMessage(GAME_TURN_STARTED, player.Name);

                yield return new WaitForSeconds(2f);

                roundMessage.HideMessage();

                HighlightBehavior.HighlightAccesPoints(PLAYER_CONTROLLER.GetPlayer(index));
            }
            
        }
    }
}