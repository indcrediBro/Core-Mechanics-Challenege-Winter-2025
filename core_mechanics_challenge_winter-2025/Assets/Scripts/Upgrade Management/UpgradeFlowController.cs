using System.Collections.Generic;
using UnityEngine;

public class UpgradeFlowController : MonoBehaviour
{
    [SerializeField] private UpgradePanel panel;
    [SerializeField] private List<UpgradeDefinition> allUpgrades;
    [SerializeField] private int choicesCount = 3;

    private void OnEnable()
    {
        RunManager.Instance.OnUpgradePhaseStarted += BeginUpgradePhase;
    }

    private void OnDisable()
    {
        RunManager.Instance.OnUpgradePhaseStarted -= BeginUpgradePhase;
    }

    private void BeginUpgradePhase()
    {
        Time.timeScale = 0f;

        List<UpgradeDefinition> choices = GetRandomUpgrades();

        panel.Show(choices, OnUpgradeSelected);
    }

    private List<UpgradeDefinition> GetRandomUpgrades()
    {
        List<UpgradeDefinition> pool = new(allUpgrades);
        List<UpgradeDefinition> result = new();

        for (int i = 0; i < choicesCount && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }

    private void OnUpgradeSelected(UpgradeDefinition def)
    {
        UpgradeManager.Instance.ApplyUpgrade(def);

        Time.timeScale = 1f;

        RunManager.Instance.AdvanceWave();

        UIManager.Instance.Show(UIState.HUD);
    }
}