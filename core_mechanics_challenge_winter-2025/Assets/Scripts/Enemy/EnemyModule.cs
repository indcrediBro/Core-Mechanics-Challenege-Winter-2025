using UnityEngine;

public abstract class EnemyModule : ScriptableObject
{
    public virtual void OnEnter(EnemyContext ctx) { }
    public virtual void Tick(EnemyContext ctx) { }
    public virtual void OnExit(EnemyContext ctx) { }
    public virtual void OnDamage(EnemyContext ctx) { }

}