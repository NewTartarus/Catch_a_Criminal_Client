namespace ScotlandYard.Scripts.UI.InGame
{
    using ScotlandYard.Scripts.PlayerScripts;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerInfoList : MonoBehaviour
    {
        [SerializeField] protected GameObject playerInfoPrefab;

        public void Init(List<Agent> agentList)
        {
            for(int i = 0; i < agentList.Count; i++)
            {
                PlayerData data = agentList[i].Data;
                GameObject child = Instantiate(playerInfoPrefab);
                child.GetComponent<PlayerInfoItem>()?.Init(data, i+1);
                child.transform.SetParent(this.transform, false);
            }
        }
    }
}
