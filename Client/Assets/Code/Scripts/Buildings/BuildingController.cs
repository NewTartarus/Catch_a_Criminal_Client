namespace ScotlandYard.Scripts.Buildings
{
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
    using ScotlandYard.Enums;

    public class BuildingController : MonoBehaviour
	{
		#region Members
		[SerializeField] private List<Building> buildingList = new List<Building>();
		[SerializeField] private List<BuildPackage> packages = new List<BuildPackage>();
		#endregion

		#region Methods
		protected void Awake()
		{
			foreach (Building building in buildingList)
            {
				Transform buildingTrans = building.transform;

                // place parts
                List<BuildPartData> modelList = building.Build();
				foreach (var bpd in modelList)
                {
					if(bpd.modelType == 0 && bpd.variant != EBuildingVariant.GROUND) { continue; }

					BuildPackage pack = packages.FirstOrDefault(p => p.BuildingType == building.BuildingType);
					
					PlaceBuildingPart(pack, bpd, GetBaseVariant(bpd.variant), buildingTrans);

					if (bpd.variant == EBuildingVariant.WINDOW || bpd.variant == EBuildingVariant.DOOR)
                    {
						PlaceBuildingPart(pack, bpd, bpd.variant, buildingTrans);
					}
				}

				// temporarily set position and rotation of the building to zero to make matrix math easier
				Vector3 position = buildingTrans.position;
				Quaternion rotation = buildingTrans.rotation;
				buildingTrans.position = Vector3.zero;
				buildingTrans.rotation = Quaternion.Euler(0, 0, 0);

				// combine parts
				MeshFilter[] meshFilters = building.gameObject.GetComponentsInChildren<MeshFilter>();
                CombineInstance[] combine = new CombineInstance[meshFilters.Length-1];
                for (int i = 1; i < meshFilters.Length; i++) // starting at 1 because GetComponentsInChildren also gets the component of the parent
                {
                    combine[i-1].mesh = meshFilters[i].sharedMesh;
                    combine[i-1].transform = meshFilters[i].transform.localToWorldMatrix;
                    meshFilters[i].gameObject.SetActive(false);
                }

                MeshFilter buildingMF = buildingTrans.GetComponent<MeshFilter>();
                buildingMF.mesh = new Mesh();
                buildingMF.mesh.CombineMeshes(combine, true, true);

                // remove the single parts
                for (int i = meshFilters.Length - 1; i > 0; i--)
                {
                    GameObject.Destroy(meshFilters[i].gameObject);
                }

                // return to orginal position and rotation
                buildingTrans.position = position;
				buildingTrans.rotation = rotation;

				// set the base texture of the material
				MeshRenderer mr = building.GetComponent<MeshRenderer>();
				mr.material.SetTexture("_BaseMap", building.Image);

				// remove the Building component, since it is no longer needed
				GameObject.Destroy(buildingTrans.gameObject.GetComponent<Building>());
			}
		}

		protected void PlaceBuildingPart(BuildPackage package, BuildPartData bpd, EBuildingVariant variant, Transform buildingTrans)
        {
			List<BuildingPart> objList = package.Parts.FindAll(o => o.Id == bpd.modelType && o.Variant == variant && !o.IsBottom && !o.IsTop);
			if (objList.Count > 0)
			{
				PlaceBuildingPart(bpd, objList, buildingTrans);
			}

			// place top
			if (bpd.hasTop)
			{
				objList = package.Parts.FindAll(o => o.Id == bpd.modelType && o.Variant == variant && o.IsTop);
				if (objList.Count > 0)
				{
					PlaceBuildingPart(bpd, objList, buildingTrans);
				}
			}

			// place bottom
			if (bpd.hasBottom)
			{
				EBuildingVariant vari = bpd.isGrounded ? EBuildingVariant.GROUND : variant;
				objList = package.Parts.FindAll(o => o.Id == bpd.modelType && o.Variant == vari && (bpd.isGrounded || o.IsBottom));
				if (objList.Count > 0)
				{
					PlaceBuildingPart(bpd, objList, buildingTrans);
				}
			}
		}

		protected void PlaceBuildingPart(BuildPartData bpd, List<BuildingPart> objList, Transform buildingTrans)
        {
			BuildingPart obj = objList[Random.Range(0, objList.Count)];
			GameObject go = GameObject.Instantiate(obj.Object, buildingTrans);
			go.transform.localPosition = bpd.localPosition;
			go.transform.localRotation = bpd.GetRotation();
		}

		protected virtual bool CheckVariants(EBuildingVariant variantA, EBuildingVariant variantB)
        {
			bool returnVal = false;

			if(variantA == variantB)
            {
				returnVal = true;
            }
			else if ((variantA == EBuildingVariant.WALL || variantA == EBuildingVariant.DOOR || variantA == EBuildingVariant.WINDOW) 
				  && (variantB == EBuildingVariant.WALL || variantB == EBuildingVariant.DOOR || variantB == EBuildingVariant.WINDOW))
            {
				returnVal = true;
            }

			return returnVal;
        }

		protected virtual EBuildingVariant GetBaseVariant(EBuildingVariant variant)
        {
			if (variant == EBuildingVariant.WINDOW || variant == EBuildingVariant.DOOR)
            {
				return EBuildingVariant.WALL;
            }
			else
            {
				return variant;
            }
        }

		public bool AddBuildingPartToPackage(EBuildingType type, BuildingPart part)
        {
			if (part == null)
            {
				return false;
            }

			if (this.packages == null)
            {
				this.packages = new List<BuildPackage>();
            }

			BuildPackage package = this.packages.FirstOrDefault(p => p.BuildingType == type);
			if (package == null)
            {
				package = new BuildPackage(type);
				this.packages.Add(package);
            }

			if (!package.Parts.Any(p => p.Equals(part)))
            {
				package.Parts.Add(part);
				return true;
			}

			return false;
        }
		#endregion
	}
}