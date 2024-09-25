using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject chestPrefab;
    public GameObject healthBarCanvas;
    [SerializeField] private float startingHealth;
    private Animator anim;
    public float currentHealth { get; private set; }
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;

    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    private SpriteRenderer spriteRend;

    [Header("Hit Mechanics")]
    [SerializeField] private int maxHitsBeforeInvulnerability = 3; 
    private int currentHitCount = 0; 
    private bool isInvulnerable = false; 
    [SerializeField] private float invulnerabilityDuration = 1f; 

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public float GetStartingHealth()
    {
        return startingHealth;
    }

    public void TakeDamage(float _damage)
    {
        
        if (isInvulnerable || dead) return;

        Debug.Log("TakeDamage called with damage: " + _damage);

        if (!healthBarCanvas.activeSelf)
            healthBarCanvas.SetActive(true);

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            SoundManager.instance.PlaySound(hitSound);

            
            currentHitCount++;

            if (currentHitCount >= maxHitsBeforeInvulnerability)
            {
                StartCoroutine(HandleInvulnerability());
            }
            else
            {
                StartCoroutine(Invulnerability());
            }
        }
        else
        {
            if (!dead)
            {
                Die();
            }
        }
    }

    private IEnumerator HandleInvulnerability()
    {
        isInvulnerable = true;
        currentHitCount = 0; 

        
        spriteRend.color = Color.blue;

       
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(0, 0, 1, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.blue; // Solid blue
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        yield return new WaitForSeconds(invulnerabilityDuration); 

        
        spriteRend.color = Color.white;
        isInvulnerable = false;
    }

    private void Die()
    {
        dead = true;
        anim.SetTrigger("die");
        healthBarCanvas.SetActive(false);
        SoundManager.instance.PlaySound(deathSound);

        if (GetComponentInParent<EnemyPatrol>() != null)
            GetComponentInParent<EnemyPatrol>().enabled = false;

        if (GetComponent<MeleeEnemy>() != null)
            GetComponent<MeleeEnemy>().enabled = false;
        SpawnChest();
    }

    private IEnumerator Invulnerability()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
    }

    private void DeActivate()
    {
        gameObject.SetActive(false);
    }
    private void SpawnChest()
    {
        if (chestPrefab != null)
        {
            Instantiate(chestPrefab, transform.position, Quaternion.identity);
        }
    }
}
