using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    public struct ActionComponent : IComponent
    {
        [HideInInspector] public int CurrentActionEntity;

        public void AddComponent(EcsWorld world, int entity)
        {
            world.GetPool<ActionComponent>().Add(entity);
        }
    }
}