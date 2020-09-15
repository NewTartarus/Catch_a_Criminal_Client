using ScotlandYard.Enums;
using ScotlandYard.Events;
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
            foreach (Player player in playerList)
            {
                player.GeneratePlayerId();

                if(player.type == EPlayerType.DETECTIVE)
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

            GameEvents.current.OnMakeNextMove += Current_OnMakeNextMove;
        }

        private void Current_OnMakeNextMove(object sender, int args)
        {
            Player player = GetPlayer(args);
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
    }
}
