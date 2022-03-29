using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damage;
    public bool destoyOnDamage;

    void DoDamage(Damagable damagable)
    {
        damagable.TakeDamage(damage);
        if (destoyOnDamage)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Damagable damagable = collision.gameObject.GetComponent<Damagable>();
        if (damagable != null)
        {
            DoDamage(damagable);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damagable damagable = collision.gameObject.GetComponent<Damagable>();
        if (damagable != null)
        {
            DoDamage(damagable);
        }
    }
}
