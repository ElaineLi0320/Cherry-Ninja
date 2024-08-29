using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private AudioSource deathSoundEffect;
    [SerializeField] private AudioSource revivalSoundEffect;
    [SerializeField] private AudioSource hitSoundEffect;
    [SerializeField] private float fallThreshold = -1000f;

    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (transform.position.y < fallThreshold && !isDead)
        {
            deathSoundEffect.Play();
            Die();
        }

        if (isDead && anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            RestartLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") && !isDead)
        {
            deathSoundEffect.Play();
            Die();
        }

        if (collision.gameObject.CompareTag("Enemy") && !isDead)
        {
            hitSoundEffect.Play();
            Die(); 
        }
    }

    public void Die()
    {
        isDead = true;
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

    private void RestartLevel()
    {
        revivalSoundEffect.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
