using Client;
using Leopotam.EcsLite.ExtendedSystems;
using Statement;

public class ServerRunHandler : EcsRunHandler
{
    public ServerRunHandler(BattleState state) : base(state)
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

    public override EcsRunHandler Clone(BattleState state)
    {
        return new ServerRunHandler(state);
    }
}
