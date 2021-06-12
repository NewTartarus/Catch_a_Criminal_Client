using ScotlandYard.Enums;
using ScotlandYard.Events;
using ScotlandYard.Interface;
using ScotlandYard.Scripts.Helper;
using ScotlandYard.Scripts.Street;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScotlandYard.Scripts.PlayerScripts
{
    public class AIPlayer : Agent
    {
        [SerializeField] protected EDifficulty difficulty;
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
            StreetPoint currentPoint = Position.GetComponent<StreetPoint>();
            List<GameObject> targets = MovementHelper.GetTargets(this);
            if(targets.Count > 0)
            {
                int index = System.Convert.ToInt32(Random.Range(0, targets.Count));

                // pay ticket
                IStreet street = currentPoint.GetPathByPosition(Position, targets[index]);
                var cost = street.ReturnTicketCost().Where(c => HasTicket(c)).ToArray();
                RemoveTicket(cost[System.Convert.ToInt32(Random.Range(0, cost.Length))]);

                // move
                StartCoroutine(nameof(Move), street);
            }
            else
            {
                HasLost = true;
                GameEvents.Current.PlayerMoveFinished(this, new PlayerEventArgs(this));
            }
        }
    }
}
