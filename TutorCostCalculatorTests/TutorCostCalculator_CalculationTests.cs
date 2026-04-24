using Moq;
using TutorCostCalculatorLib.Services;
using TutorCostCalculatorLib.Enums;
using TutorCostCalculatorLib.Interfaces;
using TutorCostCalculatorLib.Models;
using TutorCostCalculatorTests.TestData;

namespace TutorCostCalculatorTests;

public class TutorCostCalculator_CalculationTests
{
    private readonly Mock<IMaterialsFeeService> _materialsMock;
    private readonly Mock<IMusicSpecializationChecker> _musicMock;
    private readonly TutorCostCalculator _sut;

    public TutorCostCalculator_CalculationTests()
    {
        _materialsMock = new Mock<IMaterialsFeeService>();
        _musicMock = new Mock<IMusicSpecializationChecker>();
        _musicMock.Setup(m => m.HasRequiredSpecialization(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns(true);
        _materialsMock.Setup(m => m.GetCustomMaterialsFee(It.IsAny<string>(), It.IsAny<string>()))
                      .Returns(200m);
        _sut = new TutorCostCalculator(_materialsMock.Object, _musicMock.Object);
    }

    [Theory]
    [MemberData(nameof(TestDataProvider.GetCalculationTestData), MemberType = typeof(TestDataProvider))]
    public void CalculatePackageCost_ValidRequests_ReturnsCorrectValues(
    string subject, string qualification, DifficultyLevel difficulty, int duration,
    LessonFormat format, bool needMaterials, int numberOfLessons,
    decimal expectedPerLesson, decimal expectedTotal)
    {
        // Arrange
        var request = new TutoringRequest(subject, qualification, difficulty,
            duration, format, needMaterials, numberOfLessons);

        // Act
        var quote = _sut.CalculatePackageCost(request);

        // Assert
        Assert.Empty(quote.Errors);
        Assert.Equal(expectedPerLesson, quote.CostPerLesson);
        Assert.Equal(expectedTotal, quote.TotalPackageCost);
    }
}
