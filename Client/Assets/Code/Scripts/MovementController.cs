namespace ScotlandYard.Scripts
{
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.PlayerScripts;
    using ScotlandYard.Scripts.Street;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class MovementController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(EventSystem.current.IsPointerOverGameObject())
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

