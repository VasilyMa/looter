using Client;

public class TutorEcsHandler : EcsRunHandler
{
    public TutorEcsHandler() 
    {
        _commonSystems
            .Add(new RunRequestWrapperSystem<NetworkUnitEntitySpawnEvent>());
    } 

    public override EcsRunHandler Clone()
    {
        return new TutorEcsHandler();
    }
}
