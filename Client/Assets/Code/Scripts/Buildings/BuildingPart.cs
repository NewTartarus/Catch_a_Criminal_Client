namespace ScotlandYard.Scripts.Buildings
{
	using System;
    using ScotlandYard.Enums;
    using UnityEngine;

    [Serializable]
	public class BuildingPart : MonoBehaviour
	{
		#region Members
		[SerializeField] protected byte id;
		[SerializeField] protected EBuildingVariant variant;
		[SerializeField] protected byte index;
		[SerializeField] protected bool isTop;
		[SerializeField] protected bool isBottom;
		#endregion

		#region Properties
		public GameObject Object => gameObject;
		public byte Id
        {
			get => id;
			set => id = value;
        }
		public EBuildingVariant Variant
        {
			get => variant;
			set => variant = value;
        }
		public byte Index
        {
			get => index;
			set => index = value;
        }

		public bool IsTop
        {
			get => isTop;
			set => isTop = value;
        }

		public bool IsBottom
        {
			get => isBottom;
			set => isBottom = value;
        }
        #endregion

        #region Methods
        public override bool Equals(object other)
        {
            if (other is BuildingPart otherPart)
            {
				if (otherPart.Id == this.Id && otherPart.Index == this.Index && otherPart.Variant == this.Variant 
					&& otherPart.IsTop == this.IsTop && otherPart.IsBottom == this.IsBottom)
                {
					return true;
                }
            }

			return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}