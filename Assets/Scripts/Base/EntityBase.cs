using System.Collections.Generic;

using Client;

using Leopotam.EcsLite;

using UnityEngine;

[CreateAssetMenu(fileName = "NewEntity", menuName = "Entities/NewEntity")]
public class EntityBase : ScriptableObject, ISerializationCallbackReceiver
{
    public string KEY_ID;
    [SerializeReference] public List<IComponent> Components;

    public void InitEntity(EcsWorld world, int entity)
    {
        foreach (var component in Components)
        {
            component.AddComponent(world, entity);
        }
    }

    public void OnAfterDeserialize()
    { 
    }

    public void OnBeforeSerialize()
    {
        KEY_ID = name;
    }
}
