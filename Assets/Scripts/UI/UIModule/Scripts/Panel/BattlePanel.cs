using Client;
using UnityEngine;
using Leopotam.EcsLite;
using Statement;
using System;

public class BattlePanel : SourcePanel
{
    [SerializeField] FloatingJoystick movementJoystick; 
    [SerializeField] FloatingJoystick aimJoystick;

    EcsWorld world;
    EcsPool<InputMovementComponent> _movePool = default;
    EcsPool<InputAimComponent> _aimPool = default;
    EcsPool<DisposeInputAimEvent> _aimDisposePool = default;
    EcsPool<DisposeInputMovementEvent> _moveDisposePool = default;
    EcsPool<DisposeInputActionEvent> _actionDisposePool = default;
     
    public override void OnOpen(params Action[] onComplete)
    {
        base.OnOpen(onComplete);

        world = BattleState.Instance.EcsHandler.World;

        if (!BattleState.Instance.TryGetEntity("input", out int entity))
        {
            _movePool = world.GetPool<InputMovementComponent>();
            _aimPool = world.GetPool<InputAimComponent>();
            _aimDisposePool = world.GetPool<DisposeInputAimEvent>();
            _moveDisposePool = world.GetPool<DisposeInputMovementEvent>();
            _actionDisposePool = world.GetPool<DisposeInputActionEvent>();

            var inputEntity = world.NewEntity();

            world.GetPool<InputComponent>().Add(inputEntity);

            BattleState.Instance.AddEntity("input", inputEntity);
                
            movementJoystick.OnJoystickDown += OnInputDown;
            movementJoystick.OnJoystickUp += OnInputUp;
            aimJoystick.OnJoystickDown += OnInputDown;
            aimJoystick.OnJoystickUp += OnInputUp; 
        } 
    }

    public override void OnDipose()
    {
        base.OnDipose();

        movementJoystick.OnJoystickDown -= OnInputDown;
        movementJoystick.OnJoystickUp -= OnInputUp;
        aimJoystick.OnJoystickDown -= OnInputDown;
        aimJoystick.OnJoystickUp -= OnInputUp;
    }

    void OnInputDown(InputType type)
    {
        var filter = world.Filter<InputComponent>().End();

        foreach (var inputEntity in filter)
        {
            switch (type)
            {
                case InputType.move:
                    if (!_movePool.Has(inputEntity)) _movePool.Add(inputEntity).MovementJoystick = movementJoystick;
                    break;
                case InputType.aim:
                    if (!_aimPool.Has(inputEntity)) _aimPool.Add(inputEntity).AimJoystick = aimJoystick; 
                    break;
            }
        }
    }

    void OnInputUp(InputType type)
    {
        var filter = world.Filter<InputComponent>().End();

        foreach (var inputEntity in filter)
        {
            switch (type)
            {
                case InputType.move:
                    _moveDisposePool.Add(inputEntity);
                    break;
                case InputType.aim:
                    _aimDisposePool.Add(inputEntity);
                    _actionDisposePool.Add(inputEntity);
                    break;
            }
        }
    }
}
 
public enum InputType { move, aim }