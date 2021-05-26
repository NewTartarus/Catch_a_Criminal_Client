using ScotlandYard.Enums;
using ScotlandYard.Events;
using ScotlandYard.Scripts.Helper;
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
        [SerializeField] protected List<Agent> agentList;

        public void Init()
        {
            // give all players their tickets
            foreach (Agent agent in agentList)
            {
                agent.GeneratePlayerId();

                if(agent.PlayerType == EPlayerType.DETECTIVE)
                {
                    agent.AddTickets(ETicket.TAXI, 4); //
                    agent.AddTickets(ETicket.BUS, 8);
                    agent.AddTickets(ETicket.UNDERGROUND, 4);
                }
                else
                {
                    agent.AddTickets(ETicket.TAXI, 4);
                    agent.AddTickets(ETicket.BUS, 3);
                    agent.AddTickets(ETicket.UNDERGROUND, 3);
                    agent.AddTickets(ETicket.BLACK_TICKET, agentList.Count - 1);
                    agent.AddTickets(ETicket.DOUBLE_TICKET, 2);
                }
                
            }

            GameEvents.Current.OnMakeNextMove += Current_OnMakeNextMove;
        }

        protected void Current_OnMakeNextMove(object sender, int args)
        {
            Agent player = GetPlayer(args);
        }

        public Dictionary<ETicket, int> GetTicketsFromPlayer(int index)
        {
            if (agentList.Count - 1 >= index)
            {
                return agentList[index].GetTickets();
            }

            return new Dictionary<ETicket, int>();
        }

        public Agent GetPlayer(int index)
        {
            if(agentList.Count -1 >= index)
            {
                return agentList[index];
            }

            return null;
        }

        public int GetPlayerAmount()
        {
            return agentList.Count;
        }

        public bool SetPlayerStartingPosition(GameObject[] positions)
        {
            if(agentList.Count > positions.Length)
            {
                return false;
            }

            for(int i = 0; i < GetPlayerAmount(); i++)
            {
                var p = GetPlayer(i);
                p.Position = positions[i];
                p.transform.position = positions[i].transform.position;
            }

            return true;
        }

        public bool CheckIfPlayerHasLost(int playerIndex)
        {
            return CheckIfPlayerHasLost(agentList[playerIndex]);
        }

        public bool CheckIfPlayerHasLost(Agent player)
        {
            if(!player.HasLost)
            {
                StreetPoint playerPosition = player.Position.GetComponent<StreetPoint>();
                var targets = MovementHelper.GetTargets(player);
                if (targets.Count == 0)
                {
                    player.HasLost = true;
                }
            }

            return player.HasLost;
        }

        public bool HaveAllDetectivesLost()
        {
            int availableDete = agentList.Count(p => !p.HasLost && p.PlayerType == EPlayerType.DETECTIVE);
            return availableDete == 0;
        }
    }
}
