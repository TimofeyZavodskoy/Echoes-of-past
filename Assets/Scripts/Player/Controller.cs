using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public float currentSpeed;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float acceleration = 10f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    private Vector2 _move;
    private Vector3 _verticalSpeed;
    private bool _jumpRequested = false;

    public void Start()
    {
        currentSpeed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue val)
    {
        Debug.Log("Move");
        _move = val.Get<Vector2>();
    }

    public void OnSprint(InputValue val)
    {
        Debug.Log("Run " + val.Get<float>());

        if (val.Get<float>() > 0.5f)
        {
            //currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, acceleration * Time.deltaTime);
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    public void OnJump(InputValue val)
    {
        Debug.Log("Jump " + val.Get<float>());
        if (val.Get<float>() > 0.1f)
        {
            _jumpRequested = true;
            //_verticalSpeed.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
        else
        {
            _jumpRequested = false;
        }
    }

    private void Update()
    {
        if (_characterController.isGrounded)
        {
            _verticalSpeed.y = -2f;

            if (_jumpRequested)
            {
                _verticalSpeed.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                _jumpRequested = false;
            }
        }
        else
        {
            _verticalSpeed.y += gravity * Time.deltaTime;
        }

        Vector3 horizontalMove = ((GetForward() * _move.y + GetRight() * _move.x) * Time.deltaTime);

        Vector3 verticalMove = Vector3.up * _verticalSpeed.y;
        
        _characterController.Move((horizontalMove +  verticalMove) * Time.deltaTime);
        //_characterController.Move((GetForward() * _move.y + GetRight() * _move.x + Vector3.up * _verticalSpeed.y) * Time.deltaTime * currentSpeed);

        if (_jumpRequested && _verticalSpeed.y < 0)
        {
            _jumpRequested = false;
        }
    }

    private Vector3 GetForward()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;

        return forward.normalized;
    }

    private Vector3 GetRight()
    {
        Vector3 right = Camera.main.transform.right;
        right.y = 0f;

        return right.normalized;
    }
}
