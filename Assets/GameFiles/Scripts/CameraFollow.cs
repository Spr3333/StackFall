using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 camFollow;
    private Transform player, win;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (win == null)
            win = GameObject.Find("win(Clone)").GetComponent<Transform>();

        if (transform.position.y > player.transform.position.y && transform.position.y > win.position.y + 4.7f)
            camFollow = new Vector3(transform.position.x, player.position.y, transform.position.z);

        transform.position = new Vector3(transform.position.x, camFollow.y, -5);
    }
}
