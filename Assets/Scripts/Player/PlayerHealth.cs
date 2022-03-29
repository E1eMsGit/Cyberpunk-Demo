using UnityEngine;

public class PlayerHealth : Damagable
{
    private int defaultLayer;

    void Start()
    {
        base.Start();
        defaultLayer = gameObject.layer;
    }

    public override void Death()
    {
        Debug.Log("Death");
    }

    public void SetInvencible(bool state)
    {
        if (state)
        {
            gameObject.layer = LayerMask.NameToLayer("Invencible");
        }
        else
        {
            gameObject.layer = defaultLayer;
        }
    }
}
