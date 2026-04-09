using Sandbox;

namespace WizardGame;

/// <summary>
/// Simple starter projectile for wizard spells.
/// </summary>
public sealed class WizardProjectile : Component
{
    [Property] public float Speed { get; set; } = 1200f;
    [Property] public float Damage { get; set; } = 25f;

    public GameObject Owner { get; set; }
    public Vector3 Direction { get; set; }

    public void Initialize( GameObject owner, Vector3 direction, float speed, float damage )
    {
        Owner = owner;
        Direction = direction.Normal;
        Speed = speed;
        Damage = damage;

        if ( Direction.Length < 0.001f )
            Direction = WorldRotation.Forward;

        WorldRotation = Rotation.LookAt( Direction, Vector3.Up );
    }

    protected override void OnStart()
    {
        if ( Direction.Length < 0.001f )
            Direction = WorldRotation.Forward;
    }

    protected override void OnUpdate()
    {
        var move = Direction.Normal * Speed * Time.Delta;
        var start = WorldPosition;
        var end = start + move;

        var tr = Scene.Trace
            .Ray( start, end )
            .IgnoreGameObject( GameObject )
            .IgnoreGameObjectHierarchy( Owner )
            .Run();

        if ( tr.Hit )
        {
            var target = tr.GameObject?.Components.Get<DummyTarget>( FindMode.EverythingInSelfAndDescendants );
            if ( target != null )
            {
                target.TakeDamage( Damage );
            }

            GameObject.Destroy();
            return;
        }

        WorldPosition = end;
        WorldRotation = Rotation.LookAt( Direction.Normal, Vector3.Up );
    }
}
