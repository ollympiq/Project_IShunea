using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{

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
        Debug.Log("TakeDamage called with damage: " + _damage);
        if (!healthBarCanvas.activeSelf)
            healthBarCanvas.SetActive(true);

        

        if (currentHealth <= 0)
        {
            healthBarCanvas.SetActive(false);
            
        }
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        Debug.Log("Enemy HP: " + currentHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            SoundManager.instance.PlaySound(hitSound);
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                healthBarCanvas.SetActive(false);
                SoundManager.instance.PlaySound(deathSound);
                if (GetComponentInParent<EnemyPatrol>()!=null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;

                if (GetComponent<MeleeEnemy>() != null)
                    GetComponent<MeleeEnemy>().enabled = false;
                dead = true;
       
            }
        }


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
}
