using UnityEngine;

public class Pellet : MonoBehaviour
{
    public static event System.Action OnCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnCollected?.Invoke();
        Destroy(gameObject);
    }
}