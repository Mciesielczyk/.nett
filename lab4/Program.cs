// See https://aka.ms/new-console-template for more information

using lab4;

Book ksiazka1 = new Book(1, "aa", " dsa", true);
Book ksiazka2 = new Book(12, "ae", " dsa", true);
Book ksiazka3 = new Book(33, "a3", " sda", true);
EBoook eBoook1 = new EBoook(12, "tadeusz", "marian", true, "PDF");
//ksiazka1.DisplayInfo();
eBoook1.DisplayInfo();
//Console.WriteLine(ksiazka1.Author);
Reader reader = new Reader(1,"Ada", "mail@mail.com");
//EBook ksiazka1 = new EBook(1,"Ag","QQQ",true);
//ksiazka1.DisplayInfo();


Library library = new Library();

library.addBook(ksiazka1);
library.addBook(ksiazka2);
library.addBook(ksiazka3);

library.addReader(reader);
try
{
    library.BorrowBook(1, "Ada");
    library.BorrowBook(1, "Ada");
}
catch(Exception e){Console.WriteLine(e.Message);}

Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaa");
library.ListAvailableBooks();
Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaa");

reader.DisplayInfo();
Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaa");

library.ReturnBook(1);
library.ReturnBook(1);
reader.ReturnBook(1);
Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaa");

reader.DisplayInfo();
