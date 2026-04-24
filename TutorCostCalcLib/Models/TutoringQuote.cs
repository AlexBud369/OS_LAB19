/*using System.Collections.Generic;

namespace TutorCostCalculatorLib.Models
{
    public record TutoringQuote(
        decimal TotalPackageCost,
        decimal CostPerLesson,
        List<string> Errors
    );
}*/

using System.Collections.Generic;

namespace TutorCostCalcLib.Models
{
    public class TutoringQuote
    {
        public decimal TotalPackageCost { get; }
        public decimal CostPerLesson { get; }
        public List<string> Errors { get; }

        public TutoringQuote(decimal totalPackageCost, decimal costPerLesson, List<string> errors)
        {
            TotalPackageCost = totalPackageCost;
            CostPerLesson = costPerLesson;
            Errors = errors;
        }
    }
}
