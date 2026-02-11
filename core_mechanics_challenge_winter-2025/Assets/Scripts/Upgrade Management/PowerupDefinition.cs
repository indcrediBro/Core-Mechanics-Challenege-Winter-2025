using UnityEngine;

public enum PowerupType
{
    BaseHeal,
    PlayerHeal,
    PlayerShield,
    FreezeEnemies,
    KillAllEnemies
}

[CreateAssetMenu(menuName = "Powerups/Powerup")]
public class PowerupDefinition : ScriptableObject
{
    public PowerupType Type;
    public float Duration;
    public int Amount;
    public string Description;
}