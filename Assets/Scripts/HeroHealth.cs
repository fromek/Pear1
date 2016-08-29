using UnityEngine;
using System.Collections;

public class HeroHealth : MonoBehaviour {

    public float health = 100f;
    public float repeatDamagePeriod = 2f;       // How frequently the player can be damaged.
    public float hurtForce = 10f;               // The force with which the player is pushed when hurt.
    public float damageAmount = 10f;// The amount of damage to take when enemies touch the player
    public AudioClip hurtClip;
    public AudioClip deathClip;

    private float lastHitTime;                  // The time at which the player was last hit.
    private HeroControllerScript playerControl;        // Reference to the PlayerControl script.
    private AudioSource audioSource;

    void Awake()
    {
        // Setting up references.
        playerControl = GetComponent<HeroControllerScript>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // If the colliding gameobject is an Enemy...
        if (col.gameObject.tag == "Enemy" && !col.gameObject.GetComponent<Enemy>().IsFriend)
        {
            // ... and if the time exceeds the time of the last hit plus the time between hits...
            if (Time.time > lastHitTime + repeatDamagePeriod)
            {
                // ... and if the player still has health...
                if (health > 0f)
                {
                    // ... take damage and reset the lastHitTime.
                    TakeDamage(col.transform);
                    lastHitTime = Time.time;
                }
                // If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
                else
                {
                    // Find all of the colliders on the gameobject and set them all to be triggers.
                    Collider2D[] cols = GetComponents<Collider2D>();
                    foreach (Collider2D c in cols)
                    {
                        c.isTrigger = true;
                    }

                    
                    // ... disable user Player Control script
                    GetComponent<HeroControllerScript>().enabled = false;

                    // ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
                    GetComponentInChildren<Gun>().enabled = false;
                    audioSource.clip = deathClip;
                    audioSource.Play();
                }
            }
        }
    }

    void TakeDamage(Transform enemy)
    {
        // Make sure the player can't jump.
        playerControl.jump = false;

        // Create a vector that's from the enemy to the player with an upwards boost.
        Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

        // Add a force to the player in the direction of the vector and multiply by the hurtForce.
        GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

        // Reduce the player's health by 10.
        health -= damageAmount;

        audioSource.clip = hurtClip;
        audioSource.Play();

    }

}
