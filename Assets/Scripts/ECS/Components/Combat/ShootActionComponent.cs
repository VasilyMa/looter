using UnityEngine;

namespace Client 
{
    struct ShootActionComponent : IPreRequestAction, IRequestAction, IResolveAction
    {

        public void PreRequest()
        {
            Debug.Log("Pre request shoot");
        }

        public void Request()
        {
            Debug.Log("Request shoot");
        }

        public void Resolve()
        {
            Debug.Log("Resolve shoot");
        }
    }
}