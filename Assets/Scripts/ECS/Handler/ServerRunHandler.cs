using Client;

public class ServerRunHandler : EcsRunHandler
{
    public ServerRunHandler()
    {
        _systems
            .Add(new RunRequestWrapperSystem<NetworkUnitEntitySpawnEvent>())
            .Add(new RunRequestWrapperSystem<NetworkDamageEffectEvent>())


            ;
    }

    public override EcsRunHandler Clone()
    {
        return new ServerRunHandler();
    }
}
