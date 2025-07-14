using Client;
using Leopotam.EcsLite.ExtendedSystems;

public class ServerRunHandler : EcsRunHandler
{
    public ServerRunHandler()
    {
        _requestSystems
            .Add(new RunRequestWrapperSystem<NetworkDamageEffectEvent>())

            ;

        _receiveSystems
            .Add(new RunTakeDamageSystem())

            .DelHere<TakeDamageEvent>()
            ;
        _sendSystems
            .Add(new RunSendConfirmDamageSystem())
            .Add(new RunSendUpdateHealthSystem())

            .DelHere<HealthUpdateEvent>()
            .DelHere<TakeDamageConfirmEvent>()
            ;
    }

    public override EcsRunHandler Clone()
    {
        return new ServerRunHandler();
    }
}
