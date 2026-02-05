using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI m_label;
    private UpgradeType m_type;

    public void Setup(UpgradeType type)
    {
        m_type = type;
        m_label.text = type.ToString();
    }

    public void Select()
    {
        UpgradeManager.Instance.ApplyUpgrade(m_type);
    }
}