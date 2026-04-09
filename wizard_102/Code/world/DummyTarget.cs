using Sandbox;

namespace WizardGame;

/// <summary>
/// Simple damageable test target for spell prototyping.
/// Put this on a prop or target dummy object.
/// </summary>
public sealed class DummyTarget : Component
{
    [Property] public float MaxHealth { get; set; } = 100.0f;
    [Property] public bool ResetOnDeath { get; set; } = true;
    [Property] public float ResetDelay { get; set; } = 2.0f;
    [Property] public bool LogDamage { get; set; } = true;

    [Property, ReadOnly] public float CurrentHealth { get; set; }

    private TimeUntil ResetUntil { get; set; }

    protected override void OnStart()
    {
        CurrentHealth = MaxHealth;
    }

    protected override void OnUpdate()
    {
        if ( ResetOnDeath && CurrentHealth <= 0.0f && ResetUntil <= 0.0f )
        {
            CurrentHealth = MaxHealth;
            Log.Info( $"{GameObject.Name} reset." );
        }
    }

    public void TakeDamage( float amount )
    {
        if ( amount <= 0.0f || CurrentHealth <= 0.0f )
            return;

        CurrentHealth = (CurrentHealth - amount).Clamp( 0.0f, MaxHealth );

        if ( LogDamage )
        {
            Log.Info( $"{GameObject.Name} took {amount:0.#} damage. HP: {CurrentHealth:0.#}/{MaxHealth:0.#}" );
        }

        if ( CurrentHealth <= 0.0f )
        {
            OnDestroyed();
        }
    }

    private void OnDestroyed()
    {
        Log.Info( $"{GameObject.Name} destroyed." );

        if ( ResetOnDeath )
        {
            ResetUntil = ResetDelay;
        }
    }
}
