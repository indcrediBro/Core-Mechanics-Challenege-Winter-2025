using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private Button button;

    private UpgradeDefinition definition;
    private System.Action<UpgradeDefinition> callback;

    public void Initialize(
        UpgradeDefinition def,
        System.Action<UpgradeDefinition> onClick
    )
    {
        definition = def;
        callback = onClick;

        title.text = def.Type.ToString();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        callback?.Invoke(definition);
        GameManager.Instance.CloseUpgrades();
    }
}