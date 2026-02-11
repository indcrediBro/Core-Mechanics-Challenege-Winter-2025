using UnityEngine;

public enum UpgradeType
{
    FireRate,
    BulletDamage,
    BulletPierce,
    MoveSpeed,
    FrontCannon,
    RearCannon,
    PlayerMaxHealth,
    BaseMaxHealth
}

[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class UpgradeDefinition : ScriptableObject
{
    public UpgradeType Type;
    public float FloatValue;
    public int IntValue;
}