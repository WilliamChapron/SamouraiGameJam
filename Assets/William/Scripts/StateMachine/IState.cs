using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();   
    void Update();  
    void Exit();    
}

public class IdleState : IState
{
    private StateMachine stateMachine;

    public IdleState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public void Enter()
    {
        Debug.Log("Entering Idle State");
    }
    public void Update()
    {
    }
    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}