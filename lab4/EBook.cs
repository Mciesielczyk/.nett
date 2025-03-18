namespace lab4;

public class EBoook : Book
{
    public string FileFormat;

    public EBoook(int id, string title, string author, bool isAvailable, string fileFormat)
        : base(id, title, author, isAvailable) 
    {
        FileFormat = fileFormat;
    }

    public override void DisplayInfo()
    {

        base.DisplayInfo();
        Console.WriteLine(FileFormat);
    }
    
    
}