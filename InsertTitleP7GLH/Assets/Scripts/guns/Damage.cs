using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float DamageValue;
    public float BulletRange;
    private Transform PlayerCamera;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main.transform;
    }

    public void Shoot()
    {
        Ray gunRay = new Ray(PlayerCamera.position, PlayerCamera.forward);
        if (Physics.Raycast(gunRay, out RaycastHit hitinfo))
        {
            if(hitinfo.collider.gameObject.TryGetComponent(out Entity enemy))
            {
                enemy.Health -= DamageValue;
            }
        }
    }
}
