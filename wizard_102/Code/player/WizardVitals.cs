using Sandbox;

namespace WizardGame;

public sealed class WizardVitals : Component
{
    [Property] public float MaxHealth { get; set; } = 100f;
    [Property] public float MaxMana { get; set; } = 100f;
    [Property] public float ManaRegenPerSecond { get; set; } = 10f;

    public float CurrentHealth { get; set; }
    public float CurrentMana { get; set; }

    // Compatibility alias in case older code expects CurrentVitals on this component.
    public WizardVitals CurrentVitals => this;

    protected override void OnStart()
    {
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
    }

    protected override void OnUpdate()
    {
        CurrentMana += ManaRegenPerSecond * Time.Delta;

        if ( CurrentMana > MaxMana )
            CurrentMana = MaxMana;
    }

    public bool TrySpendMana( float amount )
    {
        if ( amount <= 0f )
            return true;

        if ( CurrentMana < amount )
            return false;

        CurrentMana -= amount;
        return true;
    }

    public void RestoreMana( float amount )
    {
        if ( amount <= 0f )
            return;

        CurrentMana += amount;

        if ( CurrentMana > MaxMana )
            CurrentMana = MaxMana;
    }

    public void TakeDamage( float amount )
    {
        if ( amount <= 0f )
            return;

        CurrentHealth -= amount;

        if ( CurrentHealth < 0f )
            CurrentHealth = 0f;
    }

    public void Heal( float amount )
    {
        if ( amount <= 0f )
            return;

        CurrentHealth += amount;

        if ( CurrentHealth > MaxHealth )
            CurrentHealth = MaxHealth;
    }
}
