namespace ScotlandYard.Scripts
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;
        [SerializeField] protected Transform cameraTransform;
        [SerializeField] protected Camera ownCamera;

        public float normalSpeed;
        public float fastSpeed;
        public float movementSpeed;
        public float movementTime;
        public float rotationAmount;
        public Vector3 zoomAmount;
        [SerializeField] protected float maxZoomIn;
        [SerializeField] protected float maxZoomOut;

        protected Vector3 newPosition;
        protected Quaternion newRotation;
        protected Vector3 newZoom;

        protected Vector3 dragStartPosition;
        protected Vector3 dragCurrentPosition;
        protected Vector3 rotateStartPosition;
        protected Vector3 rotateCurrentPosition;

        protected Transform ownTransform;

        protected void Awake()
        {
            ownTransform = this.transform;
        }

        void Start()
        {
            instance = this;

            newPosition = ownTransform.position;
            newRotation = ownTransform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            HandleMouseInput();
            HandleMovementInput();

            ownTransform.position = Vector3.Lerp(ownTransform.position, newPosition, Time.deltaTime * movementTime);
            ownTransform.rotation = Quaternion.Lerp(ownTransform.rotation, newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }

        protected void HandleMouseInput()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                Vector3 zoomLevel = newZoom + Input.mouseScrollDelta.y * zoomAmount;

                if((zoomLevel.y < newZoom.y && newZoom.y > maxZoomIn) || (zoomLevel.y > newZoom.y && newZoom.y < maxZoomOut))
                {
                    newZoom = zoomLevel;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = ownCamera.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            if (Input.GetMouseButton(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = ownCamera.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    newPosition = ownTransform.position + dragStartPosition - dragCurrentPosition;
                }
            }

            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;

                Vector3 difference = rotateStartPosition - rotateCurrentPosition;
                rotateStartPosition = rotateCurrentPosition; // reset StartPosition for the next frame

                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }

        protected void HandleMovementInput()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (ownTransform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (ownTransform.right * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (ownTransform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (ownTransform.right * movementSpeed);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            if (Input.GetKey(KeyCode.R) && newZoom.y > maxZoomIn)
            {
                newZoom += zoomAmount;
            }
            if (Input.GetKey(KeyCode.F) && newZoom.y < maxZoomOut)
            {
                newZoom -= zoomAmount;
            }
        }

        public void SetPosition(Vector3 position)
        {
            ownTransform.position = position;
            newPosition = position;
        }
    }
}