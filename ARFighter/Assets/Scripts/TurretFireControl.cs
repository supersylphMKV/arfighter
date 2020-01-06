using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireControl : MonoBehaviour
{
    private TurretController trtCtrl;

    private void Awake()
    {
        trtCtrl = GetComponentInParent<TurretController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("fighter"))
        {
            trtCtrl.OnEnemyInRange(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("fighter"))
        {
            trtCtrl.OnEnemyOutOfRange(other);
        }
    }
}
