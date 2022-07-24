namespace ScotlandYard.Scripts.Buildings
{
	using ScotlandYard.Enums;
	using System;
	using UnityEngine;

	[Serializable]
    public class BuildPartData
    {
        public bool isActive;
		public bool hasTop;
		public bool hasBottom;
		public bool isGrounded;
		public byte modelType;
		public bool[] activeSides;
		public Vector3 localPosition;
		public EBuildingVariant variant;
		public float[] rotation;

		public Quaternion GetRotation()
        {
			return Quaternion.Euler(rotation[0], rotation[1], rotation[2]);
		}

		public String GetBitString()
        {
			string byteString = "";
			for (int i = this.activeSides.Length - 1; i >= 0; i--)
			{
				byteString += this.activeSides[i] ? "1" : "0";
			}

			return byteString;
		}

		public byte GetModelId()
        {
			return Convert.ToByte(GetBitString(), 2);
        }
	}
}
