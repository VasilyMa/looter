using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunAllowHitSystem : IEcsRunSystem 
    {
        readonly EcsFilterInject<Inc<HitRequest>> _filter = default;
        readonly EcsPoolInject<HitRequest> _hitPool = default;
        readonly EcsPoolInject<HolderWeaponComponent> _holderPool = default;
        readonly EcsPoolInject<AttackComponent> _attackPool = default;
        readonly EcsPoolInject<TakeDamageEvent> _takeDamagePool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var hitComp = ref _hitPool.Value.Get(entity);
                
                if (_holderPool.Value.Has(hitComp.SenderEntity))
                {
                    ref var holderComp = ref _holderPool.Value.Get(hitComp.SenderEntity);
                    var weaponData = holderComp.Weapons[hitComp.WeaponIndex];

                    ref var attackComp = ref _attackPool.Value.Get(weaponData.WeaponEntity);

                    if (!_takeDamagePool.Value.Has(entity))
                    {
                        _takeDamagePool.Value.Add(entity).DamageData = new System.Collections.Generic.List<TakeDamageData>();
                    }

                    ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                    takeDamageComp.DamageData.Add(new TakeDamageData() 
                    {
                        Type = DamageType.physic,
                        Value = attackComp.Value,
                        SourceEntity = hitComp.SenderEntity,
                    });
                }
            }
        }
    }
}