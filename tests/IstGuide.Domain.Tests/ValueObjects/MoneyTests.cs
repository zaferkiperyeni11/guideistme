using IstGuide.Domain.ValueObjects;

namespace IstGuide.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Should_Create_Money_With_Valid_Data()
    {
        var money = new Money(150m, "TRY");

        Assert.Equal(150m, money.Amount);
        Assert.Equal("TRY", money.Currency);
    }

    [Fact]
    public void Should_Be_Equal_When_Same_Amount_And_Currency()
    {
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "USD");

        Assert.Equal(money1, money2);
    }

    [Fact]
    public void Should_Not_Be_Equal_When_Different_Amount()
    {
        var money1 = new Money(100m, "USD");
        var money2 = new Money(200m, "USD");

        Assert.NotEqual(money1, money2);
    }
}
