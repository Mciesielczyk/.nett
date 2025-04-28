using Xunit;

public class MyFirstTest
{
    [Fact]
    public void SampleTest()
    {
        Assert.Equal(4, 2 + 2);
    }
}