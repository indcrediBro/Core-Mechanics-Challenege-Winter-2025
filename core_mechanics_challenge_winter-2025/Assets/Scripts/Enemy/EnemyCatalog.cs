using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Enemy Catalog")]
public class EnemyCatalog : ScriptableObject
{
    public List<string> normalEnemies;
    public List<string> bossEnemies;
}