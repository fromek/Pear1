using UnityEngine;
using System.Collections;

public class HeroControllerScript : MonoBehaviour
{

    public float maxSpeed = 10f;
    public GameHelper.CharacterDirection direction = GameHelper.CharacterDirection.Right;
    public bool facingRight = true;
    Animator anim;
    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.1f;
    public LayerMask whatIsGround;
    public float jumpForce = 1000f;
    public bool jump = false;
    public AudioClip JumpClip;
    private AudioSource audioSource;
    private bool canContinue = true;
    public Enemy Monster;
    // Use this for initialization
    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void CheckCollision(string LayerName)
    {

        Collider2D[] frontHits = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, 1 << LayerMask.NameToLayer(LayerName));
        foreach (Collider2D c in frontHits)
        {
            if (c.tag == "Enemy" && Monster == null)
            {
                Enemy cEnemy = c.gameObject.GetComponent<Enemy>();
                if (cEnemy != null && cEnemy.CanBeControlledByPlayer)
                {
                    Monster = cEnemy;
                    //cEnemy.GetComponent<RelativeJoint2D>().enabled = true;
                    //cEnemy.GetComponent<RelativeJoint2D>().linearOffset = new Vector2(0.2f, 0.25f);
                    //cEnemy.GetComponent<RelativeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                    Monster.EnableRelativeJoint2D(true);
                    Monster.SetConnectedBody(GetComponent<Rigidbody2D>());
                    Monster.SetControll(true, direction);
                }
            }

        }
    }


    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        //anim.SetBool("Ground", grounded);
        if(Monster == null)
            CheckCollision("Enemies");

        var rb = GetComponent<Rigidbody2D>();
        float move = Input.GetAxis("Horizontal");
        if (canContinue)
        {
            anim.SetFloat("vSpeed", rb.velocity.y);

            anim.SetFloat("Speed", Mathf.Abs(move));
            rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
            if (Monster != null)
            {
                Monster.Move(move * maxSpeed, move);
            }
            if (move > 0 && direction == GameHelper.CharacterDirection.Left)
                Flip();
            else if (move < 0 && direction == GameHelper.CharacterDirection.Right)
                Flip();
        }
        if (jump)
        {
            SetJump(false);
            if(Monster == null || Monster.CanJump)
            {
                anim.SetTrigger("Jump");
                audioSource.clip = JumpClip;
                audioSource.Play();
                rb.AddForce(new Vector2(0f, jumpForce));
                if(Monster != null)
                    Monster.Jump(jumpForce);
            }
          
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {

            SetJump(true);
        }
    }

    private void SetJump(bool isJump)
    {
        jump = isJump;
    }

    void Flip()
    {
        facingRight = !facingRight;
        if (direction == GameHelper.CharacterDirection.Right)
            direction = GameHelper.CharacterDirection.Left;
        else
            direction = GameHelper.CharacterDirection.Right;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        if (Monster != null)
        {
            //Monster.GetComponent<RelativeJoint2D>().linearOffset = new Vector2(Monster.GetComponent<RelativeJoint2D>().linearOffset.x *-1, Monster.GetComponent<RelativeJoint2D>().linearOffset.y);
            Monster.Flip();
        }
    }
}