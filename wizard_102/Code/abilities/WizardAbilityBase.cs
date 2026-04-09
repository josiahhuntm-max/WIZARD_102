using Sandbox;

namespace WizardGame;

/// <summary>
/// Shared base for wizard abilities.
/// Designed to be flexible with multiple calling styles while you iterate.
/// </summary>
public abstract class WizardAbilityBase : Component
{
    [Property] public string AbilityName { get; set; } = "Ability";
    [Property] public float ManaCost { get; set; } = 0f;
    [Property] public float CooldownSeconds { get; set; } = 0.25f;

    protected WizardPlayer WizardPlayer { get; private set; }
    protected WizardVitals WizardVitals { get; private set; }

    public float CooldownRemaining { get; private set; }
    public bool IsOnCooldown => CooldownRemaining > 0f;

    protected override void OnStart()
    {
        WizardPlayer ??= Components.Get<WizardPlayer>( FindMode.EverythingInSelf )
                      ?? Components.Get<WizardPlayer>( FindMode.InAncestors );

        WizardVitals ??= Components.Get<WizardVitals>( FindMode.EverythingInSelf )
                       ?? Components.Get<WizardVitals>( FindMode.InAncestors );
    }

    protected override void OnUpdate()
    {
        if ( CooldownRemaining > 0f )
        {
            CooldownRemaining -= Time.Delta;

            if ( CooldownRemaining < 0f )
                CooldownRemaining = 0f;
        }
    }

    public bool TryCast()
    {
        return PerformCast();
    }

    public bool TryUse()
    {
        return PerformCast();
    }

    public bool Cast()
    {
        return PerformCast();
    }

    protected bool PerformCast()
    {
        if ( IsOnCooldown )
            return false;

        if ( WizardPlayer is null )
            return false;

        if ( ManaCost > 0f )
        {
            if ( WizardVitals is null )
                return false;

            if ( !WizardVitals.TrySpendMana( ManaCost ) )
                return false;
        }

        if ( !OnCast() )
        {
            if ( ManaCost > 0f && WizardVitals is not null )
                WizardVitals.RestoreMana( ManaCost );

            return false;
        }

        CooldownRemaining = CooldownSeconds;
        return true;
    }

    protected abstract bool OnCast();
}
