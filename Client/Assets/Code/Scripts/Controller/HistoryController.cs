namespace ScotlandYard.Scripts.Controller
{
    using ScotlandYard.Enums;
	using ScotlandYard.Scripts.History;
	using ScotlandYard.Scripts.PlayerScripts;
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
    
    public class HistoryController : MonoBehaviour
	{
		[SerializeField] protected HistoryItemList historylist;
        protected Dictionary<string, HistoryItem> history = new Dictionary<string, HistoryItem>();
		protected int[] detectionRounds;

		public void Init(List<Agent> dataList, int[] detectionRounds)
        {
			this.detectionRounds = detectionRounds;
			foreach(Agent agent in dataList)
            {
				AddHistoryItem(0, agent);
            }
		}

        public void AddHistoryItem(int round, Agent agent)
        {
			PlayerData agentDataClone = agent.Data.Clone();
			ETicket payedTicket = DeterminePayedTicket(round, agentDataClone);
			HistoryItem item = new HistoryItem(round, payedTicket, agentDataClone);
			history.Add($"{round}-{agentDataClone.ID}", item);

			if(round > 0 && agentDataClone.PlayerType == EPlayerType.MISTERX)
            {
				historylist.AddVisibleItem(item, detectionRounds.Contains(round));
            }
		}

		protected ETicket DeterminePayedTicket(int round, PlayerData playerData)
        {
			ETicket returnTicket = ETicket.EMPTY;
			if(round > 0)
            {
				HistoryItem lastEntry = history[$"{round - 1}-{playerData.ID}"];

				if(playerData.PlayerType == EPlayerType.DETECTIVE)
                {
					returnTicket = playerData.Tickets.Where(ticket => ticket.Value == lastEntry.Data.Tickets[ticket.Key] - 1).FirstOrDefault().Key;
                }
				else if(playerData.PlayerType == EPlayerType.MISTERX)
                {
					returnTicket = playerData.Tickets.Where(ticket => ticket.Value == lastEntry.Data.Tickets[ticket.Key] - 1).FirstOrDefault().Key;

					if(returnTicket == ETicket.EMPTY)
                    {
						List<ETicket> ticketsAdded = history.Where(historyItem => historyItem.Key.StartsWith($"{round-1}-") && !historyItem.Key.EndsWith($"-{playerData.ID}"))
															.Select(historyItem => historyItem.Value.Ticket).ToList();

						Dictionary<ETicket, int> countedTickets = new Dictionary<ETicket, int>();
						Enum.GetValues(typeof(ETicket)).Cast<ETicket>().ToList().ForEach(ticket => countedTickets.Add(ticket, 0));

						foreach (ETicket ticket in ticketsAdded)
						{
							if (countedTickets.ContainsKey(ticket))
							{
								countedTickets[ticket]++;
							}
							else
							{
								countedTickets.Add(ticket, 1);
							}
						}

						returnTicket = playerData.Tickets.Where(ticket => ticket.Value == lastEntry.Data.Tickets[ticket.Key] + countedTickets[ticket.Key] - 1).FirstOrDefault().Key;
					}
				}
            }

			return returnTicket;
        }
    }
}

