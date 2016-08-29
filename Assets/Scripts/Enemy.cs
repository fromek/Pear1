using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float moveSpeed = 1000f;
    public int HP = 2;
    public int damage = 1;
    public AudioClip deathClip;
    public bool CanBeControlledByPlayer = false; 

    private Transform frontCheck;
    private Transform playerCheck;
    private bool dead = false;
    private AudioSource audioSource;
    private bool isControlledByPlayer = false;
    public bool IsFriend = false;
    public bool CanJump = false;
    public float RelativeJoint2D_Offset_X = 0.25f;
    public float RelativeJoint2D_Offset_Y = 0.2f;
    Animator anim;
    public GameHelper.CharacterDirection direction = GameHelper.CharacterDirection.Left;

    void Awake()
    {
        GetComponent<RelativeJoint2D>().linearOffset = new Vector2(RelativeJoint2D_Offset_X, RelativeJoint2D_Offset_Y);
        frontCheck = transform.Find("frontCheck").transform;
        playerCheck = transform.Find("playerCheck").transform;
        audioSource = GetComponent<AudioSource>();
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public void SetControll(bool byHero, GameHelper.CharacterDirection direc)
    {
        if(CanBeControlledByPlayer)
            isControlledByPlayer = byHero;

        if (direction != direc)
            Flip();

        IsFriend = isControlledByPlayer;
        anim.SetBool("IsControlledByPlayer", isControlledByPlayer);
    }

    void FixedUpdate()
    {
        if (!isControlledByPlayer)
        {
            CheckCollision("Enemies");
            CheckCollision("Obstacles");
            CheckCollision("DestructibleObstacles");

            // Set the enemy's velocity to moveSpeed in the x direction.
            GetComponent<Rigidbody2D>().velocity = new Vector2(-(transform.localScale.x * moveSpeed), GetComponent<Rigidbody2D>().velocity.y);
        }
        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (HP <= 0 && !dead)
            // ... call the death function.
            Death();
    }

    private void  CheckCollision(string LayerName)
    {
        if (!isControlledByPlayer)
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
    }

    public void Move(float x, float speed)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(x, rb.velocity.y);
        SetSpeed(speed);
    }
    public void SetSpeed(float speed)
    {
        anim.SetFloat("vSpeed", Mathf.Abs(speed));
    }

    public void Jump(float jumpForce)
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0f, jumpForce*30));
    }

    public void Flip()
    {
        // Multiply the x component of localScale by -1.
        if (direction == GameHelper.CharacterDirection.Right)
            direction = GameHelper.CharacterDirection.Left;
        else
            direction = GameHelper.CharacterDirection.Right;

        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= - 1;
        transform.localScale = enemyScale;
        if(isControlledByPlayer)
        {
            GetComponent<RelativeJoint2D>().linearOffset = new Vector2(GetComponent<RelativeJoint2D>().linearOffset.x * -1, GetComponent<RelativeJoint2D>().linearOffset.y);
        }
    }

    public void SetConnectedBody(Rigidbody2D body)
    {
        GetComponent<RelativeJoint2D>().connectedBody = body;
    }

    public void EnableRelativeJoint2D(bool enabled)
    {
        GetComponent<RelativeJoint2D>().enabled = enabled;
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
