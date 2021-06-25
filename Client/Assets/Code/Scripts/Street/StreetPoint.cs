namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Interface;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class StreetPoint : MonoBehaviour
    {
        public new string name;
        [SerializeField] protected TextMeshPro text;
        [SerializeField] protected GameObject highlightMesh;
        protected bool isOccupied;

        [SerializeField] protected List<IStreet> streetList = new List<IStreet>();

        protected bool highlighted;

        #region Properties
        public bool IsHighlighted
        {
            get => highlighted;
            set
            {
                highlighted = value;
                highlightMesh.SetActive(highlighted);
            }
        }

        public bool IsOccupied { get; set; }
        #endregion

        void Awake()
        {
            if (text != null)
            {
                text.text = name;
            }
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

