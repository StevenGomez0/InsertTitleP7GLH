using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public int health;
    private PlayerMovement plrScript;
    private BoxCollider self;
    private MeshRenderer mesh;
    private AudioSource audioSource;

    public AudioClip healSFX;

    private void OnTriggerEnter(Collider other)
    {
        plrScript = other.gameObject.GetComponent<PlayerMovement>();
        if (plrScript.Health < plrScript.startingHealth)
        {
            audioSource = GetComponent<AudioSource>();
            mesh = GetComponent<MeshRenderer>();
            self = GetComponent<BoxCollider>();
            plrScript.Heal(health);
            mesh.enabled = false;
            self.enabled = false;
            audioSource.PlayOneShot(healSFX);
            Destroy(gameObject, healSFX.length);
        }
    }
}
