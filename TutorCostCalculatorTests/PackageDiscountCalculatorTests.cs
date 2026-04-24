using TutorCostCalcLib.Services;
using Xunit;

namespace TutorCostCalculatorTests;

public class PackageDiscountCalculatorTests
{
    [Fact]
    public void GetDiscount_For1Lesson_Returns1()
    {
        var calculator = new PackageDiscountCalculator();
        decimal discount = calculator.GetDiscount(1);
        Assert.Equal(1.0m, discount);
    }

    [Theory]
    [InlineData(2, 0.95)]
    [InlineData(5, 0.95)]
    [InlineData(10, 0.95)]
    [InlineData(11, 0.90)]
    [InlineData(20, 0.90)]
    [InlineData(30, 0.90)]
    [InlineData(31, 0.85)]
    [InlineData(40, 0.85)]
    public void GetDiscount_ValidRange_ReturnsCorrectDiscount(int lessons, decimal expected)
    {
        var calculator = new PackageDiscountCalculator();
        decimal discount = calculator.GetDiscount(lessons);
        Assert.Equal(expected, discount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(41)]
    [InlineData(-5)]
    public void GetDiscount_OutOfRange_ThrowsException(int lessons)
    {
        var calculator = new PackageDiscountCalculator();
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.GetDiscount(lessons));
    }
}