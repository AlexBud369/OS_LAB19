
namespace TutorCostCalculatorLib.Interfaces;

public interface IMaterialsFeeService
{
    decimal GetCustomMaterialsFee(string subject, string difficulty);
}
