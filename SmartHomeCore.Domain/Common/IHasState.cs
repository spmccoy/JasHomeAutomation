namespace SmartHomeCore.Domain.Common;

public interface IHasState<T>
{
    T CurrentState { get; }
    void ChangeState(T newState);
}
