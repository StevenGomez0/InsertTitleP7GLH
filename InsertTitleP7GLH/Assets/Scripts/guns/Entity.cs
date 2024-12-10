using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float StartingHealth;
    public float health;

    void Start()
    {
        health = StartingHealth;
    }
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            Debug.Log(health);

            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
