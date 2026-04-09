using Sandbox;

namespace WizardGame;

/// <summary>
/// Small tuning bridge that works alongside the built-in PlayerController.
/// Use the PlayerController inspector for most movement values.
/// </summary>
public sealed class WizardMovementController : Component, PlayerController.IEvents
{
    [Property] public PlayerController PlayerController { get; set; }
    [Property] public float LookSensitivityMultiplier { get; set; } = 1.0f;
    [Property] public bool LogJumpEvents { get; set; } = false;
    [Property] public bool LogLandEvents { get; set; } = false;

    protected override void OnStart()
    {
        PlayerController ??= Components.Get<PlayerController>( FindMode.EverythingInSelf );
    }

    void PlayerController.IEvents.OnEyeAngles( ref Angles angles )
    {
        angles.pitch *= LookSensitivityMultiplier;
        angles.yaw *= LookSensitivityMultiplier;
    }

    void PlayerController.IEvents.OnJumped()
    {
        if ( LogJumpEvents )
            Log.Info( $"{GameObject.Name} jumped" );
    }

    void PlayerController.IEvents.OnLanded( float distance, Vector3 impactVelocity )
    {
        if ( LogLandEvents )
            Log.Info( $"{GameObject.Name} landed after {distance:0.0} units" );
    }
}
