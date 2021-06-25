namespace ScotlandYard.Scripts.Events
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.PlayerScripts;
    using System;

    public class PlayerEventArgs : EventArgs
    {
        public string Name { get; set; }
        public EPlayerType Type { get; set; }

        public PlayerEventArgs(PlayerData playerdata)
        {
            this.Name = playerdata.AgentName;
            this.Type = playerdata.PlayerType;
        }
    }
}
