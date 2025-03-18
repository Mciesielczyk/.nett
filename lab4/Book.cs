namespace lab4;

public class Book
{
    private int Id;
    public string Title;
    public string Author;
    private bool IsAvaialble;

    public int getId()
    {
        return Id;
    }

public Book(int id, string title, string author, bool isAvaialble)
    {
        Id = id;
        Title = title;
        Author = author;
        IsAvaialble = isAvaialble;
        
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Id: {Id}, Title: {Title}, Author: {Author}");
    }

    public void changeAvaialble()
    {
        IsAvaialble = !IsAvaialble;
    }

    public bool isAvaialble()
    {
        return IsAvaialble;
    }
}