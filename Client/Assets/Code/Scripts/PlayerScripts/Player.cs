namespace ScotlandYard.Scripts.PlayerScripts
{
    using ScotlandYard.InputSystem;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Helper;
    using ScotlandYard.Scripts.Street;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.InputSystem;

    public class Player : Agent
    {
        private Camera mainCamera;
        private CaCInputControls controls;

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

        protected override void Awake()
        {
            base.Awake();

            mainCamera = Camera.main;
            controls = new CaCInputControls();

            controls.Player.Selection.performed += Selected;
            controls.Player.Selection.Enable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            controls.Player.Selection.performed -= Selected;
        }

        public override void BeginRound()
        {
            HighlightBehavior.HighlightAccesPoints(this);
        }

        private void Selected(InputAction.CallbackContext inputValue)
        {
            if (!Data.IsActive || EventSystem.current.IsPointerOverGameObject()) { return; }

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
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

