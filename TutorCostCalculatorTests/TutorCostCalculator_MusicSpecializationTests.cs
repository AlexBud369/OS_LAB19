using Moq;
using TutorCostCalcLib.Services;
using TutorCostCalcLib.Enums;
using TutorCostCalcLib.Interfaces;
using TutorCostCalcLib.Models;

namespace TutorCostCalculatorTests;

public class TutorCostCalculator_MusicSpecializationTests
{
    private readonly Mock<IMaterialsFeeService> _materialsMock;
    private readonly Mock<IMusicSpecializationChecker> _musicMock;
    private readonly TutorCostCalculator _sut;

    public TutorCostCalculator_MusicSpecializationTests()
    {
        _materialsMock = new Mock<IMaterialsFeeService>();
        _musicMock = new Mock<IMusicSpecializationChecker>();
        _sut = new TutorCostCalculator(_materialsMock.Object, _musicMock.Object);
    }

    [Theory]
    [InlineData("Junior")]
    [InlineData("Experienced")]
    public void CalculatePackageCost_MusicOlympiadNotExpert_ReturnsError(string qualification)
    {
        // Arrange
        var request = new TutoringRequest("Music", qualification, DifficultyLevel.Olympiad,
            60, LessonFormat.Individual, false, 5);

        // Act
        var quote = _sut.CalculatePackageCost(request);

        // Assert
        Assert.Contains("Для предмета Music и уровня Olympiad допускается только квалификация Expert.", quote.Errors);
        Assert.Equal(0, quote.CostPerLesson);
    }

    [Fact]
    public void CalculatePackageCost_MusicOlympiadExpert_Valid()
    {
        // Arrange
        _musicMock.Setup(m => m.HasRequiredSpecialization(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns(true);
        var request = new TutoringRequest("Music", "Expert", DifficultyLevel.Olympiad,
            60, LessonFormat.Individual, false, 5);

        // Act
        var quote = _sut.CalculatePackageCost(request);

        // Assert
        Assert.Empty(quote.Errors);
        // Проверака что расчет прошел 
        Assert.True(quote.CostPerLesson > 0);
    }

    [Fact]
    public void CalculatePackageCost_MusicExpert_CallsSpecializationChecker()
    {
        // Arrange
        _musicMock.Setup(m => m.HasRequiredSpecialization(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns(true);
        var request = new TutoringRequest("Music", "Expert", DifficultyLevel.School,
            60, LessonFormat.Individual, false, 5);

        // Act
        _sut.CalculatePackageCost(request);

        // Assert
        _musicMock.Verify(m => m.HasRequiredSpecialization(It.IsAny<string>(), "Piano"), Times.Once);
    }

    [Fact]
    public void CalculatePackageCost_MusicExpertNoSpecialization_ReturnsError()
    {
        // Arrange
        _musicMock.Setup(m => m.HasRequiredSpecialization(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns(false);
        var request = new TutoringRequest("Music", "Expert", DifficultyLevel.School,
            60, LessonFormat.Individual, false, 5);

        // Act
        var quote = _sut.CalculatePackageCost(request);

        // Assert
        Assert.Contains("Репетитор Expert по Music не имеет необходимой специализации.", quote.Errors);
        Assert.Equal(0, quote.CostPerLesson);
    }
}
