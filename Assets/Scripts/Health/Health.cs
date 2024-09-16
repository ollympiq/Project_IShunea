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

    [SerializeField] private TextMeshProUGUI deathMessageText;

    [Header("iFrames")]

    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        deathMessageText.gameObject.SetActive(false);
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
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
                deathMessageText.text = "YOU DIED";
                deathMessageText.gameObject.SetActive(true);
                
                StartCoroutine(ChangeSceneAfterDelay());
                
            }
        }
    }

    IEnumerator ChangeSceneAfterDelay()
    {
      
        yield return new WaitForSeconds(delayBeforeSceneChange);
        
        
        SceneManager.LoadScene(0);
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
