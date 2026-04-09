using Sandbox;

namespace WizardGame;

/// <summary>
/// Handles cast input and delegates to equipped abilities.
/// First pass only wires the primary fire spell to Attack1.
/// </summary>
public sealed class WizardAbilitySystem : Component
{
    [Property] public WizardPlayer WizardPlayer { get; set; }
    [Property] public WizardVitals Vitals { get; set; }

    [Property, Group( "Input" )] public string PrimaryCastAction { get; set; } = "Attack1";

    [Property, Group( "Abilities" )] public WizardAbilityBase PrimaryAbility { get; set; }

    protected override void OnStart()
    {
        WizardPlayer ??= Components.Get<WizardPlayer>( FindMode.EverythingInSelf );
        Vitals ??= Components.Get<WizardVitals>( FindMode.EverythingInSelf );
    }

    protected override void OnUpdate()
    {
        if ( PrimaryAbility is null )
            return;

        if ( Input.Pressed( PrimaryCastAction ) )
        {
            PrimaryAbility.TryCast();
        }
    }
}
