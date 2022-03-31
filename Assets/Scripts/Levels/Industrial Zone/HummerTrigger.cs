using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HummerTrigger : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(StartAnimation());
    }


    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        _animator.SetTrigger("Start");
    }
}
