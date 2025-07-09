using System.Collections.Generic;

using Client;

using Leopotam.EcsLite;

using UnityEngine;

[CreateAssetMenu(fileName = "NewEntity", menuName = "Entities/NewEntity")]
public class EntityBase : ScriptableObject
{
    [SerializeReference] public List<IComponent> Components;

    public void InitEntity(EcsWorld world)
    {
        var entity = world.NewEntity();

        foreach (var component in Components)
        {
            component.AddComponent(world, entity);
        }
    }
}
