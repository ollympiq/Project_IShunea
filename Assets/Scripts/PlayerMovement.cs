using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerJumpSpeed = 6f;
    private Rigidbody2D playerBody;
    private Animator animator;
    public bool grounded;
    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        playerBody.velocity = new Vector2(horizontalInput * playerSpeed,playerBody.velocity.y);

        //flip
        if(horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3 (-1,1,1);

        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        //set animator parameter

        animator.SetBool("run",horizontalInput != 0 );
        animator.SetBool("grounded", grounded);
    }
    private void Jump() 
    {
        playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpSpeed);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") 
        grounded = true;
    }
}
