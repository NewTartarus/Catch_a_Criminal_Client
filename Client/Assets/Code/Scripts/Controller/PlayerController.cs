namespace ScotlandYard.Scripts.Controller
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Helper;
    using ScotlandYard.Scripts.PlayerScripts;
    using ScotlandYard.Scripts.Street;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] protected List<Agent> agentList;

        public void Init()
        {
            // if multiple agents are misterX select a random one
            List<Agent> misterXList = agentList.FindAll(a => a.Data.PlayerType == EPlayerType.MISTERX);
            if(misterXList.Count > 1)
            {
                int index = UnityEngine.Random.Range(0, misterXList.Count);
                for (int i = 0; i < misterXList.Count; i++)
                {
                    if(i != index)
                    {
                        misterXList[i].Data.PlayerType = EPlayerType.DETECTIVE;
                    }
                }
            }
            else if(misterXList.Count == 0)
            {
                int index = UnityEngine.Random.Range(0, agentList.Count);
                agentList[index].Data.PlayerType = EPlayerType.MISTERX;
            }

            // hide misterX
            HidePlayer(GetMisterX(), false);

            // order all agents (misterX first then all detectives randomly)
            agentList = agentList.OrderBy(a => a.Data.PlayerType).ThenBy(a => UnityEngine.Random.Range(0,10)).ToList();

            // give all players their tickets
            foreach (Agent agent in agentList)
            {
                agent.Init();

                if (agent.Data.PlayerType == EPlayerType.DETECTIVE)
                {
                    agent.AddTickets(ETicket.TAXI, 10);
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
            GameEvents.Current.OnDetectiveTicketRemoved += Current_OnDetectiveTicketRemoved;
        }

        protected void Current_OnDetectiveTicketRemoved(object sender, ETicket e)
        {
            Agent misterX = agentList.FirstOrDefault(p => p.Data.PlayerType == EPlayerType.MISTERX);
            misterX.AddTickets(e, 1);
            GameEvents.Current.TicketUpdated(null, new TicketUpdateEventArgs(new List<PlayerData>() { misterX.Data, ((Agent)sender).Data}));
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

        public Agent GetMisterX()
        {
            return agentList.FirstOrDefault(a => a.Data.PlayerType == EPlayerType.MISTERX);
        }

        public int GetPlayerAmount()
        {
            return agentList.Count;
        }

        public List<Agent> GetAllAgents()
        {
            return agentList;
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

                if(p.Data.PlayerType == EPlayerType.DETECTIVE)
                {
                    positions[i].GetComponent<StreetPoint>().IsOccupied = true;
                }
            }

            return true;
        }

        public bool CheckIfPlayerHasLost(int playerIndex)
        {
            Agent current = GetPlayer(playerIndex);

            if (!current.Data.HasLost)
            {
                var targets = MovementHelper.GetTargets(current);
                if (targets.Count == 0)
                {
                    current.Data.HasLost = true;
                }

                // check if the current player has captured MisterX
                if (current.Data.PlayerType != EPlayerType.MISTERX)
                {
                    Agent misterX = GetMisterX();
                    if (misterX != null)
                    {
                        misterX.Data.HasLost = string.Equals(current.Position.GetComponent<StreetPoint>()?.name, misterX.Position.GetComponent<StreetPoint>()?.name);
                    }
                }
            }

            return current.Data.HasLost;
        }

        public bool HaveAllDetectivesLost()
        {
            int availableDete = agentList.Count(p => !p.Data.HasLost && p.Data.PlayerType == EPlayerType.DETECTIVE);
            return availableDete == 0;
        }

        public bool HasMisterXLost()
        {
            return agentList.Any(p => p.Data.PlayerType == EPlayerType.MISTERX && p.Data.HasLost);
        }

        public void HidePlayer(Agent agent, bool hide)
        {
            if(!agent.GetType().Name.Equals(nameof(Player)))
            {
                for (int i = 0; i < agent.transform.childCount; i++)
                {
                    agent.transform.GetChild(i).gameObject.SetActive(hide);
                }
            }
        }

        protected void OnDestroy()
        {
            GameEvents.Current.OnMakeNextMove -= Current_OnMakeNextMove;
            GameEvents.Current.OnDetectiveTicketRemoved -= Current_OnDetectiveTicketRemoved;
            HighlightBehavior.Destroy();
        }
    }
}
