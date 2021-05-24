using ScotlandYard.Enums;
using ScotlandYard.Events;
using ScotlandYard.Scripts.Street;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScotlandYard.Scripts.PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] protected List<Player> playerList;

        public void Init()
        {
            // give all players their tickets
            foreach (Player player in playerList)
            {
                player.GeneratePlayerId();

                if(player.PlayerType == EPlayerType.DETECTIVE)
                {
                    player.AddTickets(ETicket.TAXI, 10);
                    player.AddTickets(ETicket.BUS, 8);
                    player.AddTickets(ETicket.UNDERGROUND, 4);
                }
                else
                {
                    player.AddTickets(ETicket.TAXI, 4);
                    player.AddTickets(ETicket.BUS, 3);
                    player.AddTickets(ETicket.UNDERGROUND, 3);
                    player.AddTickets(ETicket.BLACK_TICKET, playerList.Count - 1);
                    player.AddTickets(ETicket.DOUBLE_TICKET, 2);
                }
                
            }

            GameEvents.Current.OnMakeNextMove += Current_OnMakeNextMove;
            GameEvents.Current.OnPlayerLost += Current_OnPlayerLost;
        }

        protected void Current_OnMakeNextMove(object sender, int args)
        {
            Player player = GetPlayer(args);
        }

        protected void Current_OnPlayerLost(object sender, PlayerEventArgs e)
        {
            int availableDete = playerList.Count(p => !p.HasLost && p.PlayerType == EPlayerType.DETECTIVE);
            if (availableDete == 0)
            {
                GameEvents.Current.MisterXWon(null, null);
            }
        }

        public Dictionary<ETicket, int> GetTicketsFromPlayer(int index)
        {
            if (playerList.Count - 1 >= index)
            {
                return playerList[index].GetTickets();
            }

            return new Dictionary<ETicket, int>();
        }

        public Player GetPlayer(int index)
        {
            if(playerList.Count -1 >= index)
            {
                return playerList[index];
            }

            return null;
        }

        public int GetPlayerAmount()
        {
            return playerList.Count;
        }

        public bool SetPlayerStartingPosition(GameObject[] positions)
        {
            if(playerList.Count > positions.Length)
            {
                return false;
            }

            for(int i = 0; i < GetPlayerAmount(); i++)
            {
                var p = GetPlayer(i);
                p.position = positions[i];
                p.transform.position = positions[i].transform.position;
            }

            return true;
        }

        public bool CheckIfPlayerHasLost(int playerIndex)
        {
            return CheckIfPlayerHasLost(playerList[playerIndex]);
        }

        public bool CheckIfPlayerHasLost(Player player)
        {
            StreetPoint playerPosition = player.position.GetComponent<StreetPoint>();
            var targets = playerPosition.GetStreetTargets(player);
            if(targets.Count == 0)
            {
                player.HasLost = true;
            }

            return player.HasLost;
        }
    }
}
