using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public float health = 100f;
    public Vector3 hitPos;
    // Start is called before the first frame update
    public void GetHit(Vector3 pos)
    {
        hitPos = pos;
        health -= 5;
    }
}
