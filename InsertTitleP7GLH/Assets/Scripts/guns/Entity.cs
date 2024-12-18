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
    private SphereCollider colliderr;
    public bool isDead;
    [Range(0, 1)] public float deathSFXVolume;

    private GameObject gameManager;
    private KillCounter waveSys;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        waveSys = gameManager.GetComponent<KillCounter>();

        mesh = GetComponent<MeshRenderer>();
        audiosr = GetComponent<AudioSource>();
        health = StartingHealth;
        colliderr = GetComponent<SphereCollider>();
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
            //Debug.Log(health);

            if(health <= 0)
            {
                waveSys.OnEnemyKilled();
                isDead = true;
                mesh.enabled = false;
                Destroy(colliderr);
                audiosr.PlayOneShot(deathclip, deathSFXVolume);
                Invoke(nameof(DestroyObject), deathclip.length);
            }
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
