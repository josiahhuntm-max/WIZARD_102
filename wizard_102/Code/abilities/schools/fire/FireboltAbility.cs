using Sandbox;

namespace WizardGame;

public sealed class FireboltAbility : WizardAbilityBase
{
    [Property] public GameObject ProjectilePrefab { get; set; }
    [Property] public float ProjectileSpeed { get; set; } = 2600.0f;
    [Property] public float Damage { get; set; } = 20.0f;
    [Property] public float ManaCostOverride { get; set; } = 10.0f;
    [Property] public float FallbackSpawnDistance { get; set; } = 24.0f;

    protected override void OnStart()
    {
        base.OnStart();

        AbilityName = string.IsNullOrWhiteSpace( AbilityName ) || AbilityName == "Ability"
            ? "Firebolt"
            : AbilityName;
    }

    protected override bool OnCast()
    {
        if ( ProjectilePrefab is null || WizardPlayer is null )
            return false;

        var vitals = WizardPlayer.Components.Get<WizardVitals>( FindMode.EverythingInSelf );
        if ( vitals == null )
            return false;

        if ( vitals.CurrentMana < ManaCostOverride )
            return false;

        var spawnPosition = WizardPlayer.GetCastPosition();
        var castRotation = WizardPlayer.GetAimRotationFrom( spawnPosition );

        if ( spawnPosition == Vector3.Zero )
            spawnPosition = WizardPlayer.WorldPosition + castRotation.Forward * FallbackSpawnDistance;

        var projectileObject = ProjectilePrefab.Clone( spawnPosition, castRotation );
        var projectile = projectileObject.Components.Get<WizardProjectile>( FindMode.EverythingInSelfAndDescendants );

        if ( projectile is null )
        {
            projectileObject.Destroy();
            return false;
        }

        if ( !vitals.TrySpendMana( ManaCostOverride ) )
        {
            projectileObject.Destroy();
            return false;
        }

        projectile.Initialize( WizardPlayer.GameObject, castRotation.Forward, ProjectileSpeed, Damage );
        return true;
    }
}
