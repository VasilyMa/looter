using Client;
using Leopotam.EcsLite.ExtendedSystems;

public class ServerRunHandler : EcsRunHandler
{
    public ServerRunHandler()
    {
        _initSystems
            .Add(new InitSpawnSystem())
            ;

        _commonSystems
            .Add(new RunBrainSpawnSystem())
            ;

        _requestSystems
            .Add(new RunRequestWrapperSystem<NetworkShootRequestEvent>()) 

            ;

        _receiveSystems
            .Add(new RunAllowHitSystem())
             
            .Add(new RunTakeDamageSystem())

            .DelHere<TakeDamageEvent>()
            .DelHere<HitRequest>()
            ;
        _sendSystems
            .Add(new RunSendConfirmDamageSystem())
            .Add(new RunSendUpdateHealthSystem())
            .Add(new RunSendSpawnSystem())

            .DelHere<SendSpawnEvent>()
            .DelHere<HealthUpdateEvent>()
            .DelHere<TakeDamageConfirmEvent>()
            ;
    }

    public override EcsRunHandler Clone()
    {
        return new ServerRunHandler();
    }
}
