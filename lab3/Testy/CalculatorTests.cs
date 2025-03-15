using lab3;
using NUnit.Framework;
namespace Testy;

public class Tests
{
    private Calculator calculator;
    [SetUp]
    public void Setup()
    {
         calculator = new Calculator();
    }

    [Test]
    public void TestDodawanie()
    {
        Assert.AreEqual(3,calculator.dodawanie(1,2),"test dodawanie");
    }
    [Test]
    public void TestOdejmowanie()
    {
        Assert.AreEqual(6.2, Math.Round(calculator.odejmowanie(8.2, 2), 10), "test odejmowanie");
    }
    [Test]
    public void TestMnozenie()
    {
        Assert.AreEqual(10,calculator.mnozenie(5,2),"test mnozenie");
    }
}