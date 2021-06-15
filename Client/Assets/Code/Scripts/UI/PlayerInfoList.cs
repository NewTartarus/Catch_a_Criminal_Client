using ScotlandYard.Scripts.PlayerScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScotlandYard.Scripts.UI
{
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
