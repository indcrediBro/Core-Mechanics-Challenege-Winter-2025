using UnityEngine;
using System.Collections;

public class DinoBoss : BossBase
{
    [Header("Laser")]
    [SerializeField] private LineRenderer laser;
    [SerializeField] private float laserDamage = 10f;
    [SerializeField] private float fireDuration = 1.5f;
    [SerializeField] private float fireCooldown = 3f;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer sprite;

    private bool isFiring;
    private float cooldownTimer;

    private void Awake()
    {
        laser.enabled = false;
    }

    private void Update()
    {
        if (health.IsDead())
            return;

        cooldownTimer -= Time.deltaTime;

        UpdateSpriteFlip();

        if (isFiring)
            return;

        if (HasLineOfSight() && cooldownTimer <= 0f)
        {
            StartCoroutine(FireLaserRoutine());
        }
        else if(!HasLineOfSight() && cooldownTimer <= 0f)
        {
            MoveWithAvoidance(player.position);
        }
        // else
        // {
        //     Vector2 dir2D = fallbackDirs[Random.Range(0, fallbackDirs.Length)];
        //     Vector3 step = new Vector3(dir2D.x, 0, dir2D.y) * 1;
        //     MoveTowards(step);
        // }
    }

    private void UpdateSpriteFlip()
    {
        if (player == null || sprite == null)
            return;

        sprite.flipX = player.position.x < transform.position.x;
    }

    private IEnumerator FireLaserRoutine()
    {
        isFiring = true;
        cooldownTimer = fireCooldown;

        laser.enabled = true;

        float timer = fireDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            UpdateLaser();
            yield return null;
        }

        laser.enabled = false;
        isFiring = false;
    }

    private void UpdateLaser()
    {
        Vector3 origin = laser.transform.position;
        origin.y = 0;
        Vector3 dir = (player.position - origin).normalized;
        dir.y = 0;

        laser.SetPosition(0, origin);
        laser.SetPosition(1, origin + dir * sightDistance);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, sightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                GameManager.Instance
                    .GetPlayerHealth()
                    .TakeDamage(Mathf.CeilToInt(laserDamage * Time.deltaTime));
                Debug.Log("DinoCrys Attacks!");
            }
        }
    }
}
