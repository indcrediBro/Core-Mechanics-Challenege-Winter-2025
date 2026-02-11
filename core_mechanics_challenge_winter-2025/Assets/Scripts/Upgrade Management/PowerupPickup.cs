
using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    [SerializeField] private PowerupDefinition[] definition;
    private PowerupDefinition currentDefinition;

    private void OnEnable()
    {
        currentDefinition = definition[Random.Range(0, definition.Length)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PowerupSystem system))
            return;

        FindFirstObjectByType<ScreenFlash>().TriggerFlash();
        system.Activate(currentDefinition);
        GameManager.Instance.RaisePowerUp(currentDefinition.Description);
        gameObject.SetActive(false);
    }
}