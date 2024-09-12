using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [Header("Firetrap Timers")]
    [SerializeField] private float delay;
    [SerializeField] private float active;
    [SerializeField] private float damageCooldown = 1f; 

    private Animator anim;
    private SpriteRenderer spriteRend;
    private Health player;

    private bool triggered;
    private bool activated;
    private float lastDamageTime; 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap());
            player = collision.GetComponent<Health>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = null;
        }
    }

    private void Update()
    {
        if (activated && player != null)
        {
            
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                player.TakeDamage(damage);
                lastDamageTime = Time.time; 
            }
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(delay);
        spriteRend.color = Color.white;
        activated = true;
        anim.SetBool("onTrap", true);

        yield return new WaitForSeconds(active);
        activated = false;
        triggered = false;
        anim.SetBool("onTrap", false);
    }
}
