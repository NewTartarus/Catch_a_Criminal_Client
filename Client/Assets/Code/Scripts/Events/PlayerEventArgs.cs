using ScotlandYard.Enums;
using ScotlandYard.Scripts.PlayerScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScotlandYard.Events
{
    public class PlayerEventArgs : EventArgs
    {
        public string Name { get; set; }
        public EPlayerType Type { get; set; }

        public PlayerEventArgs(Agent agent)
        {
            this.Name = agent.AgentName;
            this.Type = agent.PlayerType;
        }
    }
}
