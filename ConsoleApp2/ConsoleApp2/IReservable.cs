namespace ConsoleApp2;

public interface IReservable
{
    void Reserve(string customer);
    void CancelReserve();
    bool IsAvailable();
}