using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCreator : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _box;
    [SerializeField] private int _createBoxTime = 5;

    void Start()
    {
        StartCoroutine(CreateBox());
    }

    IEnumerator CreateBox()
    {
        while (true)
        {
            Instantiate(_box, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_createBoxTime);
        }
    }
}
