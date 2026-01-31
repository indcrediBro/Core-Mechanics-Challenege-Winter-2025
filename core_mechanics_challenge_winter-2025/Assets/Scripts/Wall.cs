using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.transform.name);
    }
}
