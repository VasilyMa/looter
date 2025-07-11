using Client;
using Statement;
using System;
using UnityEngine;
public class BattlePanel : SourcePanel
{
    [SerializeField] FloatingJoystick joystick;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

    }

    public override void OnOpen(params Action[] onComplete)
    {
        if (joystick)
        {
            var world = BattleState.Instance.EcsHandler.World;

            var inputEntity = world.NewEntity();

            ref var inputComp = ref world.GetPool<InputComponent>().Add(inputEntity);
            inputComp.Joystick = joystick;

            BattleState.Instance.InputEntity = inputEntity; 
        }

        base.OnOpen(onComplete);
    }

    public override void OnCLose(params Action[] onComplete)
    {
        if (joystick)
        {
            var world = BattleState.Instance.EcsHandler.World;
             
            world.DelEntity(BattleState.Instance.InputEntity);
        }

        base.OnCLose(onComplete);
    }
}
