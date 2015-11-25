using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    public float bombRadius = 10f;          // Radius within which enemies are killed.
    public float bombForce = 100f;          // Force that enemies are thrown from the blast.
    public AudioClip boom;                  // Audioclip of explosion.
    public AudioClip fuse;                  // Audioclip of fuse.
    public float fuseTime = 1.5f;
    public GameObject explosion;            // Prefab of explosion effect.


    private LayBombs layBombs;              // Reference to the player's LayBombs script.
    private ParticleSystem explosionFX;     // Reference to the particle system of the explosion effect.


    void Awake()
    {
        // Setting up references.
        //explosionFX = GameObject.FindGameObjectWithTag("ExplosionFX").GetComponent<ParticleSystem>();
        if (GameObject.FindGameObjectWithTag("Player"))
            layBombs = GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
    }

    void Start()
    {

        // If the bomb has no parent, it has been laid by the player and should detonate.
        if (transform.root == transform)
            StartCoroutine(BombDetonation());
    }


    IEnumerator BombDetonation()
    {
        // Play the fuse audioclip.
        //AudioSource.PlayClipAtPoint(fuse, transform.position);

        // Wait for 2 seconds.
        yield return new WaitForSeconds(fuseTime);

        // Explode the bomb.
        Explode();
    }


    private void DetectCollision(Collider2D[] colliders)
    {
        // For each collider...
        foreach (Collider2D en in colliders)
        {
            // Check if it has a rigidbody (since there is only one per enemy, on the parent).
            
            bool canContinue = false;
            if (en.tag == "Enemy")
            {
                en.gameObject.GetComponent<Enemy>().HP = 0;
                canContinue = true;
            }
            if(en.tag == "Obstacle")
            {
                en.gameObject.GetComponent<ObstacleHealth>().HP = 0;
                canContinue = true;
            }
            if (en.tag == "Bridge")
            {
                en.gameObject.GetComponent<BridgeHealth>().HP = 0;
                canContinue = true;
            }

            if (canContinue)
            {
                Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
                if(rb != null)
                {
                    // Find a vector from the bomb to the enemy.
                    Vector3 deltaPos = rb.transform.position - transform.position;

                    // Apply a force in this direction with a magnitude of bombForce.
                    Vector3 force = deltaPos.normalized * bombForce;
                    rb.AddForce(force);
                }
                
            }
        }
    }

    public void Explode()
    {

        // The player is now free to lay bombs when he has them.
        layBombs.bombLaid = false;

        // Find all the colliders on the Enemies layer within the bombRadius.
        DetectCollision(Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemies")));
        DetectCollision(Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("DestructibleObstacles")));
        DetectCollision(Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("DestructibleBridges")));
        
        // Set the explosion effect's position to the bomb's position and play the particle system.
        // explosionFX.transform.position = transform.position;
        // explosionFX.Play();

        // Instantiate the explosion prefab.
        Instantiate(explosion, transform.position, Quaternion.identity);

        // Play the explosion sound effect.
        //AudioSource.PlayClipAtPoint(boom, transform.position);

        // Destroy the bomb.
        Destroy(gameObject);
    }
}
