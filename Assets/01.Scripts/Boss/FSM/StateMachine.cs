using Unity.VisualScripting;
using UnityEngine;

public class StateMachine<T>
{
    public IState<T> CurrentState { get; private set; }
    protected IState<T> currentState;
    private T obj;

    public StateMachine(T _obj, IState<T> state)
    {
        obj = _obj;
        currentState = state;
        currentState?.Enter(obj);
    }

    public void ChangeState(IState<T> state)
    {
        if(currentState == state)
        {
            return;
        }
        currentState.Exit(obj);
        currentState = state;
        currentState.Enter(obj);
    }

    public void Update()
    {
        currentState?.Update(obj);
    }
}
