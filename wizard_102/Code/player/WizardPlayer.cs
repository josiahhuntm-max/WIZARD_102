using Sandbox;

namespace WizardGame;

public sealed class WizardPlayer : Component
{
    [Property] public PlayerController PlayerController { get; set; }
    [Property] public GameObject BodyObject { get; set; }
    [Property] public SkinnedModelRenderer BodyRenderer { get; set; }
    [Property] public WizardWandController WandController { get; set; }
    [Property] public WizardVitals Vitals { get; set; }

    protected override void OnStart()
    {
        PlayerController ??= Components.Get<PlayerController>( FindMode.EverythingInSelf );
        WandController ??= Components.Get<WizardWandController>( FindMode.EverythingInSelf );
        Vitals ??= Components.Get<WizardVitals>( FindMode.EverythingInSelf );
    }

    public Vector3 GetCastPosition()
    {
        return WandController?.GetCastPosition() ?? WorldPosition;
    }

    public Rotation GetCastRotation()
    {
        return WandController?.GetCastRotation() ?? WorldRotation;
    }

    public Vector3 GetAimPoint( float distance = 5000f )
    {
        var camera = Scene.Camera;

        if ( camera is null )
        {
            var fallbackStart = PlayerController?.EyePosition ?? WorldPosition;
            var fallbackForward = PlayerController != null
                ? Rotation.From( PlayerController.EyeAngles ).Forward
                : WorldRotation.Forward;

            var fallbackTrace = Scene.Trace
                .Ray( fallbackStart, fallbackStart + fallbackForward * distance )
                .IgnoreGameObjectHierarchy( GameObject )
                .Run();

            return fallbackTrace.Hit
                ? fallbackTrace.EndPosition
                : fallbackStart + fallbackForward * distance;
        }

        var start = camera.WorldPosition;
        var forward = camera.WorldRotation.Forward;

        var tr = Scene.Trace
            .Ray( start, start + forward * distance )
            .IgnoreGameObjectHierarchy( GameObject )
            .Run();

        return tr.Hit ? tr.EndPosition : start + forward * distance;
    }

    public Rotation GetAimRotationFrom( Vector3 origin )
    {
        var aimPoint = GetAimPoint();
        var dir = (aimPoint - origin).Normal;

        if ( dir.Length < 0.001f )
            dir = GetCastRotation().Forward;

        return Rotation.LookAt( dir, Vector3.Up );
    }
}
