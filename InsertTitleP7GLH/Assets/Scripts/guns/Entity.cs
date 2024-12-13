using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private AudioSource audiosr;
    public AudioClip deathclip;
    [SerializeField] private float StartingHealth;
    public float health;
    private MeshRenderer mesh;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        audiosr = GetComponent<AudioSource>();
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
                Destroy(mesh);
                audiosr.PlayOneShot(deathclip);
                Invoke(nameof(DestroyObject), deathclip.length);
            }
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
