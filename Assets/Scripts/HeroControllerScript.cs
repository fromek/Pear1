using UnityEngine;
using System.Collections;

public class HeroControllerScript : MonoBehaviour
{

    public float maxSpeed = 10f;
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
    // Use this for initialization
    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        //anim.SetBool("Ground", grounded);

        var rb = GetComponent<Rigidbody2D>();
        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("vSpeed", rb.velocity.y);

        anim.SetFloat("Speed", Mathf.Abs(move));
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        if (jump)
        {
            jump = false;
            anim.SetTrigger("Jump");
            audioSource.clip = JumpClip;
            audioSource.Play();
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {

            jump = true;
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}