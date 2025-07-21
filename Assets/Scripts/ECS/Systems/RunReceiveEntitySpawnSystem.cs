using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;

namespace Client 
{
    sealed class RunReceiveEntitySpawnSystem : IEcsRunSystem 
    {
        readonly EcsSharedInject<BattleState> _state;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ReceiveSpawnEvent>> _filter = default;
        readonly EcsPoolInject<ReceiveSpawnEvent> _receiveSpawnPool = default;
        readonly EcsPoolInject<NetworkEntityComponent> _networkEntityPool = default;
        readonly EcsPoolInject<OwnComponent> _ownPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<CameraSwitchEvent> _camerPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<LerpTransformComponent> _lerpTransformPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var e in _filter.Value)
            {
                ref var receiveSpawnComp = ref _receiveSpawnPool.Value.Get(e);

                var unitEntityConfig = ConfigModule.GetConfig<EntityConfig>();

                if (unitEntityConfig.TryGetEntity(receiveSpawnComp.SpawnKeyID, out EntityBase entityBase))
                {
                    var entity = _world.Value.NewEntity();

                    entityBase.InitEntity(_world.Value, entity, receiveSpawnComp.EntityKey);

                    ref var networkComp = ref _networkEntityPool.Value.Add(entity);
                    networkComp.EntityKey = receiveSpawnComp.EntityKey;
                    networkComp.PlayerOwner = receiveSpawnComp.PlayerOwner;

                    ref var viewComp = ref _viewPool.Value.Get(entity);
                    viewComp.RefObject.name = networkComp.EntityKey;

                    ref var transformComp = ref _transformPool.Value.Get(entity);
                    transformComp.Transform.position = receiveSpawnComp.SpawnPos;

                    if (PhotonRunHandler.Instance.Runner.LocalPlayer.PlayerId == receiveSpawnComp.PlayerOwner)
                    {
                        _ownPool.Value.Add(entity);

                        _state.Value.AddEntity("player", receiveSpawnComp.EntityKey, entity); 

                        if (_state.Value.TryGetEntity("camera", out int cameraEntity))
                        { 
                            _camerPool.Value.Add(cameraEntity).Target = transformComp.Transform;
                        }
                    }
                    else
                    {
                        _lerpTransformPool.Value.Add(entity);

                        _state.Value.AddEntity(receiveSpawnComp.EntityKey, entity);
                    }
                }
            }
        }
    }
}