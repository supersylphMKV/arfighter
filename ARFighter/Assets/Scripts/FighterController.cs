using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public float hoverHeight = 0.1f;
    public float hoverForce = 60.0f;
    [SerializeField]
    [Range(-1f, 1f)]
    public float stickH = 0;
    [SerializeField]
    [Range(-1f, 1f)]
    public float stickV = 0;
    [SerializeField]
    [Range(-1f, 1f)]
    public float tilt = 0;
    [SerializeField]
    [Range(-1f, 1f)]
    public float yaw = 0;
    public bool isActive = true;
    public HealthIndicator healthIndicator;

    private Rigidbody dynamics;
    private DamageController dmgCtrl;
    private bool isSelfDestroying = false;
    private WeaponController weapon;
    
    void Start()
    {
        dynamics = GetComponent<Rigidbody>();
        dmgCtrl = GetComponent<DamageController>();
        weapon = GetComponent<WeaponController>();
    }

    void FixedUpdate()
    {

        if (isActive)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            transform.localEulerAngles = new Vector3(stickV, transform.localEulerAngles.y, -stickH);

            dynamics.AddRelativeTorque(Vector3.up * hoverForce * 0.000005f * yaw);

            if (Physics.Raycast(ray, out hit, hoverHeight) && hit.collider.CompareTag("Floor"))
            {
                
                float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
                dynamics.AddRelativeForce(appliedHoverForce, ForceMode.Acceleration);
            }

            if (healthIndicator)
            {
                healthIndicator.SetHealth(dmgCtrl.health);
            }

            if (dmgCtrl.health < 0 && !isSelfDestroying)
            {
                isActive = false;
                Destroy(gameObject, 5f);
                dynamics.isKinematic = false;
                dynamics.useGravity = true;
                dynamics.AddExplosionForce(100f, dmgCtrl.hitPos, 0.5f);
                isSelfDestroying = true;
            }
        }

    }

    public void Firing()
    {
        if (weapon)
        {
            weapon.Fire();
        }
    }
}
