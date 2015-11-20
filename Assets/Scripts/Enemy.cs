using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float moveSpeed = 1000f;
    public int HP = 2;
    public int damage = 1;
    public AudioClip deathClip;

    private Transform frontCheck;
    private bool dead = false;
    private AudioSource audioSource;

    void Awake()
    {
        frontCheck = transform.Find("frontCheck").transform;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        CheckCollision("Enemies");
        CheckCollision("Obstacles");
        CheckCollision("DestructibleObstacles");

        // Set the enemy's velocity to moveSpeed in the x direction.
        GetComponent<Rigidbody2D>().velocity = new Vector2(-(transform.localScale.x * moveSpeed), GetComponent<Rigidbody2D>().velocity.y);

        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !dead)
            // ... call the death function.
            Death();
    }

    private void  CheckCollision(string LayerName)
    {
        Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1 << LayerMask.NameToLayer(LayerName));


        foreach (Collider2D c in frontHits)
        {
            if (c.tag == "Enemy" || c.tag.Equals("Obstacle"))
            {
                Flip();
                break;
            }
        }
    }

    public void Flip()
    {
        // Multiply the x component of localScale by -1.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
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
        dead = true;


        // Find all of the colliders on the gameobject and set them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }
        
        if(audioSource != null)
        {
            audioSource.clip = deathClip;
            audioSource.Play();
        }

    }
}
