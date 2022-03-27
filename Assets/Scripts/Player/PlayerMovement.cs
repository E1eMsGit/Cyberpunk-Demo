using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _movementSpeed = 5;
    private float _horizontal;
    private int _direction = 1;

    [Header("Ground")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isOnGround;
    [SerializeField] private float _groundDistance = 0.15f;

    [Header("Jump")]    
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _canDoubleJump;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _coyoteDuration = 3f;
    private bool _jumpStarted;
    private float _coyoteTime;

    [Header("Ladder")]
    [SerializeField] private float _climbSpeed = 3;
    [SerializeField] private LayerMask _ladderMask;
    [SerializeField] private bool _climbing;
    [SerializeField] private float _checkRadius = 0.3f;
    private float _vertical;
    private Transform _ladder;
    private bool _canMove = true;

    private Rigidbody2D _rb;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        IsGrounded();
        GroundMovement();
        AirMovement();
        ClimbLadder();
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        _horizontal = callbackContext.ReadValue<float>();
    }
    public void Climb(InputAction.CallbackContext callbackContext)
    {
        _vertical = callbackContext.ReadValue<float>();
    }
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            _jumpStarted = true;
        }
    }

    private void IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0f, Vector2.down, _groundDistance, _groundLayer);

        Color rayColor = raycastHit.collider != null ? Color.green : Color.red;

        Debug.DrawRay(_boxCollider2D.bounds.center + new Vector3(_boxCollider2D.bounds.extents.x, 0), Vector2.down * (_boxCollider2D.bounds.extents.y + _groundDistance), rayColor);
        Debug.DrawRay(_boxCollider2D.bounds.center - new Vector3(_boxCollider2D.bounds.extents.x, 0), Vector2.down * (_boxCollider2D.bounds.extents.y + _groundDistance), rayColor);
        Debug.DrawRay(_boxCollider2D.bounds.center - new Vector3(_boxCollider2D.bounds.extents.x, _boxCollider2D.bounds.extents.y + _groundDistance), Vector2.right * _boxCollider2D.bounds.extents.x * 2, rayColor);

        _isOnGround = raycastHit.collider != null;
        _animator.SetBool("OnGround", _isOnGround);

    }

    private void GroundMovement()
    {
        if (!_canMove)
        {
            return;
        }

        float xVelocity = _movementSpeed * _horizontal;

        _rb.velocity = new Vector2(xVelocity, _rb.velocity.y);

        if (_direction * xVelocity < 0)
        {
            Flip();
        }

        if (_isOnGround)
        {
            _coyoteTime = Time.time + _coyoteDuration;
            _isJumping = false;
            _canDoubleJump = false;
        }

        _animator.SetFloat("Speed", Mathf.Abs(xVelocity));
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

    private void Flip()
    {
        _direction *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool TouchingLadder()
    {
        return _boxCollider2D.IsTouchingLayers(_ladderMask);
    }

    private void ClimbLadder()
    {
        bool up = Physics2D.OverlapCircle(transform.position, _checkRadius, _ladderMask);
        bool down = Physics2D.OverlapCircle(transform.position + new Vector3(0, -1), _checkRadius, _ladderMask);

        if (_vertical != 0 && TouchingLadder())
        {
            _climbing = true;
            _rb.isKinematic = true;
            _canMove = false;

            float xPos = _ladder.position.x;
            transform.position = new Vector2(xPos, transform.position.y);
            _animator.SetBool("IsClimbing", true);
        }

        if (_climbing)
        {
            if (!up && _vertical >= 0)
            {
                FinishClimb();
                return;
            }

            if (!down && _vertical <= 0)
            {
                FinishClimb();
                return;
            }

            float y = _vertical * _climbSpeed;
            _rb.velocity = new Vector2(0, y);
            _animator.enabled = _vertical == 0 ? false : true;
        }
    }

    private void FinishClimb()
    {
        _climbing = false;
        _rb.isKinematic = false;
        _canMove = true;
        _animator.SetBool("IsClimbing", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            _ladder = collision.transform;
        }
    }
}
