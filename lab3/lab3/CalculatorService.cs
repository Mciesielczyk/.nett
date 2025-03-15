using System.Globalization;
using System.Linq.Expressions;

namespace lab3;

public class CalculatorService
{

    public static void wypisz()
    {
        Calculator calculator = new Calculator();
        ScientificCalculator calculator2 = new ScientificCalculator();
        while (true)
        {
            try
            {
                Console.WriteLine("Wybierz operację: +, -, *, /, ^, sqrt, log, log10, avg, min, max, sum, end\n");
                string dzialanie = Console.ReadLine();
                double result = 0;
                switch (dzialanie)
                {
                    case "+":
                        Console.WriteLine("Podaj dwie liczby");
                        string inputString = Console.ReadLine();
                        string input0String = Console.ReadLine();
                        double input0 = Convert.ToDouble(inputString);
                        double input00 = Convert.ToDouble(input0String);
                        result = calculator.dodawanie(input0, input00);
                        Console.WriteLine(result);
                        break;

                    case "-":
                        Console.WriteLine("Podaj dwie liczby");
                        string input1String = Console.ReadLine();
                        string input2String = Console.ReadLine();
                        double input1 = Convert.ToDouble(input1String);
                        double input2 = Convert.ToDouble(input2String);
                        result = calculator.odejmowanie(input1, input2);
                        Console.WriteLine(result);
                        break;

                    case "*":
                        Console.WriteLine("Podaj dwie liczby");
                        string inputtString = Console.ReadLine();
                        string inputt0String = Console.ReadLine();
                        double inputt = Convert.ToDouble(inputtString);
                        double inputt0 = Convert.ToDouble(inputt0String);
                        result = calculator.mnozenie(inputt, inputt0);
                        Console.WriteLine(result);
                        break;

                    case "/":
                        Console.WriteLine("Podaj dwie liczby");
                        string input3String = Console.ReadLine();
                        string input4String = Console.ReadLine();
                        double input4 = Convert.ToDouble(input3String);
                        double input5 = Convert.ToDouble(input4String);
                        if (input5 == 0)
                        {
                            Console.WriteLine("Nie dziel przez 0");
                        }
                        else
                            result = calculator.dzielenie(input4, input5);

                        Console.WriteLine(result);
                        break;

                    case "^":
                        Console.WriteLine("Podaj dwie liczby");
                        string input5String = Console.ReadLine();
                        string input6String = Console.ReadLine();
                        double input6 = Convert.ToDouble(input5String);
                        double input7 = Convert.ToDouble(input6String);
                        result = calculator2.potega(input6, input7);
                        Console.WriteLine(result);
                        break;
                    case "sqrt":
                        Console.WriteLine("Podaj liczbe");
                        string input7String = Console.ReadLine();
                        double input8 = Convert.ToDouble(input7String);
                        result = calculator2.pierwiastek(input8);
                        Console.WriteLine(result);
                        break;
                    case "log":
                        Console.WriteLine("Podaj liczbe");
                        string input8String = Console.ReadLine();
                        double input9 = Convert.ToDouble(input8String);
                        result = calculator2.log(input9);
                        Console.WriteLine(result);
                        break;
                    case "log10":
                        Console.WriteLine("Podaj liczbe");
                        string input9String = Console.ReadLine();
                        double input11 = Convert.ToDouble(input9String);
                        result = calculator2.log10(input11);
                        Console.WriteLine(result);
                        break;
                    case "sum":
                        Console.WriteLine("Podaj liczby do sumowania oddzielone spacja: ");
                        string input = Console.ReadLine();
                        string[] inputArray = input.Split(" ");
                        double[] numbers = new double[inputArray.Length];
                        for (int i = 0; i < inputArray.Length; i++)
                        {
                            numbers[i] = double.Parse(inputArray[i]);
                        }

                        result = calculator2.suma(numbers);
                        Console.WriteLine(result);
                        break;

                    case "avg":
                        Console.WriteLine("Podaj liczby oddzielone spacja: ");
                        string inputr = Console.ReadLine();
                        string[] inputArray1 = inputr.Split(" ");
                        double[] numbers1 = new double[inputArray1.Length];
                        for (int i = 0; i < inputArray1.Length; i++)
                        {
                            numbers1[i] = double.Parse(inputArray1[i]);
                        }

                        result = calculator2.avg(numbers1);
                        Console.WriteLine(result);
                        break;

                    case "min":
                        Console.WriteLine("Podaj liczby oddzielone spacja: ");
                        string inputt2 = Console.ReadLine();
                        string[] inputArray12 = inputt2.Split(" ");
                        double[] numbers12 = new double[inputArray12.Length];
                        for (int i = 0; i < inputArray12.Length; i++)
                        {
                            numbers12[i] = double.Parse(inputArray12[i]);
                        }

                        result = calculator2.min(numbers12);
                        Console.WriteLine(result);
                        break;

                    case "max":
                        Console.WriteLine("Podaj liczby oddzielone spacja: ");
                        string inputt22 = Console.ReadLine();
                        string[] inputArray12t = inputt22.Split(" ");
                        double[] numbers12t = new double[inputArray12t.Length];
                        for (int i = 0; i < inputArray12t.Length; i++)
                        {
                            numbers12t[i] = double.Parse(inputArray12t[i]);
                        }

                        result = calculator2.max(numbers12t);
                        Console.WriteLine(result);
                        break;
                    case "end":
                        return;
                    default:
                        Console.WriteLine("Podaj odpowiednie dane");
                        break;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("blad");
            }
        }
    }

}