using lab3;
using NUnit.Framework;
namespace Testy;

public class ScientificCalculatorTests
{
    private Calculator calculator;
    private ScientificCalculator calculatorScientific;

    [SetUp]
    public void Setup()
    {
        calculator = new Calculator();
        calculatorScientific = new ScientificCalculator();

    }

    [Test]
    public void TestPierwiastek()
    {
        Assert.AreEqual(2,calculatorScientific.pierwiastek(4),"test pierwiastek");
    }
    [Test]
    public void TestAvg()
    {
        double[] tab= new double[] { 1, 2, 3, 4 };
        Assert.AreEqual(2.5, calculatorScientific.avg(tab), "test avg");
    }
    [Test]
    public void TestPot()
    {
        Assert.AreEqual(16,calculatorScientific.potega(2,4),"test potega");
    }
    [Test]
    public void TestSum()
    {
        double[] tab= new double[] { 1, 2, 3, 4 };
        Assert.AreEqual(10,calculatorScientific.suma(tab),"test suma");
    }
    [Test]
    public void TestLog()
    {
        double[] tab= new double[] { 1, 2, 3, 4 };
        Assert.AreEqual(1,calculatorScientific.log10(10),"test log10");
    }
}