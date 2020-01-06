using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private List<Collider> targets = new List<Collider>();
    private WeaponController weaponControl;

    public Collider currentTarget;
    public Transform targetFinder;
    public Transform root;
    public bool isFiring = false;
    public bool isInRange = false;
    public float agility = 1;

    // Start is called before the first frame update
    void Start()
    {
        weaponControl = GetComponent<WeaponController>();
        targetFinder.SetParent(null);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(targets.Count > 0)
        {
            CalculatingRange();
        } else
        {
            currentTarget = null;
        }

        if (currentTarget)
        {
            targetFinder.position = Vector3.MoveTowards(targetFinder.position, currentTarget.transform.position, agility * Time.fixedDeltaTime);
            root.LookAt(targetFinder);
        }

        if (currentTarget && isInRange && !isFiring)
        {
            isFiring = true;
        }

        weaponControl.firing = isFiring;
    }

    void CalculatingRange()
    {
        float nearestDistance = float.MaxValue;
        for(int c = 0; c < targets.Count; c++)
        {
            if (targets[c])
            {
                float cDist = Vector3.Distance(transform.position, targets[c].transform.position);
                if (cDist < nearestDistance)
                {
                    nearestDistance = cDist;
                    currentTarget = targets[c];
                }
            }
            else
            {
                targets.RemoveAt(c);
            }
        }
    }

    public void OnEnemySighted(Collider enemy)
    {
        if (!targets.Contains(enemy))
        {
            targets.Add(enemy);
            //Debug.Log("tracking enemy");
        }
    }

    public void OnEnemyOutOfSight(Collider enemy)
    {
        targets.Remove(enemy);
    }

    public void OnEnemyInRange(Collider enemy)
    {
        isInRange = true;
        //Debug.Log("enemy in firing range");
    }

    public void OnEnemyOutOfRange(Collider enemy)
    {
        if(enemy == currentTarget)
        {
            isInRange = false;
            isFiring = false;
        }
    }
}
