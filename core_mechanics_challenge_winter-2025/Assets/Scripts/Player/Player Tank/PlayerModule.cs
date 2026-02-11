using UnityEngine;

public abstract class PlayerModule : ScriptableObject
{
    public virtual void Initialize(PlayerContext ctx) { }
    public virtual void Tick(PlayerContext ctx) { }
    public virtual void FixedTick(PlayerContext ctx) { }
}