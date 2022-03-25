using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private bool _jumpStarted;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _canDoubleJump;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _coyoteDuration = 3f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerGroundCheck _playerGroundCheck;
    private float _coyoteTime;
    private bool _isOnGround;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerGroundCheck = GetComponent<PlayerGroundCheck>();
        _playerGroundCheck.GroundChecking += GroundCheck;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        AirMovement();
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            _jumpStarted = true;
        }
    }

    private void AirMovement()
    {
        if (_jumpStarted && (_isOnGround || _canDoubleJump || _coyoteTime > Time.time))
        {
            if (_canDoubleJump)
            {
                _animator.SetTrigger("DoubleJump");
                _canDoubleJump = false;
            }
            else
            {
                _canDoubleJump = true;
            }

            _isJumping = true;
            _rb.velocity = Vector2.zero;
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _coyoteTime = Time.time;
        }

        _jumpStarted = false;
    }

    private void GroundCheck(bool isOnGround)
    {
        _isOnGround = isOnGround;

        if (_isOnGround)
        {
            _coyoteTime = Time.time + _coyoteDuration;
            _isJumping = false;
            _canDoubleJump = false;
        }
    }
}
