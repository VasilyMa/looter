using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    struct AmmoComponent : IWeapon
    {
        public int MaxValue;
        public int MagazineValue;
        [HideInInspector] public int CurrentValue;
        [HideInInspector] public int RemainingValue;

        public void InitWeapon(EcsWorld world, int entity, string ownerEntity)
        {
            ref var ammonComp = ref world.GetPool<AmmoComponent>().Add(entity);
            ammonComp.CurrentValue = 0;
            ammonComp.MaxValue = MaxValue;
            ammonComp.RemainingValue = 0;
            ammonComp.MagazineValue = MagazineValue;
        }
    }
}