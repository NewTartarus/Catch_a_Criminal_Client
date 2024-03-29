﻿namespace ScotlandYard.Scripts.PlayerScripts
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Helper;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class AIPlayer : Agent
    {
        [SerializeField] protected EDifficulty difficulty;

        public EDifficulty Difficulty
        {
            get => difficulty;
            set => difficulty = value;
        }

        public override void BeginRound()
        {
            switch(difficulty)
            {
                case EDifficulty.EASY:
                    MoveRandomly();
                    break;
                default:
                    break;
            }
        }

        protected void MoveRandomly()
        {
            // determine street
            List<IStreetPoint> targets = MovementHelper.GetTargets(this);
            if(targets.Count > 0)
            {
                int index = System.Convert.ToInt32(Random.Range(0, targets.Count));

                // pay ticket
                IStreet street = Data.CurrentPosition.GetPath(targets[index]);
                var cost = street.TicketCosts.Where(c => HasTicket(c)).ToArray();
                RemoveTicket(cost[System.Convert.ToInt32(Random.Range(0, cost.Length))]);

                // move
                StartCoroutine(nameof(Move), street);
            }
            else
            {
                Data.HasLost = true;
                GameEvents.Current.PlayerMoveFinished(this, new PlayerEventArgs(this.Data));
            }
        }
    }
}
