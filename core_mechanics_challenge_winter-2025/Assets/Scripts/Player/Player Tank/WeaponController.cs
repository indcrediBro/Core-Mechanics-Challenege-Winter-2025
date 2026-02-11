using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform[] m_frontCannons; // size 3
    [SerializeField] private Transform   m_rearCannon;

    public List<Transform> GetActiveFirePoints(PlayerStatistics stats)
    {
        var result = new List<Transform>();

        for (int i = 0; i < stats.FrontCannonLevel; i++)
        {
            AddFirePoints(m_frontCannons[i], stats.FirePointLevel, result);
        }

        if (stats.RearCannonEnabled)
            AddFirePoints(m_rearCannon, stats.FirePointLevel, result);

        return result;
    }

    private void AddFirePoints(Transform cannon, int level, List<Transform> list)
    {
        cannon.gameObject.SetActive(true);

        for (int i = 0; i < level; i++)
        {

            Transform fp = cannon.GetChild(i);
            fp.gameObject.SetActive(true);
            list.Add(fp);
        }
    }
}