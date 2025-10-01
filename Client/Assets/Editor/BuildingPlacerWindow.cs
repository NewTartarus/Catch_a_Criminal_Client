namespace ScotlandYard.Scripts
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.Buildings;
    using System;
	using System.Collections;
	using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
	
	public class BuildingPlacerWindow : EditorWindow
	{
		#region Members
		private BuildingController controller;
		private Transform          parentObj;
        private Material           buildingMaterial;
        private Texture2D          texture;
        private bool               isPark;
        private EBuildingType      type;
        #endregion

        #region Methods
        [MenuItem("Tools/Building/Placer")]
        public static void Open()
		{
			BuildingPlacerWindow window = GetWindow<BuildingPlacerWindow>("Building Placer");
		}

        public void OnGUI()
        {
            this.controller = EditorGUILayout.ObjectField("Building Controller", controller, typeof(BuildingController), true) as BuildingController;
            this.parentObj  = EditorGUILayout.ObjectField("Parent Transform", parentObj, typeof(Transform), true) as Transform;

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Building Properties:");
            EditorGUILayout.Space(2);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("is a Park");
            EditorGUILayout.Space();
            this.isPark           = EditorGUILayout.Toggle(isPark);
            GUILayout.EndHorizontal();

            this.type = (EBuildingType)EditorGUILayout.EnumPopup("Type", type);
            this.buildingMaterial = EditorGUILayout.ObjectField("Material", buildingMaterial, typeof(Material), true) as Material;
            this.texture = EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), true) as Texture2D;

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            if (GUILayout.Button("Create Building"))
            {
                CreateABuilding();
            }
        }

        public void CreateABuilding()
        {
            int    childCount = this.parentObj.childCount;
            string name       = this.isPark ? $"Park.{childCount:000}" : $"Building.{childCount:000}";
            
            GameObject go = new GameObject(name);

            go.transform.parent        = this.parentObj;
            go.transform.localPosition = Vector3.zero;

            go.AddComponent<MeshFilter>();

            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            Building     building = go.AddComponent<Building>();
            
            renderer.material     = this.buildingMaterial;
            building.IsPark       = this.isPark;
            building.Image        = this.texture;
            building.BuildingType = this.type;

            this.controller.AddBuilding(building);

            Selection.activeObject = go;
        }
        #endregion
    }
}