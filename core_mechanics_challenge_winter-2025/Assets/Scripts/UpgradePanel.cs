using UnityEngine;
using System.Collections.Generic;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeButton[] m_buttons;

    private void OnEnable()
    {
        Populate();
    }

    private void Populate()
    {
        List<UpgradeType> choices = UpgradeManager.Instance.GetUpgradeChoices();

        for (int i = 0; i < m_buttons.Length; i++)
        {
            if (i < choices.Count)
            {
                m_buttons[i].gameObject.SetActive(true);
                m_buttons[i].Setup(choices[i]);
            }
            else
            {
                m_buttons[i].gameObject.SetActive(false);
            }
        }
    }
}