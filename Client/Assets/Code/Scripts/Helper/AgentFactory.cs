namespace ScotlandYard.Scripts
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.GameSettings;
    using ScotlandYard.Scripts.PlayerScripts;
	using System.Collections.Generic;
    using UnityEngine;

    public class AgentFactory
	{
		public static List<Agent> GenerateAgents(GameObject malePrefab, GameObject femalePrefab, GameObject aiPrefab, SettingsSO settings)
        {
			List<Agent> agents = new List<Agent>();

			foreach(PlayerSetting ps in settings.PlayerSettings)
            {
				PlayerData data = new PlayerData(ps.PlayerName, ps.Role, ps.PlayerColor);
				GameObject go;
				Agent agent;

				switch(ps.Type)
                {
					case EPlayerType.PLAYER:
						go = GameObject.Instantiate(malePrefab);
						agent = go.AddComponent<Player>();
						break;
					case EPlayerType.AI:
						go = GameObject.Instantiate(aiPrefab);
						agent = go.AddComponent<AIPlayer>();
						((AIPlayer)agent).Difficulty = settings.Difficulty;
						break;
					default:
						go = GameObject.Instantiate(aiPrefab);
						agent = go.AddComponent<AIPlayer>();
						((AIPlayer)agent).Difficulty = settings.Difficulty;
						break;
                }

				AgentIndicator indicator = go.GetComponentInChildren<AgentIndicator>(true);
				agent.SetDefaultValues(data, settings.AgentSpeed, indicator);
				agents.Add(agent);
            }

			return agents;
        }
	}
}