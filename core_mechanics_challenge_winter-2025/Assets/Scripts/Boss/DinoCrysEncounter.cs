using UnityEngine;

public class DinoBossEncounter : BossEncounter
{
    [SerializeField] private GameObject dinoPrefab;
    private EnemyHealth health;

    public override void StartEncounter()
    {
        started = true;
        LevelManager.Instance.LoadBossMap("DinoArena");

        var dino = Instantiate(dinoPrefab, Vector3.zero, Quaternion.identity);
        health = dino.GetComponent<EnemyHealth>();
    }

    public override bool IsCompleted => health == null || health.IsDead();

    public override void EndEncounter()
    {
        FindObjectOfType<CameraShake>().TriggerShake(1f);
    }
}