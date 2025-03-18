namespace lab4;

public interface IBookOperations
{ 
  public bool BorrowBook(int bookdId, string borrowerName);
   public bool ReturnBook(int bookId);

}