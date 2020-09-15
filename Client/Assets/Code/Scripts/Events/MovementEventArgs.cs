using ScotlandYard.Scripts.PlayerScripts;
using ScotlandYard.Scripts.Street;
using System;

namespace ScotlandYard.Events
{
    public class MovementEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public StreetPoint TargetPosition { get; set; }

        public MovementEventArgs(Player player, StreetPoint point)
        {
            this.Player = player;
            this.TargetPosition = point;
        }
    }
}