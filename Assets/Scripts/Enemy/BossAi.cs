using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAi : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range; // Attack range
    [SerializeField] private float colliderDistance; // Attack collider distance
    [SerializeField] private float aggroRange; // Distance where the boss starts chasing
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed; // Speed for moving toward the player
    [SerializeField] private BoxCollider2D boxCollider; // Attack range collider
    [SerializeField] private LayerMask playerLayer;

    [Header("Summon Attack")]
    [SerializeField] private BoxCollider2D summonCollider; // The collider for the summon attack
    [SerializeField] private float expansionSpeed = 2f; // Speed of expanding the summon collider
    [SerializeField] private float maxExpansionSize = 10f; // Maximum size of the summon collider
    [SerializeField] private float pushForce = 10f; // Force to push the player away

    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private Health playerHealth;
    private Transform player;
    private bool playerInAggroRange; // Player within chase range
    private bool playerInAttackRange; // Player within attack range
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip attackSound2;
    [SerializeField] private AudioClip attackSound3;

    [SerializeField] private float turnCooldown = 2f;
    private float lastTurnTime;
    private bool facingRight = true;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        summonCollider.enabled = false; // Disable summon collider initially
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        if (summonCollider == null)
            summonCollider = GetComponent<BoxCollider2D>();

        if (anim == null)
            anim = GetComponent<Animator>();

        // Disable summon collider initially
        summonCollider.enabled = false;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Check if player is in aggro range (to start chasing)
        playerInAggroRange = PlayerInAggroRange();

        // Check if player is in attack range (to start attacking)
        playerInAttackRange = PlayerInAttackRange();

        if (playerInAggroRange && !playerInAttackRange)
        {
            // Chase the player if within aggro range but not in attack range
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

    // Function to move toward the player
    private void MoveTowardPlayer()
    {
        if (player != null)
        {
            // Move towards the player's x position while keeping the y position constant
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Calculate the direction to the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Check if enough time has passed since the last turn
            if (Time.time >= lastTurnTime + turnCooldown)
            {
                // Only turn if the player is on the opposite side
                if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
                {
                    // Turn the boss to face the player
                    TurnBoss(direction.x);
                }
            }
        }
    }

    private void TurnBoss(float directionX)
    {
        if (directionX > 0)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingRight = true;
        }
        else
        {
            // Face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingRight = false;
        }

        // Reset the cooldown timer after turning
        lastTurnTime = Time.time;
    }

    // Perform a random attack
    private void PerformRandomAttack()
    {
        int attackIndex = Random.Range(0, 3); // Randomly select one of the three attacks

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

    // Check if the player is in attack range
    private bool PlayerInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            player = hit.transform; // Store player's position

            // Safely get the Health component
            playerHealth = hit.transform.GetComponent<Health>();
            if (playerHealth == null)
            {
                Debug.LogError("Player does not have a Health component!");
            }
        }

        return hit.collider != null;
    }

    // Check if the player is in the boss's aggro range (for chasing)
    private bool PlayerInAggroRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, aggroRange, playerLayer);

        if (hit != null)
        {
            player = hit.transform; // Store the player's position
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit != null;
    }

    private void OnDrawGizmos()
    {
        // Visualize attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        // Visualize aggro range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    // Function to deal damage to the player
    private void DamagePlayer()
    {
        if (PlayerInAttackRange())
        {
            playerHealth.TakeDamage(damage);
        }
    }

    // Animation event triggered when the boss raises hands (summon animation)
    public void SummonEvent()
    {
        Debug.Log("Summon Event Triggered");
        StartCoroutine(ExpandSummonCollider());
    }

    private IEnumerator ExpandSummonCollider()
    {
        summonCollider.enabled = true;
        Vector2 originalSize = summonCollider.size;
        Vector2 currentSize = originalSize;

        Debug.Log("Summon Collider Expanding");

        while (currentSize.x < maxExpansionSize)
        {
            currentSize += new Vector2(expansionSpeed * Time.deltaTime, 0); // Expand left and right
            summonCollider.size = currentSize;
            yield return null; // Wait for the next frame
        }

        Debug.Log("Summon Collider Max Expansion Reached");

        yield return new WaitForSeconds(2f); // Collider stays active for 2 seconds after full expansion

        // After the collider has fully expanded, deactivate it
        summonCollider.enabled = false;
        summonCollider.size = originalSize; // Reset the size for next use

        Debug.Log("Summon Collider Reset");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && summonCollider.enabled)
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;

                // Continuously push the player over a short time for more noticeable effect
                StartCoroutine(PushPlayer(playerRb, pushDirection));
            }
        }
    }

    private IEnumerator PushPlayer(Rigidbody2D playerRb, Vector2 pushDirection)
    {
        float pushDuration = 0.5f; // Duration of the push
        float pushForce = 200f; // Stronger push

        float elapsed = 0f;
        while (elapsed < pushDuration)
        {
            playerRb.AddForce(pushDirection * pushForce * Time.deltaTime, ForceMode2D.Force); // Apply force over time
            elapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Player Pushed with continuous force");
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
