using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScotlandYard.Interface;
using ScotlandYard.Enums;
using System;
using ScotlandYard.Events;
using ScotlandYard.Scripts.Helper;

namespace ScotlandYard.Scripts.PlayerScripts
{
    public class Player : Agent
    {
        public override IStreet StreetPath
        {
            get => streetPath;
            set
            {
                if (isMoving == false)
                {
                    streetPath = value;
                    if (streetPath != null)
                    {
                        StopCoroutine(nameof(Move));
                        StartCoroutine(nameof(Move), streetPath);
                    }
                }
            }
        }

        public override void BeginRound()
        {
            HighlightBehavior.HighlightAccesPoints(this);
        }
    }
}

