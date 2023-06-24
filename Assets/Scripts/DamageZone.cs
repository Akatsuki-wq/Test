using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damage = -10f;
    private float time;
    private bool resume = false;
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (!resume)
        {
            resume = true;
            time = Time.fixedTime;
        }
        if (controller != null)
        {
            if (Time.fixedTime - time > 1f)
            {
                controller.ChangeHealth(damage);
                resume = false;
            }
        }
    }
}
