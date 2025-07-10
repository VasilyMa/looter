using Client;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;

public abstract class EcsRunHandler
{
    public EcsWorld World;
    protected EcsSystems _systems;
    protected EcsData _data;
    public EcsRunHandler()
    {
        World = new Leopotam.EcsLite.EcsWorld();
        _data = new EcsData();
        _systems = new EcsSystems(World, _data);
        _systems
            .Add(new RunInputSystem())
            .Add(new RunPlayerMovementSystem())
            .Add(new RunNPCMovementSystem())


            .Add(new RunRequestWrapperSystem<NetworkTransformEvent>())
            .Add(new RunReceiveTransformSystem())
            .DelHere<NetworkTransformUpdateEvent>()
            .Add(new RunLerpNetworkTransformSystem())
            ;

#if UNITY_EDITOR
        _systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
    }
    public abstract EcsRunHandler Clone();

    public virtual void Init()
    {
        _systems.Inject();
        _systems.Init();
    }

    public virtual void Run()
    {
        _systems.Run();
    }

    public virtual void FixedRun()
    {

    }

    public virtual void Dispose()
    {
        _systems.Destroy();
        World.Destroy();
        World = null;
    } 
}

public class EcsData
{

}