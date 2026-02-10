using UnityEngine;

public class BossFactory : Singleton<BossFactory>
{
    [SerializeField] private GameObject turretBossPrefab;
    [SerializeField] private GameObject pacmanBossPrefab;
    [SerializeField] private GameObject dinoBossPrefab;
    [SerializeField] private GameObject bonusBossPrefab;
    [SerializeField] private GameObject snakeBossPrefab;

    public BossEncounter SpawnBoss()
    {
        int index = Random.Range(0, 5);

        GameObject bossGO = index switch
        {
            0 => Instantiate(turretBossPrefab),
            1 => Instantiate(pacmanBossPrefab),
            2 => Instantiate(dinoBossPrefab),
            3 => Instantiate(bonusBossPrefab),
            _ => Instantiate(snakeBossPrefab)
        };

        return bossGO.GetComponent<BossEncounter>();
    }
}