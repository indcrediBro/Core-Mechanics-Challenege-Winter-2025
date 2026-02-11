using UnityEngine;

[CreateAssetMenu(menuName = "Player/Modules/Shooting")]
public class PlayerShootingModule : PlayerModule
{
    private float timer;

    public override void Tick(PlayerContext ctx)
    {
        timer -= Time.deltaTime;

        if (!ctx.Input.ShootPressed || timer > 0f)
            return;

        var firePoints =
            ctx.Controller.WeaponRig.GetActiveFirePoints(ctx.Stats);

        foreach (var fp in firePoints)
        {
            GameObject bullet =
                ObjectPoolManager.Instance.SpawnPooledObject(
                    ctx.Controller.BulletKey,
                    fp.position,
                    fp.rotation
                );

            if (bullet.TryGetComponent(out Bullet b))
            {
                b.SetDamage(ctx.Stats.Damage);
                b.SetNewMaxHealth(ctx.Stats.BulletPierce);
                bullet.transform.localScale =
                    Vector3.one * ctx.Stats.BulletSize;
            }

            bullet.SetActive(true);
        }

        timer = ctx.Stats.FireRate;
        ctx.Input.ConsumeShoot();
    }
}