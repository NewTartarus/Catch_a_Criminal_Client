namespace ScotlandYard.Scripts.PlayerScripts
{
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Helper;
    using ScotlandYard.Scripts.Street;
    using UnityEngine;
    using UnityEngine.EventSystems;

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

        protected void Update()
        {
            if (Data.IsActive && Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform != null)
                    {
                        StreetPoint targetPoint = hit.transform.parent?.GetComponent<StreetPoint>();
                        if (targetPoint != null && targetPoint.IsHighlighted)
                        {
                            Player player = this.transform.GetComponent<Player>();
                            GameEvents.Current.DestinationSelected(null, new MovementEventArgs(player, targetPoint));
                        }
                    }
                }
            }
        }
    }
}

