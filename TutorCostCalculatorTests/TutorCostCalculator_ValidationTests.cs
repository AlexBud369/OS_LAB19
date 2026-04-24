using Moq;
using TutorCostCalculatorLib.Services;
using TutorCostCalculatorLib.Enums;
using TutorCostCalculatorLib.Interfaces;
using TutorCostCalculatorLib.Models;

namespace TutorCostCalculatorTests;

public class TutorCostCalculator_ValidationTests
{
    private readonly Mock<IMaterialsFeeService> _materialsMock;
    private readonly Mock<IMusicSpecializationChecker> _musicMock;
    private readonly TutorCostCalculator _sut;

    public TutorCostCalculator_ValidationTests()
    {
        _materialsMock = new Mock<IMaterialsFeeService>();
        _musicMock = new Mock<IMusicSpecializationChecker>();
        _musicMock.Setup(m => m.HasRequiredSpecialization(It.IsAny<string>(), It.IsAny<string>()))
                  .Returns(true);
        _sut = new TutorCostCalculator(_materialsMock.Object, _musicMock.Object);
    }

    [Theory]
    [InlineData(44)]
    [InlineData(181)]
    [InlineData(-10)]
    public void CalculatePackageCost_DurationOutOfRange_ReturnsError(int duration)
    {
        var request = new TutoringRequest("Math", "Experienced", DifficultyLevel.School,
            duration, LessonFormat.Individual, false, 5);
        var quote = _sut.CalculatePackageCost(request);
        Assert.Contains("Длительность занятия должна быть от 45 до 180 минут.", quote.Errors);
        Assert.Equal(0, quote.CostPerLesson);
        Assert.Equal(0, quote.TotalPackageCost);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(41)]
    [InlineData(-5)]
    public void CalculatePackageCost_NumberOfLessonsOutOfRange_ReturnsError(int numberOfLessons)
    {
        var request = new TutoringRequest("Math", "Experienced", DifficultyLevel.School,
            60, LessonFormat.Individual, false, numberOfLessons);
        var quote = _sut.CalculatePackageCost(request);
        Assert.Contains("Количество занятий должно быть от 1 до 40.", quote.Errors);
        Assert.Equal(0, quote.CostPerLesson);
        Assert.Equal(0, quote.TotalPackageCost);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" Math")]
    [InlineData("Math ")]
    [InlineData("Ma th")]
    [InlineData("math")]
    [InlineData("Mat")]
    [InlineData("Math1")]
    [InlineData("Biology")]
    [InlineData("Математика")]
    public void CalculatePackageCost_InvalidSubject_ReturnsError(string subject)
    {
        var request = new TutoringRequest(subject, "Experienced", DifficultyLevel.School,
            60, LessonFormat.Individual, false, 5);
        var quote = _sut.CalculatePackageCost(request);
        Assert.Contains($"Недопустимый предмет: {(subject ?? "null")}.", quote.Errors);
        Assert.Equal(0, quote.CostPerLesson);
        Assert.Equal(0, quote.TotalPackageCost);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" Junior")]
    [InlineData("Junior ")]
    [InlineData("Juni or")]
    [InlineData("junior")]
    [InlineData("Juni")]
    [InlineData("Junior1")]
    [InlineData("Master")]
    [InlineData("Новичок")]
    public void CalculatePackageCost_InvalidQualification_ReturnsError(string qualification)
    {
        var request = new TutoringRequest("Math", qualification, DifficultyLevel.School,
            60, LessonFormat.Individual, false, 5);
        var quote = _sut.CalculatePackageCost(request);
        Assert.Contains($"Недопустимая квалификация: {(qualification ?? "null")}.", quote.Errors);
        Assert.Equal(0, quote.CostPerLesson);
        Assert.Equal(0, quote.TotalPackageCost);
    }

    [Fact]
    public void CalculatePackageCost_InvalidDifficulty_ReturnsError()
    {
        var request = new TutoringRequest("Math", "Experienced", (DifficultyLevel)10,
            60, LessonFormat.Individual, false, 5);
        var quote = _sut.CalculatePackageCost(request);
        Assert.Contains($"Недопустимый уровень сложности: {request.Difficulty}.", quote.Errors);
        Assert.Equal(0, quote.CostPerLesson);
        Assert.Equal(0, quote.TotalPackageCost);
    }

    [Fact]
    public void CalculatePackageCost_InvalidFormat_ReturnsError()
    {
        var request = new TutoringRequest("Math", "Experienced", DifficultyLevel.School,
            60, (LessonFormat)10, false, 5);
        var quote = _sut.CalculatePackageCost(request);
        Assert.Contains($"Недопустимый формат занятия: {request.Format}.", quote.Errors);
        Assert.Equal(0, quote.CostPerLesson);
        Assert.Equal(0, quote.TotalPackageCost);
    }
}