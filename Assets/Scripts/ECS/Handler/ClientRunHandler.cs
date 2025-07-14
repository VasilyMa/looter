using Client;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;

public class ClientRunHandler : EcsRunHandler
{
    public ClientRunHandler()
    {
        _requestSystems
            .Add(new RunRequestWrapperSystem<NetworkHealthUpdateEvent>())

            ;
        _syncUpdateSystems
            .Add(new RunHealthUpdateSystem())

            .DelHere<HealthUpdateEvent>()
            ;
    }

    public override EcsRunHandler Clone()
    {
        return new ClientRunHandler ();
    }
}
