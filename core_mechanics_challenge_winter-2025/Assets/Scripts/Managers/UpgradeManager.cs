using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private HealthComponent m_playerHealth;
    [SerializeField] private HealthComponent m_baseHealth;

    [SerializeField] private int m_choicesPerWave = 3;

    private readonly List<UpgradeType> m_allUpgrades = new()
    {
        UpgradeType.FireRate,
        UpgradeType.BulletDamage,
        UpgradeType.BulletSize,
        UpgradeType.BulletPierce,
        UpgradeType.MoveSpeed,
        UpgradeType.FrontCannon,
        UpgradeType.RearCannon,
        UpgradeType.ExtraFirePoint,
        UpgradeType.PlayerHeal,
        UpgradeType.PlayerMaxHealth,
        UpgradeType.BaseHeal,
        UpgradeType.BaseMaxHealth
    };

    public List<UpgradeType> GetUpgradeChoices()
    {
        List<UpgradeType> valid = new();

        foreach (var upgrade in m_allUpgrades)
        {
            if (IsUpgradeValid(upgrade))
                valid.Add(upgrade);
        }

        Shuffle(valid);

        if (valid.Count > m_choicesPerWave)
            valid.RemoveRange(m_choicesPerWave, valid.Count - m_choicesPerWave);

        return valid;
    }

    public void ApplyUpgrade(UpgradeType type)
    {
        UpgradeFactory.Apply(
            type,
            GameManager.Instance.GetPlayer().GetStats(),
            m_playerHealth,
            m_baseHealth
        );

        Debug.Log("Upgrades complete of type:" + type.ToString());

        GameManager.Instance.GetPlayer().RebuildWeapon();
        GameManager.Instance.CloseUpgrades();
    }

    private bool IsUpgradeValid(UpgradeType type)
    {
        return type switch
        {
            UpgradeType.FireRate        => GameManager.Instance.GetPlayer().GetStats().FireRate > PlayerStats.MIN_FIRE_RATE,
            UpgradeType.BulletDamage    => GameManager.Instance.GetPlayer().GetStats().Damage < PlayerStats.MAX_DAMAGE,
            UpgradeType.BulletSize      => GameManager.Instance.GetPlayer().GetStats().BulletSize < PlayerStats.MAX_BULLET_SIZE,
            UpgradeType.BulletPierce    => GameManager.Instance.GetPlayer().GetStats().BulletPierce < PlayerStats.MAX_PIERCE,
            UpgradeType.MoveSpeed       => GameManager.Instance.GetPlayer().GetStats().MoveSpeed < PlayerStats.MAX_MOVE_SPEED,
            UpgradeType.FrontCannon     => GameManager.Instance.GetPlayer().GetStats().FrontCannonLevel < 3,
            UpgradeType.RearCannon      => !GameManager.Instance.GetPlayer().GetStats().RearCannonEnabled,
            UpgradeType.ExtraFirePoint  => GameManager.Instance.GetPlayer().GetStats().FirePointLevel < 3,
            UpgradeType.PlayerHeal      => m_playerHealth.Health.Current < m_playerHealth.Health.Max,
            UpgradeType.PlayerMaxHealth => true,
            UpgradeType.BaseHeal        => m_baseHealth.Health.Current < m_baseHealth.Health.Max,
            UpgradeType.BaseMaxHealth   => true,
            _ => false
        };
    }

    private void Shuffle(List<UpgradeType> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
