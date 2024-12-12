using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class Gun1 : MonoBehaviour
{
    public UnityEvent OnGunShoot;
    public ParticleSystem muzzleFlash;

    public float shotCD;
    private float currentCD;

    void Start()
    {
        currentCD = shotCD; 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentCD <= 0f)
            {
                muzzleFlash.Play();
                //add animation maybe if i have time
                OnGunShoot?.Invoke();
                currentCD = shotCD;
            }
        }
        currentCD -= Time.deltaTime;
    }

}
