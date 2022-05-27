namespace ScotlandYard.Scripts
{
    using ScotlandYard.InputSystem;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CameraController : MonoBehaviour
    {
        #region Members
        private CaCInputControls controls;
        private InputAction movement;
        private Transform ownTransform;
        
        // Camera Object
        [SerializeField] private Camera usedCamera;
        private Transform cameraTransform;

        // Horizontal Translation
        [SerializeField] private float maxSpeed = 5f;
        private float speed;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float damping = 15f;

        // Vertical Translation
        [SerializeField] private float stepSize = 2f;
        [SerializeField] private float zoomDampening = 7.5f;
        [SerializeField] private float minHeight = 5f;
        [SerializeField] private float maxHeight = 50f;
        [SerializeField] private float cameraAngleDeg = 45f;

        // Rotation
        [SerializeField] private float maxRotationSpeed = 1f;

        // Edge Movement
        [SerializeField, Range(0f, 0.1f)] private float edgeTolerance = 0.05f;
        [SerializeField]                  private bool useScreenEdge = false;

        // used to update the position of the camera base
        private Vector3 targetPosition;

        private float zoomHeight;

        // used to track and maintain velocity w/o a rigidbody
        private Vector3 horizontalVelocity;
        private Vector3 lastPosition;

        // tracks where the dragging action started
        private Vector3 startDrag;

        public static CameraController instance;
        #endregion

        #region Methods
        private void Awake()
        {
            instance = this;

            this.controls = new CaCInputControls();
            this.ownTransform = this.transform;
            this.cameraTransform = usedCamera.transform;
        }

        private void OnEnable()
        {
            zoomHeight = cameraTransform.localPosition.y;
            cameraTransform.LookAt(ownTransform);

            lastPosition = ownTransform.position;

            movement = controls.Player.Movement;
            controls.Player.Rotation.performed += RotateCamera;
            controls.Player.Zoom.performed += ZoomCamera;
            controls.Player.Enable();
        }

        private void Update()
        {
            if (Time.timeScale == 0f) { return; }

            GetKeyboardMovement();
            if (useScreenEdge)
            {
                CheckMouseAtScreenEdge();
            }
            DragCamera();

            UpdateVelocity();
            UpdateCameraPosition();
            UpdateBasePosition();
        }

        private void OnDisable()
        {
            controls.Player.Rotation.performed -= RotateCamera;
            controls.Player.Zoom.performed -= ZoomCamera;
            controls.Player.Disable();
        }

        #region Methods provided by https://onewheelstudio.com/blog/2022/1/14/strategy-game-camera-unitys-new-input-system
        private void UpdateVelocity()
        {
            horizontalVelocity = (ownTransform.position - lastPosition) / Time.deltaTime;
            horizontalVelocity.y = 0f;
            lastPosition = ownTransform.position;
        }

        private void GetKeyboardMovement()
        {
            Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight() + movement.ReadValue<Vector2>().y * GetCameraForward();
            inputValue = inputValue.normalized;

            if(inputValue.sqrMagnitude > 0.1f)
            {
                targetPosition += inputValue;
            }
        }

        private Vector3 GetCameraForward()
        {
            Vector3 forward = cameraTransform.forward;
            forward.y = 0f;
            return forward;
        }

        private Vector3 GetCameraRight()
        {
            Vector3 right = cameraTransform.right;
            right.y = 0f;
            return right;
        }

        private void UpdateBasePosition()
        {
            if(targetPosition.sqrMagnitude > 0.1f)
            {
                // create a ramp up or acceleration
                speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
                ownTransform.position += targetPosition * speed * Time.deltaTime;
            }
            else
            {
                // create smooth slow down
                horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
                ownTransform.position += horizontalVelocity * Time.deltaTime;
            }

            // reset for next frame
            targetPosition = Vector3.zero;
        }

        private void RotateCamera(InputAction.CallbackContext inputValue)
        {
            if (!Mouse.current.middleButton.isPressed) { return; }

            float value = inputValue.ReadValue<Vector2>().x;
            ownTransform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + ownTransform.rotation.eulerAngles.y, 0f);
        }

        private void ZoomCamera(InputAction.CallbackContext inputValue)
        {
            float value = -inputValue.ReadValue<Vector2>().y / 100f;

            if(Mathf.Abs(value) > 0.1f)
            {
                zoomHeight = cameraTransform.localPosition.y + value * stepSize;

                if (zoomHeight < minHeight)
                {
                    zoomHeight = minHeight;
                }
                else if (zoomHeight > maxHeight)
                {
                    zoomHeight = maxHeight;
                }
            }
        }

        private void UpdateCameraPosition()
        {
            float distance = -1 * (zoomHeight / Mathf.Tan(cameraAngleDeg * Mathf.Deg2Rad));
            Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, distance);

            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
            cameraTransform.LookAt(ownTransform);
        }

        private void CheckMouseAtScreenEdge()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 moveDirection = Vector3.zero;

            if (mousePosition.x < edgeTolerance * Screen.width)
            {
                moveDirection += -GetCameraRight();
            }
            else if (mousePosition.x > (1 - edgeTolerance) * Screen.width)
            {
                moveDirection += GetCameraRight();
            }

            if (mousePosition.y < edgeTolerance * Screen.height)
            {
                moveDirection += -GetCameraForward();
            }
            else if (mousePosition.y > (1 - edgeTolerance) * Screen.height)
            {
                moveDirection += GetCameraForward();
            }

            targetPosition += moveDirection;
        }

        private void DragCamera()
        {
            if (!Mouse.current.rightButton.isPressed) { return; }

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = usedCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if(plane.Raycast(ray, out float distance))
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    startDrag = ray.GetPoint(distance);
                }
                else
                {
                    targetPosition += startDrag - ray.GetPoint(distance);
                }
            }
        }
        #endregion

        public void SetPosition(Vector3 position)
        {
            ownTransform.position = position;
            lastPosition = position;
        }
        #endregion
    }
}