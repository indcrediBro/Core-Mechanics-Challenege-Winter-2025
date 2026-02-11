using UnityEngine;
using System.Collections;

public class PowerupSystem : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    public void Activate(PowerupDefinition def)
    {
        switch (def.Type)
        {
            case PowerupType.BaseHeal:
                BaseHealth bHealth = GameManager.Instance.GetPlayerBase().GetComponent<BaseHealth>();
                    bHealth.Heal(bHealth.GetMaxHealthValue());
                break;

            case PowerupType.PlayerHeal:
                playerHealth.Heal(playerHealth.GetMaxHealthValue());
                break;

            case PowerupType.PlayerShield:
                StartCoroutine(ShieldRoutine(def.Duration));
                break;

            case PowerupType.FreezeEnemies:
                StartCoroutine(FreezeRoutine(def.Duration));
                break;

            case PowerupType.KillAllEnemies:
                EnemySpawnDirector.Instance.KillAllActiveEnemies();
                break;
        }
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        playerHealth.SetInvulnerable(true);
        yield return new WaitForSeconds(duration);
        playerHealth.SetInvulnerable(false);
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        RunManager.Instance.freezeEnemies = true;
        yield return new WaitForSeconds(duration);
        RunManager.Instance.freezeEnemies = false;
    }
}