using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerJumpSpeed = 6f;
    private Rigidbody2D playerBody;
    

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        playerBody.velocity = new Vector2(Input.GetAxis("Horizontal")*playerSpeed,playerBody.velocity.y);

        if (Input.GetKey(KeyCode.Space)) 
        playerBody.velocity = new Vector2 (playerBody.velocity.x, playerJumpSpeed);
    }
}
