using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _horizontalDirection;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private int _direction = 1;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        GroundMovement();
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        _horizontalDirection = callbackContext.ReadValue<float>();
    }

    private void GroundMovement()
    {
        float xVelocity = _movementSpeed * _horizontalDirection;

        _rb.velocity = new Vector2(xVelocity, _rb.velocity.y);

        if (_direction * xVelocity < 0)
        {
            Flip();
        }

        _animator.SetFloat("Speed", Mathf.Abs(xVelocity));
    }

    private void Flip()
    {
        _direction *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
