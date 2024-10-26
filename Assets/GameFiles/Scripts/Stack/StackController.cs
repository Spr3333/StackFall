using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] private StackShatter[] stackPartColntrols = null;

    public void ShatterAllParts()
    {
        if (transform.parent != null)
            transform.parent = null;

        foreach (StackShatter item in stackPartColntrols)
        {
            item.Shatter();
        }

        StartCoroutine(DeleteParts());
    }

    IEnumerator DeleteParts()
    {
        yield return new WaitForSeconds(1);
        foreach (StackShatter item in stackPartColntrols)
        {
            item.RemoveAllChilds();
        }
        Destroy(gameObject);
    }
}
