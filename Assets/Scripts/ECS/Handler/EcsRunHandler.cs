using Client;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Statement;
using System.Collections.Generic;

public abstract class EcsRunHandler
{
    public EcsWorld World;
    public bool IsRun;
    /// <summary>
    /// Init systems use local on all clients
    /// </summary>
    protected EcsSystems _initSystems;
    /// <summary>
    /// Common systems is update local systems
    /// </summary>
    protected EcsSystems _commonSystems;
    /// <summary>
    /// Action systems is responsobility for the actions of player, full pipeline prerequest, request, resolve
    /// </summary>
    protected EcsSystems _actionSystems;
    /// <summary>
    /// Request systems is get from network event
    /// </summary>
    protected EcsSystems _requestSystems;
    /// <summary>
    /// Send systems is events from local to other or host
    /// </summary>
    protected EcsSystems _sendSystems;
    /// <summary>
    /// Receive systems is events from request 
    /// </summary>
    protected EcsSystems _receiveSystems;
    /// <summary>
    /// Sync update systems is update local on client with data from receive systems
    /// </summary>
    protected EcsSystems _syncUpdateSystems;
    /// <summary>
    /// Dispose any dispose systems for dispose components from entity it's not destroy entity only clean are components
    /// </summary>
    protected EcsSystems _disposeSystems;
    protected EcsData _data;
    protected List<EcsSystems> _allSystems;
    public EcsRunHandler(BattleState state)
    {
        World = new Leopotam.EcsLite.EcsWorld();
        _data = new EcsData();
        
        _initSystems = new EcsSystems(World, state);
        _commonSystems = new EcsSystems(World, state);
        _actionSystems = new EcsSystems(World, state);
        _requestSystems = new EcsSystems(World, state);
        _syncUpdateSystems = new EcsSystems(World, state);
        _receiveSystems = new EcsSystems(World, state);
        _sendSystems = new EcsSystems(World, state);
        _disposeSystems = new EcsSystems(World, state);

        _initSystems
            .Add(new InitCameraSystem())
            ;

        _commonSystems
            .Add(new RunInputMovementSystem())
            .Add(new RunInputAimSystem()) 
            .Add(new RunInputActionSystem()) 

            .Add(new RunInvokeActionSystem())

            .Add(new RunDisposeInputMovementSystem())
            .Add(new RunDisposeInputAimSystem())
            .Add(new RunDisposeInputActionSystem())
            
            .Add(new RunAimDirectionSystem())
            .Add(new RunSelectTargetSystem())
            .Add(new RunAimingToTargetSystem())




            .Add(new RunPrepareShootSystem())
            .Add(new RunRequestShootSystem())
            .Add(new RunResolveShootSystem())

            .Add(new RunBroadcastStartShootSystem())
            .Add(new RunBroadcastFinishShootSystem())

            .Add(new RunPlayerMovementSystem())
            .Add(new RunNPCMovementSystem())

            .Add(new RunCameraSwitchSystem())

            .Add(new RunDisposeAimSystem())
            .Add(new RunDisposeMovementSystem())
            .Add(new RunDisposeInActionSystem()) 

            .DelHere<ResolveShootEvent>()
            .DelHere<DirectionComponent>()
            .DelHere<AimDirectionComponent>()
            .DelHere<AllowShootComponent>()
            .DelHere<WeaponShootRequestEvent>()
            .DelHere<AimComponent>()
            .DelHere<AimTargetBufferComponent>()
            .DelHere<CancelShootingEvent>()

            .DelHere<SearchTargetEvent>()
            .DelHere<DisposeAimEvent>()
            .DelHere<DisposeMovementEvent>()
            .DelHere<DisposeInputAimEvent>()
            .DelHere<DisposeInputMovementEvent>()
            .DelHere<DisposeInputActionEvent>()

            .Add(new RunWeaponCooldownSystem())
            ;

        _actionSystems
            .Add(new RunPreRequestActionSystem<WeaponActionComponent>())
            .Add(new RunRequestActionSystem<WeaponActionComponent>()) 
            .Add(new RunDisposeActionSystem<WeaponActionComponent>()) 
            .Add(new RunResolveActionSystem<WeaponActionComponent>())

            .DelHere<PreRequestActionEvent>()
            .DelHere<RequestActionEvent>()
            .DelHere<ResolveActionEvent>()
            .DelHere<DisposeActionEvent>()
            ;

        _requestSystems
            .Add(new RunRequestWrapperSystem<NetworkStartShootEvent>())
            .Add(new RunRequestWrapperSystem<NetworkFinishShootEvent>())
            .Add(new RunRequestWrapperSystem<NetworkUnitEntitySpawnEvent>())
            .Add(new RunRequestWrapperSystem<NetworkTransformEvent>())
            ;

        _receiveSystems 
            .Add(new RunReceiveEntitySpawnSystem())
            .Add(new RunReceiveTransformSystem())
            .DelHere<ReceiveSpawnEvent>()
            .DelHere<ReceiveTransformEvent>()
            ;

        _syncUpdateSystems
            .Add(new RunLerpNetworkTransformSystem())
            ;

        _sendSystems
            .Add(new RunSendStartShootingSystem())
            .Add(new RunSendFinishShootingSystem())
            .Add(new RunSendShootSystem())

            .Add(new RunSendTransformUpdateSystem())

            .DelHere<SendFinishShootingEvent>()
            .DelHere<SendStartShootingEvent>()
            .DelHere<SendShootEvent>()
            //.DelHere<SendTransformUpdateEvent>()
            ;

#if UNITY_EDITOR
        _commonSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        _allSystems = new List<EcsSystems>()
        {
            _initSystems, 
            _commonSystems,
            _actionSystems,
            _requestSystems,
            _receiveSystems,
            _syncUpdateSystems,
            _sendSystems, 
            _disposeSystems
        };
    }
    public abstract EcsRunHandler Clone(BattleState state);

    public virtual void Init()
    {
        for (int i = 0; i < _allSystems.Count; i++)
        {
            var systems = _allSystems[i];
            systems.Inject(); 
            systems.Init();
        }
    }

    public virtual void Run()
    {
        for (int i = 0; i < _allSystems.Count; i++)
        {
            _allSystems[i].Run();
        } 
    }

    public virtual void FixedRun()
    {

    }

    public virtual void Dispose()
    {
        _allSystems.ForEach(_x => _x.Destroy());
        World.Destroy();
        World = null;
    } 
}

public class EcsData
{

}