using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScotlandYard.Scripts.Helper;
using ScotlandYard.Scripts.Street;
using ScotlandYard.Events;

namespace ScotlandYard.Scripts.PlayerScripts
{
    public class MovementController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
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
                            GameEvents.current.DestinationSelected(null, new MovementEventArgs(player, targetPoint));

                            //player.StreetPath = targetPoint.GetPathByPosition(player.position, hit.transform.parent.gameObject);
                        }
                    }
                }
            }
        }

        public void OnMouseDown()
        {
            CameraController.instance.followTransform = this.transform;
        }
    }
}

