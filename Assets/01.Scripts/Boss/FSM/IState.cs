using UnityEngine;

public interface IState<T>
{
    public void Enter(T obj);
    public void Update(T obj);
    public void Exit(T obj);
}