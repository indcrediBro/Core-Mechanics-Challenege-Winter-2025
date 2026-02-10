using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private int health = 20;
    [SerializeField] private float speed = 3f;

    private bool initialized;

    public bool IsDead => health <= 0;

    public void Initialize()
    {
        initialized = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!initialized || IsDead)
            return;

        // Simple left-right patrol
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
    }

    public void Explode()
    {
        Destroy(gameObject);
    }
}