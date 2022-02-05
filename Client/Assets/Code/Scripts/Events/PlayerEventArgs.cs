namespace ScotlandYard.Scripts.Events
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.PlayerScripts;
    using System;

    public class PlayerEventArgs : EventArgs
    {
        public string PlayerId { get; set; }
        public EPlayerRole PlayerRole { get; set; }
        public bool IsActive { get; set; }

        public PlayerEventArgs(PlayerData playerdata)
        {
            this.PlayerId = playerdata.ID;
            this.PlayerRole = playerdata.PlayerRole;
            this.IsActive = playerdata.IsActive;
        }
    }
}
