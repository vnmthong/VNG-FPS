using VNGFPS;
using PYDFramework;
using PYDFramework.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STFoodIdle : State<Troop>
{
    public STFoodIdle(Troop agent, StateMachine stateMachine) : base(agent, stateMachine)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}