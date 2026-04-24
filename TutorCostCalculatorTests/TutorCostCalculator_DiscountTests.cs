using Moq;
using TutorCostCalcLib.Services;
using TutorCostCalcLib.Enums;
using TutorCostCalcLib.Interfaces;
using TutorCostCalcLib.Models;

namespace TutorCostCalculatorTests;

public class TutorCostCalculator_DiscountTests
{
    private readonly TutorCostCalculator _sut;

    public TutorCostCalculator_DiscountTests()
    {
        var materialsMock = new Mock<IMaterialsFeeService>();
        var musicMock = new Mock<IMusicSpecializationChecker>();
        musicMock.Setup(m => m.HasRequiredSpecialization(It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(true);
        materialsMock.Setup(m => m.GetCustomMaterialsFee(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(0);
        _sut = new TutorCostCalculator(materialsMock.Object, musicMock.Object);
    }

    [Theory]
    [InlineData(1, 800, 800)]
    [InlineData(2, 800, 1520)]
    [InlineData(5, 800, 3800)]
    [InlineData(10, 800, 7600)]
    [InlineData(11, 800, 7920)]
    [InlineData(30, 800, 21600)]
    [InlineData(31, 800, 21080)]
    [InlineData(40, 800, 27200)]
    public void CalculatePackageCost_DiscountScenarios_ReturnsCorrectTotal(
        int numberOfLessons, decimal lessonCost, decimal expectedTotal)
    {
        // Arrange фиксированная стоимость занятия (Math, Experienced, School, 60 мин, Individual, без материалов)
        var request = new TutoringRequest("Math", "Experienced", DifficultyLevel.School,
            60, LessonFormat.Individual, false, numberOfLessons);

        // Act
        var quote = _sut.CalculatePackageCost(request);

        // Assert
        Assert.Empty(quote.Errors);
        Assert.Equal(expectedTotal, quote.TotalPackageCost);
    }
}
