using Client;
using Leopotam.EcsLite.ExtendedSystems;
using Statement;

public class ClientRunHandler : EcsRunHandler
{
    public ClientRunHandler(BattleState state) : base(state)
    {
        _requestSystems
            .Add(new RunRequestWrapperSystem<NetworkHealthUpdateEvent>())
            .Add(new RunRequestWrapperSystem<NetworkConfirmDamageEvent>())

            ;

        _receiveSystems
            .Add(new RunConfirmDamageUpdateSystem())
            .Add(new RunHealthUpdateSystem())

            .DelHere<ConfirmDamageUpdateEvent>()
            .DelHere<HealthUpdateEvent>()
            ;


        /*_syncUpdateSystems
            ;*/
    }

    public override EcsRunHandler Clone(BattleState state)
    {
        return new ClientRunHandler(state);
    }
}
