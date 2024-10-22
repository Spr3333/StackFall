using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Refrences")]
    private Rigidbody rb;

    private bool smash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            smash = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            smash = false;
        }

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            smash = true;
            rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
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
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!smash)
        {
            rb.velocity = new Vector3(0, 50 * Time.fixedDeltaTime * 5, 0);
        }
    }
}
