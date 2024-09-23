using System.Collections;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    [SerializeField] private AudioClip hitSound;
    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    
    public void OnHit()
    {
        SoundManager.instance.PlaySound(hitSound);
        StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f); 
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white; 
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
    }
}