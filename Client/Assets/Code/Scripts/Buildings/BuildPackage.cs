namespace ScotlandYard.Scripts.Buildings
{
    using ScotlandYard.Enums;
    using System;
	using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
	public class BuildPackage
	{
		#region Members
		[SerializeField] protected EBuildingType buildingType;
		[SerializeField] protected List<BuildingPart> parts = new List<BuildingPart> ();
        #endregion

        #region Properties
        public EBuildingType BuildingType => buildingType;
        public List<BuildingPart> Parts => parts;
        #endregion

        #region Methods
        public BuildPackage(EBuildingType type)
        {
            this.buildingType = type;
        }
        #endregion
    }
}