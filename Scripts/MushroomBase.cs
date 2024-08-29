using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer SR;

    public float speed = 2f;
    private float timer;

    public float startWaitTime = 2f;
    public Transform[] partrolPoints;

    private int i = 1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector2(partrolPoints[0].position.x, partrolPoints[0].position.y);
        rb.gravityScale = 1f;
        timer = startWaitTime;
    }


    void Update()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, partrolPoints[i].transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, partrolPoints[i].transform.position) < 0.1f)
        {
            if (timer <= 0)
            {
                if (partrolPoints[i] != partrolPoints[partrolPoints.Length - 1])
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
                timer = startWaitTime;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        CheckMove();
    }

    private void CheckMove()
    {
        if (transform.position.x > partrolPoints[i].transform.position.x)
        {
            SR.flipX = false;
            anim.SetBool("Idle", false);
        }
        else if (transform.position.x < partrolPoints[i].transform.position.x)
        {
            SR.flipX = true;
            anim.SetBool("Idle", false);
        }
        else
        {
            anim.SetBool("Idle", true);
        }
    }
}