using System.Collections.Generic;

namespace TutorCostCalc.Models
{
    public record TutoringQuote(
        decimal TotalPackageCost,
        decimal CostPerLesson,
        List<string> Errors
    );
}
