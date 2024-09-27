using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float aggroRange;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    


    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth;
    private Transform player;
    private bool playerInAggroRange;
    private bool playerInAttackRange;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip attackSound2;
    [SerializeField] private AudioClip attackSound3;

    [SerializeField] private float turnCooldown = 2f;
    private float lastTurnTime;
    private bool facingRight = true;
    private Rigidbody2D playerRb;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;


        playerInAggroRange = PlayerInAggroRange();


        playerInAttackRange = PlayerInAttackRange();

        if (playerInAggroRange && !playerInAttackRange)
        {

            MoveTowardPlayer();
        }

        if (playerInAttackRange && playerHealth.currentHealth > 0)
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                PerformRandomAttack();
            }
        }
    }


    private void MoveTowardPlayer()
    {
        if (player != null)
        {

            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);


            Vector3 direction = (player.position - transform.position).normalized;


            if (Time.time >= lastTurnTime + turnCooldown)
            {

                if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
                {

                    TurnBoss(direction.x);
                }
            }
        }
    }

    private void TurnBoss(float directionX)
    {
        if (directionX > 0)
        {

            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingRight = true;
        }
        else
        {

            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingRight = false;
        }


        lastTurnTime = Time.time;
    }


    private void PerformRandomAttack()
    {
        int attackIndex = Random.Range(0, 3);

        switch (attackIndex)
        {
            case 0:
                anim.SetTrigger("attack1");
                break;
            case 1:
                anim.SetTrigger("attack2");
                break;
            case 2:
                anim.SetTrigger("summon");
                break;
        }
    }

    public void SummonEvent()
    {
        if (player != null)
        {
            StartCoroutine(HandleSummonEffect());
        }
    }

    private IEnumerator HandleSummonEffect()
    {
        
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false; 
        }

       
        playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.gravityScale = 0f;

            
            playerRb.velocity = new Vector2(0f, 1f);
        }

        
        yield return new WaitForSeconds(2f);

        
        if (playerRb != null)
        {
            playerRb.gravityScale = 1f; 
        }

        
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    private bool PlayerInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            player = hit.transform;

            
            playerHealth = hit.transform.GetComponent<Health>();
            if (playerHealth == null)
            {
                Debug.LogError("Player does not have a Health component!");
            }
        }

        return hit.collider != null;
    }


    private bool PlayerInAggroRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, aggroRange, playerLayer);

        if (hit != null)
        {
            player = hit.transform;
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit != null;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }


    private void DamagePlayer()
    {
        if (PlayerInAttackRange())
        {
            playerHealth.TakeDamage(damage);
        }
    }


    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;



            }
        }
    }



    public void SoundPLay()
    {
        SoundManager.instance.PlaySound(attackSound);
    }
    public void SoundPLay2()
    {
        SoundManager.instance.PlaySound(attackSound2);
    }
    public void SoundPLay3()
    {
        SoundManager.instance.PlaySound(attackSound3);
    }
}
