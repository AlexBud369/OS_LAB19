namespace TutorCostCalculatorLib.Services;

public class PackageDiscountCalculator
{
    public decimal GetDiscount(int numberOfLessons)
    {
        if (numberOfLessons == 1) return 1.0m;
        if (numberOfLessons >= 2 && numberOfLessons <= 10) return 0.95m;
        if (numberOfLessons >= 11 && numberOfLessons <= 30) return 0.90m;
        if (numberOfLessons >= 31 && numberOfLessons <= 40) return 0.85m;
        throw new ArgumentOutOfRangeException(nameof(numberOfLessons),
            "Количество занятий должно быть от 1 до 40.");
    }
}