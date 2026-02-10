using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Combat/Shooter")]
public class ShooterModule : EnemyModule
{
    public string bulletKey;
    public float fireRate = 0.6f;

    private float timer;

    public override void OnEnter(EnemyContext ctx)
    {
        timer = 0f;
    }

    public override void Tick(EnemyContext ctx)
    {
        timer -= ctx.deltaTime;
        if (timer > 0f) return;

        GameObject b = ObjectPoolManager.Instance.SpawnPooledObject(
            bulletKey,
            ctx.firePoint.position,
            ctx.firePoint.rotation
        );

        b.SetActive(true);
        timer = fireRate;
    }
}