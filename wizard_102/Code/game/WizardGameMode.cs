using Sandbox;

namespace WizardGame;

/// <summary>
/// Lightweight helper for scenes using NetworkHelper.
/// Add this to the same GameObject as NetworkHelper and assign your player prefab there.
/// </summary>
public sealed class WizardGameMode : Component
{
    [Property] public NetworkHelper NetworkHelper { get; set; }
    [Property] public GameObject PlayerPrefab { get; set; }

    protected override void OnStart()
    {
        NetworkHelper ??= Components.Get<NetworkHelper>( FindMode.EverythingInSelf );

        if ( NetworkHelper is not null && PlayerPrefab is not null )
        {
            NetworkHelper.PlayerPrefab = PlayerPrefab;
        }
    }
}
