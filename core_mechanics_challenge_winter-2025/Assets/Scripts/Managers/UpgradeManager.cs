using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private PlayerController player;

    public void ApplyUpgrade(UpgradeDefinition upgrade)
    {
        var stats = player.Stats;

        switch (upgrade.Type)
        {
            case UpgradeType.FireRate:
                stats.FireRate =
                    Mathf.Max(0.05f, stats.FireRate - upgrade.FloatValue);
                GameManager.Instance.RaisePowerUp("Fire-Rate Upgraded!");
                break;

            case UpgradeType.BulletDamage:
                stats.Damage += upgrade.IntValue;
                GameManager.Instance.RaisePowerUp("Damage Upgraded!");
                break;

            case UpgradeType.BulletPierce:
                stats.BulletPierce += upgrade.IntValue;
                GameManager.Instance.RaisePowerUp("Piercing Upgraded!");
                break;

            case UpgradeType.MoveSpeed:
                stats.MoveSpeed += upgrade.FloatValue;
                GameManager.Instance.RaisePowerUp("Movement Upgraded!");
                break;

            case UpgradeType.FrontCannon:
                stats.FrontCannonLevel =
                    Mathf.Clamp(stats.FrontCannonLevel + 1, 1, 3);
                player.WeaponRig.GetActiveFirePoints(stats);
                GameManager.Instance.RaisePowerUp("Cannon Upgraded!");
                break;

            case UpgradeType.RearCannon:
                stats.RearCannonEnabled = true;
                player.WeaponRig.GetActiveFirePoints(stats);
                GameManager.Instance.RaisePowerUp("Ass Cannon!");
                break;

            case UpgradeType.PlayerMaxHealth:
                GameManager.Instance.GetPlayerHealth()
                    .SetMaxHealth(GameManager.Instance.GetPlayerHealth().GetMaxHealthValue() + upgrade.IntValue);
                GameManager.Instance.RaisePowerUp("Health Upgraded!");
                UIManager.Instance.UpdateLivesUI();
                break;

            case UpgradeType.BaseMaxHealth:
                BaseHealth baseHealth = GameManager.Instance.GetPlayerBase().GetComponent<BaseHealth>();
                baseHealth.SetMaxHealth(baseHealth.GetMaxHealthValue() + upgrade.IntValue);
                GameManager.Instance.RaisePowerUp("Base Upgraded!");
                UIManager.Instance.UpdateLivesUI();
                break;
        }
    }
}