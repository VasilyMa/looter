using Client;
using Statement;
using UnityEngine;
public class BattlePanel : SourcePanel
{
    [SerializeField] FloatingJoystick joystick;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

        if (joystick)
        {
            var world = BattleState.Instance.EcsHandler.World;

            var inputEntity = world.NewEntity();

            ref var inputComp = ref world.GetPool<InputComponent>().Add(inputEntity);
            inputComp.Joystick = joystick;
        }
    }
}
