using Sandbox;

namespace WizardGame;

/// <summary>
/// Keeps references to the wand object and cast point.
/// This version respects scene-assigned objects and does not replace your model at runtime.
/// </summary>
public sealed class WizardWandController : Component
{
    [Property] public GameObject WandObject { get; set; }
    [Property] public GameObject CastPoint { get; set; }
    [Property] public ModelRenderer WandRenderer { get; set; }

    protected override void OnStart()
    {
        if ( WandObject is null )
        {
            WandObject = GameObject.Children.FirstOrDefault( x => x.Name == "Wand" );
        }

        if ( CastPoint is null && WandObject is not null )
        {
            CastPoint = WandObject.Children.FirstOrDefault( x => x.Name == "CastPoint" );
        }

        if ( WandRenderer is null && WandObject is not null )
        {
            WandRenderer = WandObject.Components.Get<ModelRenderer>( FindMode.EverythingInSelfAndDescendants );
        }
    }

    public Vector3 GetCastPosition()
    {
        return CastPoint?.WorldPosition ?? WandObject?.WorldPosition ?? WorldPosition;
    }

    public Rotation GetCastRotation()
    {
        return CastPoint?.WorldRotation ?? WandObject?.WorldRotation ?? WorldRotation;
    }
}
