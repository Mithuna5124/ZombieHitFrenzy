using UnityEngine;

public class SwipeInputHandler : MonoBehaviour
{
    [SerializeField] private float deadZone = 10f;

    public Vector3 joystickDirection { get; private set; }
    public bool IsTouching { get; private set; }

    private Vector2 touchStartPos;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        joystickDirection = Vector3.zero;
        IsTouching = false;

        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStartPos = touch.position;
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                Vector2 delta = touch.position - touchStartPos;
                if (delta.magnitude < deadZone) break;

                IsTouching = true;
                joystickDirection = GetWorldDirection(delta);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                touchStartPos = Vector2.zero;
                break;
        }
    }

    private Vector3 GetWorldDirection(Vector2 screenDelta)
    {
        // Get camera's forward and right on the ground plane
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        // Flatten to ground — ignore height
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Drag up on screen = camera forward in world
        // Drag right on screen = camera right in world
        return (camForward * screenDelta.y + camRight * screenDelta.x).normalized;
    }

#if UNITY_EDITOR
    private Vector2 mouseDragStart;
    private bool mouseDragging;

    private void LateUpdate()
    {
        joystickDirection = Vector3.zero;

        if (Input.GetMouseButtonDown(0))
        {
            mouseDragStart = Input.mousePosition;
            mouseDragging = true;
        }

        if (Input.GetMouseButton(0) && mouseDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - mouseDragStart;
            if (delta.magnitude < deadZone) return;

            IsTouching = true;
            joystickDirection = GetWorldDirection(delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseDragging = false;
            IsTouching = false;
            mouseDragStart = Vector2.zero;
        }
    }
#endif
}