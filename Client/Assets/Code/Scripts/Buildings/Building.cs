namespace ScotlandYard.Scripts.Buildings
{
	using ScotlandYard.Enums;
	using System.Collections.Generic;
	using UnityEngine;

	public class Building : MonoBehaviour
	{
		#region Members
		[SerializeField] private Vector3             size;
		[SerializeField] private List<bool>          voxels     = new List<bool>();
		[SerializeField] private List<BuildPartData> buildParts = new List<BuildPartData>();
        [SerializeField] private EBuildingType       buildingType;
		[SerializeField] private Texture2D           image;
		#endregion

		#region Properties
		public Vector3 Size
		{
			get => size;
		}

		public List<BuildPartData> BuildParts
		{
			get => buildParts;
		}

		public EBuildingType BuildingType
        {
			get => buildingType;
        }

		public Texture2D Image
        {
			get => image;
        }
		#endregion

		#region Methods
		public void PlaceParts()
		{
			int listSize = (int)size.x * (int)size.y * (int)size.z;
			int dif = buildParts.Count - listSize;

			if (dif < 0)
			{
				dif *= -1;
				for (int i = 0; i < dif; i++)
				{
					BuildPartData bp   = new BuildPartData();
					bp.activeSides = new bool[6];

					buildParts.Add(bp);
				}
			}
			else if (dif > 0)
			{
				for (int i = (buildParts.Count - 1); i >= listSize; i--)
				{
					buildParts.RemoveAt(i);
				}
			}

			listSize = (int)(size.x + 1) * (int)size.y * (int)(size.z + 1);
			dif = voxels.Count - listSize;

			if (dif < 0)
			{
				dif *= -1;
				for (int i = 0; i < dif; i++)
				{
					voxels.Add(false);
				}
			}
			else if (dif > 0)
			{
				for (int i = (voxels.Count - 1); i >= listSize; i--)
				{
					voxels.RemoveAt(i);
				}
			}

			SetBuildingPositions();
		}

		public void SetBuildingPositions()
        {
			int index = 0;
			for (int y = 0; y < size.y; y++)
			{
				for (float z = (size.z / 2 * -1); z < (size.z / 2); z++)
				{
					for (float x = (size.x / 2 * -1); x < (size.x / 2); x++)
					{
                        buildParts[index].localPosition = new Vector3(x * 2 + 1, y * 2, z * 2 + 1);
						index++;
					}
				}
			}
		}

		public void UpdateBuildingParts(bool updatePositions = false)
		{
			int sizeX = (int)size.x;
			int sizeZ = (int)size.z;
			int sizeY = (int)size.y;

			if (updatePositions)
            {
				SetBuildingPositions();
			}

			int index = 0;
			for (int y = 0; y < sizeY; y++)
            {
				for (int z = 0; z < sizeZ; z++)
                {
					for (int x = 0; x < sizeX; x++)
                    {
						BuildPartData bp = buildParts[index];
						bp.activeSides = new bool[4];

						if (bp.isActive)
						{
							int[] indices = new int[4];
							indices[0] = index + z + y * (sizeX + sizeZ + 1);
							indices[1] = indices[0] + 1;
							indices[2] = indices[0] + sizeX + 1;
							indices[3] = indices[2] + 1;

							bp.activeSides[0] = voxels[indices[0]];
							bp.activeSides[1] = voxels[indices[1]];
							bp.activeSides[2] = voxels[indices[2]];
							bp.activeSides[3] = voxels[indices[3]];
						}

						index++;
                    }
                }
            }

			for (int i = 0; i < buildParts.Count; i++)
			{
				BuildPartData bp = buildParts[i];

				if (bp.isActive)
				{
					int[] indices = new int[2];
					indices[0] = i - sizeX * sizeZ;
					indices[1] = i + sizeX * sizeZ;

					bp.hasBottom  = indices[0] < 0 || !buildParts[indices[0]].isActive;
					bp.isGrounded = indices[0] < 0;
					bp.hasTop     = indices[1] >= buildParts.Count || !buildParts[indices[1]].isActive || bp.GetModelId() > buildParts[indices[1]].GetModelId();
                }
			}
		}

		public List<BuildPartData> Build()
        {
			UpdateBuildingParts(true);

			for (int i = 0; i < buildParts.Count; i++)
            {
				BuildPartData bp = buildParts[i];

				var modelAndRotation = MarchingSquaresLookUp.GetModelAndRotation(bp.GetModelId());
				bp.modelType = modelAndRotation.Item1;
				bp.rotation = modelAndRotation.Item2;
			}

			return this.buildParts;
		}

		public string GetByteString(int index)
		{

			return buildParts[index].GetBitString();
		}

		protected bool AreConnectedParts(EBuildingVariant own, EBuildingVariant other)
        {
			bool connected = false;

			if (own == other)
            {
				connected = true;
            }
			else if (own == EBuildingVariant.GROUND || own == EBuildingVariant.WALL || own == EBuildingVariant.WINDOW || own == EBuildingVariant.DOOR)
            {
				connected = other == EBuildingVariant.WALL || other == EBuildingVariant.WINDOW || other == EBuildingVariant.DOOR;
			}

			return connected;
        }

        #region Unity Editor
#if UNITY_EDITOR
        [SerializeField] private bool debugShowVoxels;
		[SerializeField, Range(-1, 15)] private int  debugShowLevel = -1;
		private void OnDrawGizmos()
		{
			Gizmos.matrix = transform.localToWorldMatrix;

			if (debugShowVoxels)
            {
				DrawVoxelGizmos();
			}
			else
            {
				DrawBuildingPartGizmos();
            }
		}

		protected void DrawVoxelGizmos()
		{
			float gizmoSize = 1f;
			int index = 0;

			for (int y = 0; y < size.y; y++)
			{
				for (float z = (size.z / 2 * -1); z < (size.z / 2) + 1; z++)
				{
					for (float x = (size.x / 2 * -1); x < (size.x / 2) + 1; x++)
					{
						Gizmos.color = Color.white;

						if (voxels[index] && ShowFloor(y))
						{
							Gizmos.DrawCube(new Vector3(x * 2, y * 2 + 1, z * 2), new Vector3(gizmoSize / 2, gizmoSize / 2, gizmoSize / 2));
						}

						index++;
					}
				}
			}

			gizmoSize = 2f;
			index = 0;

			for (int y = 0; y < size.y; y++)
			{
				for (float z = (size.z / 2 * -1); z < (size.z / 2); z++)
				{
					for (float x = (size.x / 2 * -1); x < (size.x / 2); x++)
					{
						Gizmos.color = Color.grey;
						Gizmos.DrawWireCube(new Vector3(x * 2 + 1, y * 2 + 1, z * 2 + 1), new Vector3(gizmoSize, gizmoSize, gizmoSize));
						index++;
					}
				}
			}
		}

		protected void DrawBuildingPartGizmos()
        {
			float gizmoSize = 2f;
			int index = 0;

			for (int y = 0; y < size.y; y++)
			{
				for (float z = (size.z / 2 * -1); z < (size.z / 2); z++)
				{
					for (float x = (size.x / 2 * -1); x < (size.x / 2); x++)
					{
						Gizmos.color = Color.grey;
						Gizmos.DrawWireCube(new Vector3(x * 2 + 1, y * 2 + 1, z * 2 + 1), new Vector3(gizmoSize, gizmoSize, gizmoSize));

						if (buildParts[index].isActive && ShowFloor(y))
						{
							Gizmos.color = GetGizmosColor(buildParts[index]);
							Gizmos.DrawCube(new Vector3(x * 2 + 1, y * 2 + 1, z * 2 + 1), new Vector3(gizmoSize / 2, gizmoSize / 2, gizmoSize / 2));
						}

						index++;
					}
				}
			}
		}

		protected Color GetGizmosColor(BuildPartData bp)
        {
			switch(bp.variant)
            {
				case EBuildingVariant.WALL:
					return Color.white;
				case EBuildingVariant.DOOR:
					return Color.blue;
				case EBuildingVariant.WINDOW:
					return Color.cyan;
				case EBuildingVariant.ROOF:
					return Color.red;
				case EBuildingVariant.GROUND:
					return Color.green;
				default:
					return Color.white;
			}
        }

		protected bool ShowFloor(int floor)
        {
			return !(debugShowLevel != -1 && floor != debugShowLevel);

		}
#endif
        #endregion
        #endregion
    }
}