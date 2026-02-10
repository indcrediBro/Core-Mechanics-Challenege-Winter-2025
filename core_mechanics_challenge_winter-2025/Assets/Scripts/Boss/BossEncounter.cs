using UnityEngine;

public abstract class BossEncounter : MonoBehaviour
{
    protected bool started;

    public abstract void StartEncounter();
    public abstract bool IsCompleted { get; }
    public abstract void EndEncounter();

    protected virtual void Update()
    {
        if (!started) return;

        if (IsCompleted)
        {
            EndEncounter();
            RunManager.Instance.BossDefeated();
            Destroy(gameObject);
        }
    }
}