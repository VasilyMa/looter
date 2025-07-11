using Client;
using Leopotam.EcsLite;

public class ClientRunHandler : EcsRunHandler
{
    public ClientRunHandler()
    {
        _systems
            .Add(new RunRequestWrapperSystem<NetworkHealthUpdateEvent>())
            .Add(new RunHealthUpdateSystem())
            
            ;
            
    }

    public override EcsRunHandler Clone()
    {
        return new ClientRunHandler ();
    }
}
