using System;
using UnityEngine;

[Serializable]
public class PlayerMoveController : MonoBehaviour
{
    private Vector3 cameraOffset = Vector3.zero;
    public float cameraPreview = 2f;
    public float cameraSmoothing = 0.01f;
    private Vector3 cameraVelocity = Vector3.zero;
    public Transform character;
    public float cursorFacingCamera;
    private Transform cursorObject;
    public float cursorPlaneHeight;
    public GameObject cursorPrefab;
    private Vector3 cursorScreenPosition;
    public float cursorSmallerWhenClose = 1;
    public float cursorSmallerWithDistance;
    private Vector3 initOffsetToPlayer;
    private Joystick joystickLeft;
    public GameObject joystickPrefab;
    private Joystick joystickRight;
    private GameObject joystickRightGO;
    private Camera mainCamera;
    private Transform mainCameraTransform;
    public MovementMotor motor;
    private Plane playerMovementPlane;
    private Vector3 screenMovementForward;
    private Vector3 screenMovementRight;
    private Quaternion screenMovementSpace;

    public override void Awake()
    {
        this.motor.movementDirection = (Vector3) Vector2.zero;
        this.motor.facingDirection = (Vector3) Vector2.zero;
        this.mainCamera = Camera.main;
        this.mainCameraTransform = this.mainCamera.transform;
        if (this.character == null)
        {
            this.character = this.transform;
        }
        this.initOffsetToPlayer = this.mainCameraTransform.position - this.character.position;
        if (this.joystickPrefab != null)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.joystickPrefab) as GameObject;
            obj2.name = "Joystick Left";
            this.joystickLeft = obj2.GetComponent<Joystick>();
            this.joystickRightGO = UnityEngine.Object.Instantiate<GameObject>(this.joystickPrefab) as GameObject;
            this.joystickRightGO.name = "Joystick Right";
            this.joystickRight = this.joystickRightGO.GetComponent<Joystick>();
        }
        this.cameraOffset = this.mainCameraTransform.position - this.character.position;
        this.cursorScreenPosition = new Vector3(0.5f * Screen.width, 0.5f * Screen.height, (float) 0);
        this.playerMovementPlane = new Plane(this.character.up, this.character.position + ((Vector3) (this.character.up * this.cursorPlaneHeight)));
    }

    public override void HandleCursorAlignment(Vector3 cursorWorldPosition)
    {
        if (this.cursorObject != null)
        {
            this.cursorObject.position = cursorWorldPosition;
            if (Input.mousePosition.x >= 0)
            {
            }
            if (Input.mousePosition.x <= Screen.width)
            {
            }
            if (Input.mousePosition.y >= 0)
            {
            }
            Cursor.visible = Input.mousePosition.y > Screen.height;
            Quaternion rotation = this.cursorObject.rotation;
            if (this.motor.facingDirection != Vector3.zero)
            {
                rotation = Quaternion.LookRotation(this.motor.facingDirection);
            }
            Vector3 forward = Input.mousePosition - this.mainCamera.WorldToScreenPoint(this.transform.position + ((Vector3) (this.character.up * this.cursorPlaneHeight)));
            forward.z = 0;
            Quaternion to = this.mainCameraTransform.rotation * Quaternion.LookRotation(forward, -Vector3.forward);
            this.cursorObject.rotation = Quaternion.Slerp(rotation, to, this.cursorFacingCamera);
            float from = 0.1f * Vector3.Dot(cursorWorldPosition - this.mainCameraTransform.position, this.mainCameraTransform.forward);
            float num2 = Mathf.Lerp(0.7f, 1f, Mathf.InverseLerp(0.5f, 4f, this.motor.facingDirection.magnitude));
            this.cursorObject.localScale = (Vector3) ((Vector3.one * Mathf.Lerp(from, (float) 1, this.cursorSmallerWithDistance)) * num2);
            if (Input.GetKey(KeyCode.O))
            {
                this.cursorFacingCamera += Time.deltaTime * 0.5f;
            }
            if (Input.GetKey(KeyCode.P))
            {
                this.cursorFacingCamera -= Time.deltaTime * 0.5f;
            }
            this.cursorFacingCamera = Mathf.Clamp01(this.cursorFacingCamera);
            if (Input.GetKey(KeyCode.K))
            {
                this.cursorSmallerWithDistance += Time.deltaTime * 0.5f;
            }
            if (Input.GetKey(KeyCode.L))
            {
                this.cursorSmallerWithDistance -= Time.deltaTime * 0.5f;
            }
            this.cursorSmallerWithDistance = Mathf.Clamp01(this.cursorSmallerWithDistance);
        }
    }

    public override void Main()
    {
    }

    public override void OnDisable()
    {
        if (this.joystickLeft != null)
        {
            this.joystickLeft.enabled = false;
        }
        if (this.joystickRight != null)
        {
            this.joystickRight.enabled = false;
        }
    }

    public override void OnEnable()
    {
        if (this.joystickLeft != null)
        {
            this.joystickLeft.enabled = true;
        }
        if (this.joystickRight != null)
        {
            this.joystickRight.enabled = true;
        }
    }

    public static Vector3 PlaneRayIntersection(Plane plane, Ray ray)
    {
        float enter = new float();
        plane.Raycast(ray, out enter);
        return ray.GetPoint(enter);
    }

    public static Vector3 ScreenPointToWorldPointOnPlane(Vector3 screenPoint, Plane plane, Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(screenPoint);
        return PlaneRayIntersection(plane, ray);
    }

    public override void Start()
    {
        float num;
        Rect rect;
        GUITexture component = this.joystickRightGO.GetComponent<GUITexture>();
        float single1 = num = (Screen.width - component.pixelInset.x) - component.pixelInset.width;
        Rect rect1 = rect = component.pixelInset;
        rect.x = num;
        Rect rect5 = component.pixelInset = rect;
        this.screenMovementSpace = Quaternion.Euler((float) 0, this.mainCameraTransform.eulerAngles.y, (float) 0);
        this.screenMovementForward = (Vector3) (this.screenMovementSpace * Vector3.forward);
        this.screenMovementRight = (Vector3) (this.screenMovementSpace * Vector3.right);
    }

    public override void Update()
    {
        this.motor.movementDirection = (Vector3) ((this.joystickLeft.position.x * this.screenMovementRight) + (this.joystickLeft.position.y * this.screenMovementForward));
        if (this.motor.movementDirection.sqrMagnitude > 1)
        {
            this.motor.movementDirection.Normalize();
        }
        this.playerMovementPlane.normal = this.character.up;
        this.playerMovementPlane.distance = -this.character.position.y + this.cursorPlaneHeight;
        Vector3 zero = Vector3.zero;
        this.motor.facingDirection = (Vector3) ((this.joystickRight.position.x * this.screenMovementRight) + (this.joystickRight.position.y * this.screenMovementForward));
        zero = this.motor.facingDirection;
        Vector3 target = (this.character.position + this.initOffsetToPlayer) + ((Vector3) (zero * this.cameraPreview));
        this.mainCameraTransform.position = Vector3.SmoothDamp(this.mainCameraTransform.position, target, ref this.cameraVelocity, this.cameraSmoothing);
        this.cameraOffset = this.mainCameraTransform.position - this.character.position;
    }
}

