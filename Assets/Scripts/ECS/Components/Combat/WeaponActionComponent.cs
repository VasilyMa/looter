using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    struct WeaponActionComponent : IPreRequestAction, IRequestAction, IResolveAction, IDispose
    {

        public bool PreRequest(EcsWorld world, int entity)
        {
            Debug.Log("Pre request shoot");

            return true;
        }

        public bool Request(EcsWorld world, int entity)
        {
            Debug.Log("Request shoot");

            return true;
        }

        public void Resolve(EcsWorld world, int entity)
        {
            Debug.Log("Resolve shoot");
        }

        public void Dispose(EcsWorld world, int entity)
        {
            Debug.Log("Dispose shoot");
        }
    }
}