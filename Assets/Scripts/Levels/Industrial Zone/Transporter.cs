using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    [SerializeField] private int _direction = 1;
    [SerializeField] private float _force = 10;

    private Animator _animator;
    private List<Rigidbody2D> _rbs = new List<Rigidbody2D>();

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_direction == 1)
        {
            _animator.Play("Transporter Right");
        }
        else if (_direction == -1)
        {
            _animator.Play("Transporter Left");
        }

        for (int i = 0; i < _rbs.Count; i++)
        {
            _rbs[i].AddForce(Vector2.right * _direction * _force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D newRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (newRb != null)
        {
            _rbs.Add(newRb);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D newRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (newRb != null)
        {
            if (_rbs.Contains(newRb))
            {
                _rbs.Remove(newRb);
            }
        }
    }
}
