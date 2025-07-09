 public class TutorEcsHandler : EcsRunHandler
{
    public TutorEcsHandler() 
    { 

    } 

    public override EcsRunHandler Clone()
    {
        return new TutorEcsHandler();
    }

    public override void Init()
    {
        _systems.Init();
    }

    public override void Run()
    {
        _systems.Run();
    }

    public override void FixedRun()
    {

    }

    public override void Dispose()
    {
        _systems.Destroy();
        World.Destroy();
        World = null;
    }

}
