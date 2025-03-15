namespace lab3;

public class ScientificCalculator
{ 

    private Calculator calculator; //  ScientificCalculator zawiera instancję Calculator

    public ScientificCalculator()
    {
    calculator = new Calculator(); // Tworzymy instancję Calculator wewnątrz ScientificCalculator
    }

    public double dodawanie(double a, double b)
    {
        return calculator.dodawanie(a, b);
    }
    public double potega(double a, double b)
    {
        return double.Pow(a,b);
    }

    public double pierwiastek(double a)
    {
        if (a < 0)
        {
            return a;
        }
        return Math.Sqrt(a);
    }
    

    public double log(double a)
    {
        return Math.Log(a);
    }

    public double log10(double a)
    {
        return Math.Log10(a);
    }
    
    public double suma(double[] tab)
    {
        double sum = 0;
        foreach (var x in tab)
        {
            sum=sum+x;
        }

        return sum;
    }

    public double avg(double[] tab)
    {
        return tab.Average();
    }

    public double max(double[] tab)
    {
        return tab.Max();
    }

    public double min(double[] tab)
    {
        return tab.Min();
    }
}