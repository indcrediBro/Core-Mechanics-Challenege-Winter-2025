using UnityEngine;

public class EnemyHealth : Health
{
    [Space] [SerializeField] private int pointsForKill;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        Vector3 pos = transform.position;
        int r = Random.Range(0, 100);
        if (r < 10)
        {
            ObjectPoolManager.Instance.SpawnPooledObject("PowerUp",
                transform.position,
                Quaternion.identity
            ).SetActive(true);
        }

        ScoreManager.Instance.AddScore(pointsForKill,pos);
        ObjectPoolManager.Instance.SpawnPooledObject("Explosion", transform.position, Quaternion.identity).SetActive(true);
    }
}