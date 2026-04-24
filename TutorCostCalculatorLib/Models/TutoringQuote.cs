using System.Collections.Generic;

namespace TutorCostCalculatorLib.Models
{
    public record TutoringQuote(
        decimal TotalPackageCost,
        decimal CostPerLesson,
        List<string> Errors
    );
}
