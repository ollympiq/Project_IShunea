using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Health : MonoBehaviour
{
    [Header("Health")]

    [SerializeField]private float startingHealth;
    private Animator anim;
    public float currentHealth { get; private set; }
    private bool dead;
    [SerializeField] private float delayBeforeSceneChange = 2f;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("iFrames")]

    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();    
    }

    public void TakeDamage(float _damage) {

        currentHealth = Mathf.Clamp(currentHealth - _damage,0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            SoundManager.instance.PlaySound(hitSound);
            StartCoroutine(Invulnerability());
        }
        else {
            if (!dead) {
                anim.SetTrigger("die");
                SoundManager.instance.PlaySound(deathSound);

                gameOverScreen.SetActive(true);
                SoundManager.instance.PlaySound(gameOverSound);
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
              
                
               
                
            }
        }
    }

   
    

     
    private IEnumerator Invulnerability() {
        Physics2D.IgnoreLayerCollision(10,11,true);
        for (int i = 0; i < numberOfFlashes; i++) {
            spriteRend.color = new Color(1,0,0,0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes*2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
