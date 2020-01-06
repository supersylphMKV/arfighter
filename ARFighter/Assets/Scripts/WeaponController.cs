using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform[] weapons;
    public GameObject projectile;
    public Transform parent;
    public float firingRate = 0.25f;
    public bool firing;

    private float fireTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (firing)
        {
            if(fireTimer > 0f)
            {
                fireTimer -= Time.deltaTime;
            }
            else
            {
                Fire();
                fireTimer = firingRate;
            }
        } else
        {
            fireTimer = 0;
        }
    }

    // Update is called once per frame
    public void Fire()
    {
        foreach(Transform t in weapons)
        {
            GameObject prj = Instantiate(projectile, t);
            prj.transform.localPosition = Vector3.zero;
            prj.transform.localRotation = Quaternion.identity;
            prj.transform.SetParent(parent);
            Destroy(prj, 5);
        }
    }
}
