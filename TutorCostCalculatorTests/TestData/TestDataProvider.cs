using TutorCostCalcLib.Enums;

namespace TutorCostCalculatorTests.TestData;

public static class TestDataProvider
{
    public static IEnumerable<object[]> GetCalculationTestData()
    {
        // Тест 1: Math, Experienced, School, 60, Individual, false, 5
        yield return new object[]
        {
                "Math", "Experienced", DifficultyLevel.School, 60,
                LessonFormat.Individual, false, 5,
                800m,
                800m * 5 * 0.95m
        };

        // Тест 2: Physics, Junior, University, 90, Pair, false, 10
        decimal baseRate2 = 900m * 0.8m * 1.3m;
        decimal lessonCost2 = baseRate2 * (90m / 60m) * 0.7m;
        yield return new object[]
        {
                "Physics", "Junior", DifficultyLevel.University, 90,
                LessonFormat.Pair, false, 10,
                lessonCost2,
                lessonCost2 * 10 * 0.95m
        };

        // Тест 3: English, Expert, Exam, 120, MiniGroup, true, 1
        decimal baseRate3 = 700m * 1.8m * 1.5m;
        decimal lessonCost3 = baseRate3 * (120m / 60m) * 0.5m + 200m;
        yield return new object[]
        {
                "English", "Expert", DifficultyLevel.Exam, 120,
                LessonFormat.MiniGroup, true, 1,
                lessonCost3,
                lessonCost3
        };

        // Тест 4: Programming, Experienced, Olympiad, 45, Individual, false, 30
        decimal baseRate4 = 1000m * 1.0m * 1.7m;
        decimal lessonCost4 = baseRate4 * (45m / 60m);
        yield return new object[]
        {
                "Programming", "Experienced", DifficultyLevel.Olympiad, 45,
                LessonFormat.Individual, false, 30,
                lessonCost4,
                lessonCost4 * 30 * 0.90m
        };

        // Тест 5: Music, Expert, Olympiad, 180, Individual, false, 40
        decimal baseRate5 = 1200m * 1.8m * 1.7m;
        decimal lessonCost5 = baseRate5 * (180m / 60m);
        yield return new object[]
        {
                "Music", "Expert", DifficultyLevel.Olympiad, 180,
                LessonFormat.Individual, false, 40,
                lessonCost5,
                lessonCost5 * 40 * 0.85m
        };

        // Тест 6: Music, Expert, School, 60, Individual, false, 5
        decimal baseRate6 = 1200m * 1.8m * 1.0m;
        decimal lessonCost6 = baseRate6;
        yield return new object[]
        {
                "Music", "Expert", DifficultyLevel.School, 60,
                LessonFormat.Individual, false, 5,
                lessonCost6,
                lessonCost6 * 5 * 0.95m
        };
    }
}