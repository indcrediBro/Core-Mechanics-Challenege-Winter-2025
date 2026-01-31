using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.GetComponent<Bullet>())
        {
           Destroy(gameObject);
           Destroy(other.gameObject);
        }
    }
}
