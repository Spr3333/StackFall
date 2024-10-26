using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackShatter : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    StackController controller;
    MeshRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        controller = transform.parent.GetComponent<StackController>();
        rend = GetComponent<MeshRenderer>();
    }

    public void Shatter()
    {
        rb.isKinematic = false;
        col.enabled = false;

        Vector3 forcePoint = transform.parent.position;
        float parentXpos = transform.parent.position.x;
        float xPos = rend.bounds.center.x;

        Vector3 subDir = (parentXpos - xPos < 0) ? Vector3.right : Vector3.left;
        Vector3 dir = (Vector3.up * 1.5f + subDir).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        rb.AddForceAtPosition(dir * force, forcePoint, ForceMode.Impulse);
        rb.AddTorque(Vector3.left * torque);
        rb.velocity = Vector3.down;

    }

    public void RemoveAllChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }
}
