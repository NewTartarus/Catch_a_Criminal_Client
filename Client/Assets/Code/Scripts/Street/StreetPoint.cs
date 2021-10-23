namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Interfaces;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class StreetPoint : MonoBehaviour, IStreetPoint
    {
        [SerializeField] protected string streetPointName;
        [SerializeField] protected TextMeshPro text;
        [SerializeField] protected GameObject highlightMesh;
        protected bool isOccupied;

        [SerializeField] protected List<IStreet> streetList = new List<IStreet>();

        protected bool highlighted;
        protected GameObject ownGameObject;
        protected Transform ownTransform;

        #region Properties
        public string StreetPointName
        {
            get => streetPointName;
            set => streetPointName = value;
        }

        public bool IsHighlighted
        {
            get => highlighted;
            set
            {
                highlighted = value;
                highlightMesh.SetActive(highlighted);
            }
        }

        public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
        #endregion

        void Awake()
        {
            if (text != null)
            {
                text.text = streetPointName;
            }
        }

        public IStreet GetPath(IStreetPoint target)
        {
            foreach (IStreet path in streetList)
            {
                if ((this.Equals(path.StartPoint) && target.Equals(path.EndPoint)) || (this.Equals(path.EndPoint) && target.Equals(path.StartPoint)))
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
            if(ownGameObject == null)
            {
                ownGameObject = this.gameObject;
            }

            return ownGameObject;
        }

        public Transform GetTransform()
        {
            if (ownTransform == null)
            {
                ownTransform = this.transform;
            }

            return ownTransform;
        }

        public override string ToString()
        {
            return streetPointName;
        }

        public override bool Equals(object other)
        {
            if(other is StreetPoint sp && sp.StreetPointName.Equals(this.StreetPointName) &&
                sp.GetTransform().position == this.GetTransform().position)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

