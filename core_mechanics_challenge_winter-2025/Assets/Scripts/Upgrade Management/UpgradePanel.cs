using System;
using UnityEngine;
using System.Collections.Generic;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeButton buttonPrefab;
    [SerializeField] private Transform buttonParent;

    private Action<UpgradeDefinition> onSelected;

    public void Show(
        List<UpgradeDefinition> upgrades,
        Action<UpgradeDefinition> callback
    )
    {
        gameObject.SetActive(true);
        onSelected = callback;

        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);

        foreach (var up in upgrades)
        {
            var btn = Instantiate(buttonPrefab, buttonParent);
            btn.Initialize(up, SelectUpgrade);
        }
    }

    private void SelectUpgrade(UpgradeDefinition def)
    {
        onSelected?.Invoke(def);
        gameObject.SetActive(false);
    }
}