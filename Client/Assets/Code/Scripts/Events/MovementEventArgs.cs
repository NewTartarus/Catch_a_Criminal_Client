namespace ScotlandYard.Scripts.Events
{
    using ScotlandYard.Scripts.PlayerScripts;
    using ScotlandYard.Scripts.Street;
    using System;

    public class MovementEventArgs : EventArgs
    {
        public Agent Player { get; set; }
        public StreetPoint TargetPosition { get; set; }

        public MovementEventArgs(Agent player, StreetPoint point)
        {
            this.Player = player;
            this.TargetPosition = point;
        }
    }
}