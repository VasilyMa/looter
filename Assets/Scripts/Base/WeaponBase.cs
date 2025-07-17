using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Entities/Weapon")]
public class WeaponBase : ScriptableObject
{
    [SerializeReference] List<IWeapon> Components;

    public void Init(EcsWorld world, int entity, string entityOwner)
    {
        foreach (var component in Components)
        {
            component.InitWeapon(world, entity, entityOwner);
        }
    }
}
