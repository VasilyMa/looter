using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

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

#if UNITY_EDITOR
        _systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        _systems.Inject();
    }
    public abstract EcsRunHandler Clone();
    public abstract void Init();
    public abstract void Run();
    public abstract void FixedRun();
    public abstract void Dispose();
}

public class EcsData
{

}