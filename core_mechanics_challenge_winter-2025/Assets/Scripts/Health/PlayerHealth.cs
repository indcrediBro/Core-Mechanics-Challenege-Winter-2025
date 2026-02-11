using System;

public class PlayerHealth : Health
{
    private bool invulnerable;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Start()
    {
        UIManager.Instance.UpdateLivesUI();
    }

    public override void TakeDamage(int _damage)
    {
        if (invulnerable)
        {
            AudioManager.Instance.PlaySound("SFX_PlayerSteel");
            return;
        }

        base.TakeDamage(_damage);
        AudioManager.Instance.PlaySound("SFX_PlayerHurt");
        UIManager.Instance.UpdateLivesUI();
    }

    public void SetInvulnerable(bool _invulnerable)
    {
        invulnerable = _invulnerable;
    }
}