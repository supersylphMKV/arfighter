using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DamageController damageController = other.transform.GetComponentInParent<DamageController>();
        if (damageController)
        {
            damageController.GetHit(transform.position);
            Destroy(gameObject);
        }
    }
}
