

namespace ScotlandYard.Scripts.MeshGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    public struct OrientedPoint
    {
		#region Members
		private Vector3 position;
		private Quaternion rotation;
		#endregion

		#region Properties
		public Vector3 Position { get => position; set => position = value; }
		public Quaternion Rotation { get => rotation; set => rotation = value; }
        #endregion

        public OrientedPoint(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public OrientedPoint(Vector3 position, Vector3 forwardVector)
        {
            this.position = position;
            this.rotation = Quaternion.LookRotation(forwardVector);
        }

        #region Methods
        public Vector3 LocalToWorldPosition(Vector3 localSpacePos)
        {
            return position + rotation * localSpacePos;
        }

        public Vector3 LocalToWorldVector(Vector3 localSpacePos)
        {
            return rotation * localSpacePos;
        }
        #endregion
    }
}
