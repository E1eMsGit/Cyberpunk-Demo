using System.Collections;
using UnityEngine;

public class Hummer : MonoBehaviour
{
    [SerializeField] int _intervalMinVal;
    [SerializeField] int _intervalMaxVal;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(_intervalMinVal, _intervalMaxVal));
        _animator.enabled = true;
    }
}
