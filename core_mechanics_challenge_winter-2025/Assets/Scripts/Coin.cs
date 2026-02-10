using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static event Action OnCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        OnCollected?.Invoke();
        Destroy(gameObject);
    }
}