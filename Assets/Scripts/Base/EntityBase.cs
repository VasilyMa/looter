using System.Collections.Generic;

using Client;

using Leopotam.EcsLite;

using UnityEngine;

[CreateAssetMenu(fileName = "NewEntity", menuName = "Entities/NewEntity")]
public class EntityBase : ScriptableObject, ISerializationCallbackReceiver
{
    public string KEY_ID;
    [Header("Main attributes entity")]
    [SerializeReference] public List<IComponent> Components;

    [Space(5f)]
    [Header("Combat stats of entity")]
    [SerializeReference] public List<IStat> Stats;

    [Space(5f)]
    [Header("Weapon")]
    public WeaponBase Weapon;


    public void InitEntity(EcsWorld world, int entity, string netEntityKey)
    {
        foreach (var component in Components)
        {
            component.AddComponent(world, entity);
        }

        foreach (var stat in Stats)
        {
            stat.Init(world, entity);
        }

        if (Weapon)
        {
            int weaponEntity = world.NewEntity();

            Weapon.Init(world, weaponEntity, netEntityKey);

            ref var weaponComp = ref world.GetPool<WeaponComponent>().Get(weaponEntity);
            weaponComp.Index = 0;
            ref var holderWeaponComp = ref world.GetPool<HolderWeaponComponent>().Add(entity);
            holderWeaponComp.Weapons = new List<WeaponData>();
            holderWeaponComp.Weapons.Add(new WeaponData()
            {
                WeaponEntity = weaponEntity,
            });
            ref var actionComp = ref world.GetPool<ActionComponent>().Get(entity);
            actionComp.CurrentActionEntity = weaponEntity;
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
