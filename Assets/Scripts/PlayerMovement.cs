using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerJumpSpeed = 6f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    
    [SerializeField] private float pushPower = 2f;

    private Rigidbody2D playerBody;
    private Animator animator;
    private BoxCollider2D boxCollider;
    

    private float wallJumpCooldown;
    private float horizontalInput;


    private GameObject boxObject;
    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
      

        playerBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }

    private void Update()
    {
         horizontalInput = Input.GetAxis("Horizontal");
        

        //flip
        if(horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3 (-1,1,1);

        

        //set animator parameter

        animator.SetBool("run",horizontalInput != 0 );
        animator.SetBool("grounded", isGrounded());

        //wall jump
        if (wallJumpCooldown > 0.3f)
        {
            
            playerBody.velocity = new Vector2(horizontalInput * playerSpeed, playerBody.velocity.y);
            
                if (onWall() && !isGrounded() && horizontalInput != 0)
            {
                playerBody.gravityScale = 0.5f; 
                playerBody.velocity = new Vector2(playerBody.velocity.x, -4f);
            }
            else
                playerBody.gravityScale = 1;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else wallJumpCooldown += Time.deltaTime;

        if (boxObject != null)
        {
            Rigidbody2D boxRigidbody = boxObject.GetComponent<Rigidbody2D>();
            boxRigidbody.velocity = new Vector2(horizontalInput * pushPower, boxRigidbody.velocity.y);
        }
    }
    private void Jump() 
    {
        if (isGrounded())
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpSpeed);
        }
        else if (onWall() && !isGrounded())
        {

            playerBody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 3);
            wallJumpCooldown = 0;

        }
       
       
        
    }

    

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size,0,Vector2.down,0.1f,groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, 
            new Vector2(transform.localScale.x,0), 0.1f, wallLayer);

        return raycastHit.collider != null;
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            boxObject = collision.gameObject; // Player is touching the box
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            boxObject = null; // Player is no longer touching the box
        }
    }
}
