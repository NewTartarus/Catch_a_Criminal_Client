namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Helper;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class StreetPoint : MonoBehaviour, IStreetPoint
    {
        [SerializeField] protected string streetPointName;
        [SerializeField] protected ECrossroadType type;
        [SerializeField] protected TextMeshPro text;
        [SerializeField] protected GameObject highlightMesh;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected MeshRenderer meshRenderer;
        [SerializeField] protected Material crossRoadMaterial;
        [SerializeField] protected bool invertNorthLinesOrder;
        [SerializeField] protected bool invertSouthLinesOrder;
        [SerializeField] protected bool invertWestLinesOrder;
        [SerializeField] protected bool invertEastLinesOrder;

        [SerializeField] protected float yRotation;
        protected bool isOccupied;

        [SerializeField] protected List<IStreet> streetList = new List<IStreet>();

        protected bool highlighted;
        protected GameObject ownGameObject;
        protected Transform ownTransform;
        protected Material spriteMaterial;

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

            spriteMaterial = spriteRenderer.material;
            meshRenderer.material = crossRoadMaterial;
            yRotation = meshRenderer.transform.localRotation.eulerAngles.y;
        }

        public void Init()
        {
            HashSet<ETicket> tickets = new HashSet<ETicket>();
            foreach(IStreet s in streetList)
            {
                tickets.UnionWith(s.TicketCosts);
            }

            spriteMaterial.SetInt("_ShowFirstColor", (tickets.Contains(ETicket.TAXI)) ? 1 : 0);
            spriteMaterial.SetInt("_ShowSecondColor", (tickets.Contains(ETicket.BUS)) ? 1 : 0);
            spriteMaterial.SetInt("_ShowThirdColor", (tickets.Contains(ETicket.UNDERGROUND)) ? 1 : 0);

            meshRenderer.material.SetMatrix("_DisplayedLines", this.GetCrossroadMatrix());
            meshRenderer.material.SetInt("_InvertNorthLinesOrder", invertNorthLinesOrder ? 1 : 0);
            meshRenderer.material.SetInt("_InvertSouthLinesOrder", invertSouthLinesOrder ? 1 : 0);
            meshRenderer.material.SetInt("_InvertWestLinesOrder",  invertWestLinesOrder  ? 1 : 0);
            meshRenderer.material.SetInt("_InvertEastLinesOrder",  invertEastLinesOrder ? 1 : 0);
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

        #region code for drawing lines at the crossroad-section
        /// <summary>
        /// The method <c>GetCrossroadMatrix</c> determines a matrix that is needed for the 
        /// "Crossroad"-material to show the correct ticket-lines on the crossroad-section.
        /// </summary>
        protected virtual Matrix4x4 GetCrossroadMatrix()
        {
            Dictionary<float, float> angleTicketMap = new Dictionary<float, float>();

            foreach (IStreet street in streetList)
            {
                if (street is Route)
                {
                    continue;
                }

                Vector3 wayPoint;

                if (street.StartPoint.Equals(this))
                {
                    wayPoint = street.GetWaypoint(0);
                }
                else
                {
                    wayPoint = street.GetWaypoint(street.GetNumberOfWaypoints() - 1);
                }

                if ((this.yRotation % 90) != 0)
                {
                    wayPoint = GetVector3FromRotation(this.GetTransform().position, wayPoint, this.yRotation);
                }

                #if UNITY_EDITOR
                // only for debugging purposes
                wpTransforms.Add(wayPoint);
                #endif

                float angle = GetWayPointAngle(this.GetTransform().position, wayPoint);
                float ticketMatrix = this.GetTicketMatrix(street.TicketCosts);

                angleTicketMap.Add(angle, ticketMatrix);
            }

            float minusRotation = this.yRotation % 360;

            switch (this.type)
            {
                case ECrossroadType.CROSS:
                    return GetCrossMatrix(angleTicketMap, minusRotation);
                case ECrossroadType.T_CROSS:
                    return GetTCrossMatrix(angleTicketMap, minusRotation);
                case ECrossroadType.CORNER:
                    return GetCornerMatrix(angleTicketMap, minusRotation);
                case ECrossroadType.LINE:
                    return GetLineMatrix(angleTicketMap, minusRotation);
                default:
                    return new Matrix4x4();
            }
        }

        /// <summary>
        /// The method <c>GetWayPointAngle</c> calculates the angle between <c>ownPosition</c> and <c>wayPoint</c>.
        /// </summary>
        /// <returns>angle in degrees</returns>
        protected virtual float GetWayPointAngle(Vector3 ownPosition, Vector3 wayPoint)
        {
            if (ownPosition.x == wayPoint.x && ownPosition.z == wayPoint.z)
            {
                // if X and Z are equal ownPosition and wayPoint are at the same Point
                return -1f;
            }
            // as the following calculation doesn't result in a multiple of 90, the angle is firstly determined using the X and Z coordinates 
            else if (ownPosition.x == wayPoint.x)
            {
                if (ownPosition.z < wayPoint.z)
                {
                    return 90f;
                }
                else
                {
                    return 270f;
                }
            }
            else if (ownPosition.z == wayPoint.z)
            {
                if (ownPosition.x < wayPoint.x)
                {
                    return 0f;
                }
                else
                {
                    return 180f;
                }
            }

            // calculation of the intersection angle with the x-axis
            float m = (wayPoint.z - ownPosition.z) / (wayPoint.x - ownPosition.x);
            float angle = Mathf.Rad2Deg * Mathf.Atan(m);

            if (angle < 0)
            {
                angle += 180f;
            }

            if (ownPosition.z > wayPoint.z)
            {
                angle += 180f;
            }

            return angle;
        }

        /// <summary>
        /// The method <c>GetVector3FromRotation</c> calculates a new position based on the <c>rotation</c>
        /// around the <c>center</c> point.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="origin"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        protected virtual Vector3 GetVector3FromRotation(Vector3 center, Vector3 origin, float rotation)
        {
            if (rotation == 0)
            {
                return origin;
            }

            // distances from the center
            float distanceX = origin.x - center.x;
            float distanceZ = origin.z - center.z;

            // calculate rotated position
            // Desc: The rotation is applied to the distances and the center is added back to calculate the new position.
            float rotatedX = Mathf.Cos(rotation) * distanceX - Mathf.Sin(rotation) * distanceZ + origin.x;
            float rotatedZ = Mathf.Sin(rotation) * distanceX + Mathf.Cos(rotation) * distanceZ + origin.z;

            return new Vector3(rotatedX, origin.y, rotatedZ);
        }

        /// <summary>
        /// The method <c>GetTicketMatrix</c> determines the ticket-value for the matrix used in the "Crossroad"-material.
        /// </summary>
        private float GetTicketMatrix(List<ETicket> tickets)
        {
            float matrix = 0;
            foreach (ETicket t in tickets)
            {
                switch (t)
                {
                    case ETicket.TAXI:
                        matrix += 2;
                        break;
                    case ETicket.BUS:
                        matrix += 4;
                        break;
                    case ETicket.UNDERGROUND:
                        matrix += 1;
                        break;
                    default:
                        break;
                }
            }

            return matrix;
        }

        /// <summary>
        /// The method <c>GetCrossMatrix</c> uses the angle of the street and the rotation of the crossroad to determine 
        /// which ticket-value is placed on which direction of a <b>4-way-crossroad</b>.
        /// </summary>
        protected virtual Matrix4x4 GetCrossMatrix(Dictionary<float, float> angleTicketMap,
                                                   float                    minusRotation)
        {
            Vector4 column0 = Vector4.zero;
            Vector4 column1 = Vector4.zero;
            Vector4 column2 = Vector4.zero;

            foreach (float keyAngle in angleTicketMap.Keys)
            {
                float correctedAngle = MathHelper.ModFloat((keyAngle % 360) - minusRotation, 360);

                if (correctedAngle > 45 && correctedAngle <= 135)
                {
                    column1.x = angleTicketMap[keyAngle]; // North
                }
                else if (correctedAngle > 135 && correctedAngle <= 225)
                {
                    column0.y = angleTicketMap[keyAngle]; // West
                }
                else if (correctedAngle > 225 && correctedAngle <= 315)
                {
                    column1.z = angleTicketMap[keyAngle]; // South
                }
                else
                {
                    column2.y = angleTicketMap[keyAngle]; // East
                }
            }

            return new Matrix4x4(column0, column1, column2, Vector4.zero);
        }

        /// <summary>
        /// The method <c>GetTCrossMatrix</c> uses the angle of the street and the rotation of the crossroad to determine 
        /// which ticket-value is placed on which direction of a <b>T-junction</b>.
        /// </summary>
        protected virtual Matrix4x4 GetTCrossMatrix(Dictionary<float, float> angleTicketMap,
                                                    float                    minusRotation)
        {
            Vector4 column0 = Vector4.zero;
            Vector4 column1 = Vector4.zero;
            Vector4 column2 = Vector4.zero;

            foreach (float keyAngle in angleTicketMap.Keys)
            {
                float correctedAngle = (minusRotation == 90 || minusRotation == 270)
                                               ? MathHelper.ModFloat((keyAngle % 360) - (minusRotation - 180), 360)
                                               : MathHelper.ModFloat((keyAngle % 360) - minusRotation, 360);

                if (correctedAngle > 0 && correctedAngle <= 135)
                {
                    column1.x = angleTicketMap[keyAngle]; // North
                }
                else if (correctedAngle > 135 && correctedAngle <= 225)
                {
                    if (minusRotation == 90 || minusRotation == 270)
                    {
                        column0.y = angleTicketMap[keyAngle]; // East
                    }
                    else
                    {
                        column0.y = angleTicketMap[keyAngle]; // West
                    }
                }
                else
                {
                    column1.z = angleTicketMap[keyAngle]; // South
                }
            }

            return new Matrix4x4(column0, column1, column2, Vector4.zero);
        }

        /// <summary>
        /// The method <c>GetCornerMatrix</c> uses the angle of the street and the rotation of the crossroad to determine 
        /// which ticket-value is placed on which direction of a <b>corner</b>.
        /// </summary>
        protected virtual Matrix4x4 GetCornerMatrix(Dictionary<float, float> angleTicketMap,
                                                    float                    minusRotation)
        {
            Vector4 column0 = Vector4.zero;
            Vector4 column1 = Vector4.zero;
            Vector4 column2 = Vector4.zero;

            foreach (float keyAngle in angleTicketMap.Keys)
            {
                float correctedAngle = MathHelper.ModFloat((keyAngle % 360) - minusRotation, 360);

                if (correctedAngle > 45 && correctedAngle <= 225)
                {
                    column1.x = angleTicketMap[keyAngle]; // North
                }
                else
                {
                    column2.y = angleTicketMap[keyAngle]; // East
                }
            }

            return new Matrix4x4(column0, column1, column2, Vector4.zero);
        }

        /// <summary>
        /// The method <c>GetLineMatrix</c> uses the angle of the street and the rotation of the crossroad to determine 
        /// which ticket-value is placed on which direction of a <b>straight transition</b>.
        /// </summary>
        protected virtual Matrix4x4 GetLineMatrix(Dictionary<float, float> angleTicketMap,
                                                  float                    minusRotation)
        {
            Vector4 column0 = Vector4.zero;
            Vector4 column1 = Vector4.zero;
            Vector4 column2 = Vector4.zero;

            foreach (float keyAngle in angleTicketMap.Keys)
            {
                float correctedAngle = MathHelper.ModFloat((keyAngle % 360) - minusRotation, 360);

                if (correctedAngle > 0 && correctedAngle <= 180)
                {
                    column1.z = angleTicketMap[keyAngle]; // North
                }
                else
                {
                    column1.x = angleTicketMap[keyAngle]; // South
                }
            }

            return new Matrix4x4(column0, column1, column2, Vector4.zero);
        }
        #endregion

        #if UNITY_EDITOR
        protected List<Vector3> wpTransforms = new List<Vector3>(4);

        private void OnDrawGizmos()
        {
            if (this.wpTransforms == null)
            {
                return;
            }

            if (this.wpTransforms.Count >= 1 && this.wpTransforms[0] != null && !this.wpTransforms[0].Equals(Vector3.zero))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(this.wpTransforms[0], new Vector3(MathHelper.EPSILON, 1, MathHelper.EPSILON));
            }

            if (this.wpTransforms.Count >= 2 && this.wpTransforms[1] != null && !this.wpTransforms[0].Equals(Vector3.zero))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(this.wpTransforms[1], new Vector3(MathHelper.EPSILON, 1, MathHelper.EPSILON));
            }

            if (this.wpTransforms.Count >= 3 && this.wpTransforms[2] != null && !this.wpTransforms[0].Equals(Vector3.zero))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(this.wpTransforms[2], new Vector3(MathHelper.EPSILON, 1, MathHelper.EPSILON));
            }

            if (this.wpTransforms.Count >= 4 && this.wpTransforms[3] != null && !this.wpTransforms[0].Equals(Vector3.zero))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(this.wpTransforms[3], new Vector3(MathHelper.EPSILON, 1, MathHelper.EPSILON));
            }
        }
        #endif
    }
}

