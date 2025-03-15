using System.Linq.Expressions;
using System.Security.Cryptography;

namespace lab3;

public class Calculator
{
    public double dodawanie(double a, double b)
    {
        return a + b;
    }
    public double odejmowanie(double a, double b) => a - b;
    public double mnozenie(double a, double b) => a * b;

    public double dzielenie(double a, double b)
    {
        if (b == 0) return b;
        else return a / b;
    }

   
    
    
            
        
    
}