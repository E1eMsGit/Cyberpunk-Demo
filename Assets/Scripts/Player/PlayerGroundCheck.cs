using System;
using System.Collections;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isOnGround;
    [SerializeField] private float _groundDistance = 0.15f;

    private BoxCollider2D _boxCollider2D;
    private Animator _animator;

    public event Action<bool> GroundChecking;

    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        StartCoroutine(IsGrounded());
    }

    private IEnumerator IsGrounded()
    {
        while (true)
        {
            yield return null;

            RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0f, Vector2.down, _groundDistance, _groundLayer);

            Color rayColor = raycastHit.collider != null ? Color.green : Color.red;

            Debug.DrawRay(_boxCollider2D.bounds.center + new Vector3(_boxCollider2D.bounds.extents.x, 0), Vector2.down * (_boxCollider2D.bounds.extents.y + _groundDistance), rayColor);
            Debug.DrawRay(_boxCollider2D.bounds.center - new Vector3(_boxCollider2D.bounds.extents.x, 0), Vector2.down * (_boxCollider2D.bounds.extents.y + _groundDistance), rayColor);
            Debug.DrawRay(_boxCollider2D.bounds.center - new Vector3(_boxCollider2D.bounds.extents.x, _boxCollider2D.bounds.extents.y + _groundDistance), Vector2.right * _boxCollider2D.bounds.extents.x * 2, rayColor);

            _isOnGround = raycastHit.collider != null;
            GroundChecking?.Invoke(_isOnGround);
            _animator.SetBool("OnGround", _isOnGround);
        }
    }
}
