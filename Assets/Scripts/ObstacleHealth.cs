using UnityEngine;
using System.Collections;

public class ObstacleHealth : MonoBehaviour {

    public int HP = 5;
    public AudioClip destroyedClip;
    private AudioSource audioSource;
    private bool destroyed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {

        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !destroyed)
            // ... call the death function.
            Death();
    }

    public void Hurt()
    {
        // Reduce the number of hit points by one.
        HP--;
    }

    void Death()
    {
        // Find all of the sprite renderers on this object and it's children.
        SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Disable all of them sprite renderers.
        foreach (SpriteRenderer s in otherRenderers)
        {
            s.enabled = false;
        }


        // Set dead to true.
        destroyed = true;


        // Find all of the colliders on the gameobject and set them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        if (audioSource != null && destroyedClip != null)
        {
            audioSource.clip = destroyedClip;
            audioSource.Play();
        }

    }
}
