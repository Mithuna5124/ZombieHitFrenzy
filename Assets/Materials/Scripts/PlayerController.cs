using UnityEngine;

[RequireComponent(typeof(PrometeoCarController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float deadZone = 10f;

    private PrometeoCarController _car;
    private Camera _cam;
    private Vector2 _pressOrigin;
    private Vector3 _inputDirection;
    private bool _hasInput;

    private void Awake()
    {
        _car = GetComponent<PrometeoCarController>();
        _cam = Camera.main;
        _car.useTouchControls = false;
        CancelInvoke("Decelerate");
    }

    private void Update()
    {
        ReadInput();

#if !UNITY_EDITOR
        Drive();
        Steer();
#endif
    }

    private void LateUpdate()
    {
#if UNITY_EDITOR
        ReadMouseInput();
        Drive();
        Steer();
#endif
    }

    private void ReadInput()
    {
        _hasInput = false;
        _inputDirection = Vector3.zero;

        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            _pressOrigin = touch.position;
            return;
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            _pressOrigin = Vector2.zero;
            return;
        }

        Vector2 drag = touch.position - _pressOrigin;
        if (drag.magnitude < deadZone) return;

        _hasInput = true;
        _inputDirection = ToWorldDirection(drag);
    }

    private void Drive()
    {
        if (_hasInput)
        {
            CancelInvoke("Decelerate");
            _car.deceleratingCar = false;
            _car.GoForward();
        }
        else
        {
            _car.ThrottleOff();
            if (!_car.deceleratingCar)
            {
                InvokeRepeating("Decelerate", 0f, 0.1f);
                _car.deceleratingCar = true;
            }
        }
    }

    private void Steer()
    {
        if (!_hasInput) { _car.ResetSteeringAngle(); return; }

        float angle = Vector3.SignedAngle(transform.forward, _inputDirection, Vector3.up);

        if (angle > 5f) _car.TurnRight();
        else if (angle < -5f) _car.TurnLeft();
        else _car.ResetSteeringAngle();
    }

    private Vector3 ToWorldDirection(Vector2 drag)
    {
        var fwd = _cam.transform.forward; fwd.y = 0f;
        var right = _cam.transform.right; right.y = 0f;
        return (fwd.normalized * drag.y + right.normalized * drag.x).normalized;
    }

    private void Decelerate() => _car.DecelerateCar();

#if UNITY_EDITOR
    private Vector2 _mouseOrigin;
    private bool _mouseDragging;

    private void ReadMouseInput()
    {
        _hasInput = false;
        _inputDirection = Vector3.zero;

        if (Input.GetMouseButtonDown(0))
        {
            _mouseOrigin = Input.mousePosition;
            _mouseDragging = true;
        }

        if (Input.GetMouseButton(0) && _mouseDragging)
        {
            Vector2 drag = (Vector2)Input.mousePosition - _mouseOrigin;
            if (drag.magnitude < deadZone) return;
            _hasInput = true;
            _inputDirection = ToWorldDirection(drag);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseDragging = false;
            _hasInput = false;
            _mouseOrigin = Vector2.zero;
        }
    }
#endif
}