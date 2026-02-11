// using UnityEngine;
//
// public enum UpgradeType
// {
//     FireRate,
//     BulletDamage,
//     BulletSize,
//     BulletPierce,
//     MoveSpeed,
//     FrontCannon,
//     RearCannon,
//     ExtraFirePoint,
//     PlayerHeal,
//     PlayerMaxHealth,
//     BaseHeal,
//     BaseMaxHealth
// }
//
// public static class UpgradeFactory
// {
//     public static void Apply(
//         UpgradeType type,
//         PlayerStats stats,
//         PlayerHealth playerHealth,
//         BaseHealth baseHealth)
//     {
//         switch (type)
//         {
//             case UpgradeType.FireRate:
//                 stats.FireRate = Mathf.Max(
//                     PlayerStats.MIN_FIRE_RATE,
//                     stats.FireRate * 0.85f
//                 );
//                 break;
//
//             case UpgradeType.BulletDamage:
//                 stats.Damage = Mathf.Min(
//                     PlayerStats.MAX_DAMAGE,
//                     stats.Damage + 1
//                 );
//                 break;
//
//             case UpgradeType.BulletSize:
//                 stats.BulletSize = Mathf.Min(
//                     PlayerStats.MAX_BULLET_SIZE,
//                     stats.BulletSize + 0.2f
//                 );
//                 break;
//
//             case UpgradeType.BulletPierce:
//                 stats.BulletPierce = Mathf.Min(
//                     PlayerStats.MAX_PIERCE,
//                     stats.BulletPierce + 1
//                 );
//                 break;
//
//             case UpgradeType.MoveSpeed:
//                 stats.MoveSpeed = Mathf.Min(
//                     PlayerStats.MAX_MOVE_SPEED,
//                     stats.MoveSpeed + 0.5f
//                 );
//                 break;
//
//             case UpgradeType.FrontCannon:
//                 stats.FrontCannonLevel = Mathf.Min(3, stats.FrontCannonLevel + 1);
//                 break;
//
//             case UpgradeType.RearCannon:
//                 stats.RearCannonEnabled = true;
//                 break;
//
//             case UpgradeType.ExtraFirePoint:
//                 stats.FirePointLevel = Mathf.Min(3, stats.FirePointLevel + 1);
//                 break;
//
//             case UpgradeType.PlayerHeal:
//                 playerHealth.Heal(5);
//                 break;
//
//             case UpgradeType.PlayerMaxHealth:
//                 playerHealth.SetMaxHealth(playerHealth.GetMaxHealthValue() + 5);
//                 break;
//
//             case UpgradeType.BaseHeal:
//                 baseHealth.Heal(10);
//                 break;
//
//             case UpgradeType.BaseMaxHealth:
//                 baseHealth.SetMaxHealth(baseHealth.GetMaxHealthValue() + 10);
//                 break;
//         }
//     }
// }
