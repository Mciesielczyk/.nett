namespace lab4;

public class Reader
{
    private int id;
    private string name;
    private string email;
    private List<int> ksiazki = new List<int>();

    public string GetName()
    {
        return name;
    }

    public void AddBook(int id)
    {
        ksiazki.Add(id);
    }

    public void ReturnBook(int id)
    {
        ksiazki.Remove(id);
    }
    public Reader(int id, string name, string email, List<int> ksiazki = null)
    {
        this.id = this.id;
        this.name = name;
        this.email = email;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Id: {this.id}, Name: {this.name}, Email: {this.email}");
        foreach (var ksiazka in ksiazki)
        {
            Console.WriteLine($"{ksiazka}");
        }
    }
}