namespace lab4;

public class Library : IBookOperations
{
    private List<Book> books = new List<Book>();
    private List<Reader> readers = new List<Reader>();
    
    public bool BorrowBook(int bookId,string borrowerName )
    {
        foreach (var book in books)
        {
            if (book.getId() == bookId)
            {
                if (book.isAvaialble() == true)
                {
                    book.changeAvaialble();
                    
                    foreach (var reader in readers)
                    {
                        if (reader.GetName() == borrowerName)
                        {
                            reader.AddBook(bookId);
                            return true;
                        }
                    }

                }
                else
                {
                    //Console.WriteLine("Ta ksiazka juz jest wypozyczna. ");
                    return false;
                    throw new ArgumentException("Ta ksiazka juz jest wypozyczna. ");
                    
                }
            }
        }


        return false;

    }

   public bool ReturnBook(int bookId)
    {
        foreach (Book book in books)
        {
            if (book.getId() == bookId)
            {
                if (book.isAvaialble() == false)
                {
                    book.changeAvaialble();
                    return true;
                }
                else Console.WriteLine("Nie byla wypozyczna. ");
                return false;

            }
        }

        return false;
    }

    public void addBook(Book book)
    {
        books.Add(book);
    }

    public void addReader(Reader reader)
    {
        readers.Add(reader);
    }

    public void ListAvailableBooks()
    {
        foreach (var book in books)
        {
            if (book.isAvaialble() == true)
            {
                book.DisplayInfo();
            }
        }
    }
}