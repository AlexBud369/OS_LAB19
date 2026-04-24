using Moq;
using TutorCostCalculatorLib.Services;
using TutorCostCalculatorLib.Enums;
using TutorCostCalculatorLib.Interfaces;
using TutorCostCalculatorLib.Models;

namespace TutorCostCalculatorTests;

public class TutorCostCalculator_MaterialsTests
{
    private readonly Mock<IMaterialsFeeService> _materialsMock;
    private readonly Mock<IMusicSpecializationChecker> _musicMock;
    private readonly TutorCostCalculator _sut;

    public TutorCostCalculator_MaterialsTests()
    {
        _materialsMock = new Mock<IMaterialsFeeService>();
        _musicMock = new Mock<IMusicSpecializationChecker>();
        _musicMock.Setup(m => m.HasRequiredSpecialization(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns(true);
        _sut = new TutorCostCalculator(_materialsMock.Object, _musicMock.Object);
    }

    [Fact]
    public void CalculatePackageCost_NeedMaterials_CallsMaterialsServiceAndAddsFee()
    {
        // Arrange
        _materialsMock.Setup(m => m.GetCustomMaterialsFee("Math", "School")).Returns(200m);
        var request = new TutoringRequest("Math", "Experienced", DifficultyLevel.School,
            60, LessonFormat.Individual, true, 5);

        // Act
        var quote = _sut.CalculatePackageCost(request);

        // Assert
        _materialsMock.Verify(m => m.GetCustomMaterialsFee("Math", "School"), Times.Once);
        Assert.Equal(800 + 200, quote.CostPerLesson);
    }

    [Fact]
    public void CalculatePackageCost_NoMaterials_DoesNotCallMaterialsService()
    {
        // Arrange
        var request = new TutoringRequest("Math", "Experienced", DifficultyLevel.School,
            60, LessonFormat.Individual, false, 5);

        // Act
        _sut.CalculatePackageCost(request);

        // Assert
        _materialsMock.Verify(m => m.GetCustomMaterialsFee(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}