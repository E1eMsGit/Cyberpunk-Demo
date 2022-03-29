using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Animator _animator;
   
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D newRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (newRb != null)
        {
            _animator.SetTrigger("Start");
        }
    }
}
