namespace ScotlandYard.CustomEditor
{
	using System;
	using UnityEngine;
	using UnityEditor;
    using System.IO;
    using ScotlandYard.Scripts.Buildings;
    using ScotlandYard.Enums;

    public class BuildMeshImportEditor : EditorWindow
	{
		#region Members
		private string inputPath = "Assets/Models/BuildingObjects/import";
		private string outputPath = "Assets/Prefabs/3D/Buildings/default";
		private BuildingController controller;
		private Material material;

		Vector2 scroll;

		private string outputLog;
		#endregion

		#region Methods
		[MenuItem("Tools/Building/Mesh Importer")]
		public static void ShowWindow()
		{
			GetWindow<BuildMeshImportEditor>("Building Mesh Importer");
		}

		private void OnGUI()
        {
			GUILayout.Label("Create Building Prefabs", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			inputPath  = EditorGUILayout.TextField("Input Path", inputPath);
			outputPath = EditorGUILayout.TextField("Output Path", outputPath);
			material = EditorGUILayout.ObjectField("Building Material", material, typeof(Material), true) as Material;
			controller = EditorGUILayout.ObjectField("Building Controller", controller, typeof(BuildingController), true) as BuildingController;

			EditorGUILayout.Space();
			if (GUILayout.Button("Create Prefabs"))
            {
				CreatePrefabs(inputPath, outputPath);
			}

			EditorGUILayout.Space();
			GUILayout.Label("Log");
			scroll = EditorGUILayout.BeginScrollView(scroll);
			EditorGUILayout.TextArea(outputLog);
			EditorGUILayout.EndScrollView();
		}

		private void CreatePrefabs(string objectPath, string prefabsPath)
        {
			outputLog = "";

			var allObjectGuids = AssetDatabase.FindAssets("t:Object", new string[] { objectPath });
			
			if(this.material == null)
            {
				Debug.Log("Could not find material");
				return;
            }

			foreach (var guid in allObjectGuids)
			{
				AddToLog(CreateSinglePrefab(guid, prefabsPath, this.material));
			}
		}

		private string CreateSinglePrefab(string guid, string prefabsPath, Material material)
        {
			string assetPath = AssetDatabase.GUIDToAssetPath(guid);
			string name = Path.GetFileNameWithoutExtension(assetPath);

			Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);

			GameObject go = new GameObject(name);

			MeshFilter filter = go.AddComponent<MeshFilter>();
			filter.sharedMesh = mesh;

			MeshRenderer renderer = go.AddComponent<MeshRenderer>();
			renderer.material = material;

			BuildingPart part = go.AddComponent<BuildingPart>();
			SetBuildingPartsMembersByName(part, name);

			string path = Path.Combine(prefabsPath, name + ".prefab");

			GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, path, out bool success);

			if(!success)
            {
				Debug.Log($"The Prefab {name} could not be created in {path}");
				return "";
            }

			UnityEngine.Object.DestroyImmediate(go);
			AddPrefabToController(name, prefab);

			return name;
		}

		private void SetBuildingPartsMembersByName(BuildingPart part, string name)
        {
            string[] nameParts = name.Split('_');

			if(nameParts.Length >= 4)
            {
				part.Id = Convert.ToByte(nameParts[0], 2);
				part.Variant = GetVariantByName(nameParts[2]);
				part.Index = Convert.ToByte(nameParts[3], 2);
			}

			if (nameParts.Length >= 5)
			{
				part.IsTop    = "t".Equals(nameParts[4]);
				part.IsBottom = "b".Equals(nameParts[4]);
			}
		}

		private void AddPrefabToController(string goName, GameObject prefab)
        {
			if (controller == null) { return; }

			BuildingPart part = prefab.GetComponent<BuildingPart>();

			string[] nameParts = goName.Split('_');
			if (nameParts.Length >= 2)
            {
                controller.AddBuildingPartToPackage(GetTypeByName(nameParts[1]), part);
			}
        }

		private EBuildingType GetTypeByName(string typeName)
        {
			switch (typeName)
            {
				case "default":
					return EBuildingType.DEFAULT;
				default:
					return EBuildingType.DEFAULT;
            }
        }

		private EBuildingVariant GetVariantByName(string variantName)
		{
			switch (variantName)
			{
				case "w":
					return EBuildingVariant.WALL;
				case "r":
					return EBuildingVariant.ROOF;
				case "g":
					return EBuildingVariant.GROUND;
				case "win":
					return EBuildingVariant.WINDOW;
				case "d":
					return EBuildingVariant.DOOR;
				default:
					return EBuildingVariant.WALL;
			}
		}

		private void AddToLog(string prefabName)
        {
			if (String.IsNullOrEmpty(prefabName)) { return; }
            
			if (outputLog.Length == 0)
            {
				outputLog += $"Created the Prefab {prefabName}";
			}
            else
            {
				outputLog += $"\nCreated the Prefab {prefabName}";
			}
        }
		#endregion
	}
}