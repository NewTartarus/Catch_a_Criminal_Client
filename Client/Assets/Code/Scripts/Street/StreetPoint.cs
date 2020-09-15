using ScotlandYard.Enums;
using ScotlandYard.Interface;
using ScotlandYard.Scripts.PlayerScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ScotlandYard.Scripts.Street
{
    public class StreetPoint : MonoBehaviour
    {
        public new string name;
        [SerializeField] protected TextMeshPro text;
        [SerializeField] protected GameObject highlightMesh;

        [SerializeField] protected List<IStreet> streetList = new List<IStreet>();

        #region Properties
        private bool _highlighted;
        public bool IsHighlighted
        {
            get => _highlighted;
            set
            {
                _highlighted = value;
                highlightMesh.SetActive(_highlighted);
            }
        }
        #endregion

        void Awake()
        {
            if (text != null)
            {
                text.text = name;
            }
        }

        public List<GameObject> GetStreetTargets(Player player)
        {
            List<GameObject> targets = new List<GameObject>();
            foreach (IStreet street in streetList)
            {
                bool playerHasTicket = false;
                foreach(ETicket ticket in street.ReturnTicketCost())
                {
                    if(player.HasTicket(ticket))
                    {
                        playerHasTicket = true;
                        break;
                    }
                }

                if(playerHasTicket)
                {
                    targets.Add(!street.StartPoint.Equals(this.gameObject) ? street.StartPoint : street.EndPoint);
                }
            }

            return targets;
        }

        internal IStreet GetPathByPosition(GameObject position, GameObject target)
        {
            foreach (IStreet path in streetList)
            {
                if ((position.Equals(path.StartPoint) && target.Equals(path.EndPoint)) || (position.Equals(path.EndPoint) && target.Equals(path.StartPoint)))
                {
                    return path;
                }
            }

            return null;
        }

        public void AddStreet(IStreet path)
        {
            streetList.Add(path);
        }

        public IStreet[] GetStreetArray()
        {
            return streetList.ToArray();
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public override string ToString()
        {
            return name;
        }
    }
}

