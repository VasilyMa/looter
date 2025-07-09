using Client;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
public class MainEcsHandler : EcsRunHandler
{ 
    public MainEcsHandler() : base()
    {
        _systems
            .Add(new InitPlayerSystem())

            .Add(new RunNetworkPlayerSpawnSystem());
    }
    public override EcsRunHandler Clone()
    {
        return new MainEcsHandler();
    }
    public override void Init() => _systems?.Init(); 
    public override void Run() => _systems?.Run(); 
    public override void FixedRun() => _systems?.Run();
    public override void Dispose()
    { 
        _systems.Destroy();
        World.Destroy();
        World = null;
    }
} 