using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Refrences")]
    private Rigidbody rb;

    private float currentTime;

    private bool smash, invincible;

    public enum PlayerState
    {
        Preperation,
        Playing,
        Dead,
        Finish
    }

    public PlayerState state = PlayerState.Preperation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(state);
        if (state == PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                smash = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                smash = false;
            }

            if (invincible)
            {
                currentTime -= Time.deltaTime * .35f;
            }
            else
            {
                if (smash)
                    currentTime += Time.deltaTime * .8f;
                else
                    currentTime -= Time.deltaTime * .5f;

            }

            if (currentTime >= 1)
            {
                currentTime = 1;
                invincible = true;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
            }
        }

        if (state == PlayerState.Preperation)
        {
            if (Input.GetMouseButtonDown(0))
            {
                state = PlayerState.Playing;
            }
        }

        if (state == PlayerState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
                FindObjectOfType<LevelManager>().NextLevel();
        }
    }

    private void FixedUpdate()
    {
        if ((state == PlayerState.Playing))
        {
            if (Input.GetMouseButton(0))
            {
                smash = true;
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }

        if (rb.velocity.y > 5)
        {
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!smash)
        {
            rb.velocity = new Vector3(0, 50 * Time.fixedDeltaTime * 5, 0);
        }
        else
        {

            if (invincible)
            {
                if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("plane"))
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("enemy"))
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }

                if (collision.gameObject.CompareTag("plane"))
                {
                    Debug.Log("Game Over");
                }
            }

        }

        if (collision.gameObject.CompareTag("Finish") && state == PlayerState.Playing)
        {
            state = PlayerState.Finish;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!smash || collision.gameObject.CompareTag("Finish"))
        {
            rb.velocity = new Vector3(0, 50 * Time.fixedDeltaTime * 5, 0);
        }
    }
}
