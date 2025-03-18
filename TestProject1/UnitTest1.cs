using lab4;

namespace TestProject1;

public class Tests
{
    
    
    Book ksiazka1 = new Book(1, "aa", " dsa", true);
    Book ksiazka2 = new Book(12, "ae", " dsa", true);
    Book ksiazka3 = new Book(33, "a3", " sda", true);
    EBoook eBoook1 = new EBoook(12, "tadeusz", "marian", true, "PDF");
    Reader reader = new Reader(1,"Ada", "mail@mail.com");
    Reader reader1 = new Reader(2,"Aga", "maildrugi@mail.com");
    Library library = new Library();

    [SetUp]
    public void Setup()
    {
        library.addBook(ksiazka1);
        library.addBook(ksiazka2);
        library.addBook(ksiazka3);
        library.addReader(reader);
        library.BorrowBook(1, "Ada");

    }

    [Test]
    public void Test1()
    {
      //  Assert.AreEqual(library.BorrowBook(1, "Ada"),);
     
      var ex = library.BorrowBook(1, "Ada");
      Assert.IsFalse(ex);

    }
    [Test]
    public void Test2()
    {
     
        var ex= library.BorrowBook(12, "Ada");
        Assert.IsTrue(ex);
    }
    [Test]
    public void Test3()
    {
     
        var ex= library.ReturnBook(1212);
        Assert.IsFalse(ex);
    }
}