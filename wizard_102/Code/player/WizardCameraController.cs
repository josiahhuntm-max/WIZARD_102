using Sandbox;

namespace WizardGame;

/// <summary>
/// Optional camera hook for the built-in PlayerController camera.
/// Keep the built-in camera enabled on PlayerController and use this for light customization.
/// </summary>
public sealed class WizardCameraController : Component, PlayerController.IEvents
{
    [Property] public PlayerController PlayerController { get; set; }
    [Property] public bool OverrideFieldOfView { get; set; } = false;
    [Property] public float FieldOfView { get; set; } = 90.0f;

    protected override void OnStart()
    {
        PlayerController ??= Components.Get<PlayerController>( FindMode.EverythingInSelf );
    }

    void PlayerController.IEvents.PostCameraSetup( CameraComponent cam )
    {
        if ( OverrideFieldOfView && cam is not null )
        {
            cam.FieldOfView = FieldOfView;
        }
    }
}
