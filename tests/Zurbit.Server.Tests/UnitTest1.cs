namespace Zurbit.Server.Tests;

public class UnitTest1
{
    [Fact]
    public void DummyTest_ShouldPass()
    {
        // Arrange
        var expected = 42;

        // Act
        var actual = 42;

        // Assert
        Assert.Equal(expected, actual);
    }
}
