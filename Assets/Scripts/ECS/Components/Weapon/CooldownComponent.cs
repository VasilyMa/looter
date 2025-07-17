using Leopotam.EcsLite;

namespace Client 
{
    struct CooldownComponent : IWeapon
    {
        public float Tick;
        [UnityEngine.HideInInspector] public float CurrentTime;

        public void InitWeapon(EcsWorld world, int entity, string ownerEntity)
        {
            ref var cooldownComp = ref world.GetPool<CooldownComponent>().Add(entity);
            cooldownComp.Tick = Tick;
            cooldownComp.CurrentTime = CurrentTime;
        }
    }
}