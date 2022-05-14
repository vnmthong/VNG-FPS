using VNGFPS;
using PYDFramework;
using PYDFramework.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STFoodSpawn : State<Troop>
{
    public STFoodSpawn(Troop agent, StateMachine stateMachine) : base(agent, stateMachine)
    {
    }

    public override void Enter()
    {
        RunSpawnAnimation();
    }

    private void RunSpawnAnimation()
    {
        var pos = Singleton<GameController>.instance.map.GetSlot(agent.model.SlotId, agent.model.team);
        if (!pos)
            Debug.LogError($"{agent.model.SlotId}-{agent.model.team}");

        agent.transform.position = pos.position;
        agent.transform.rotation = pos.rotation;
        agent.smFood.ChangeState(agent.states.idle);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
