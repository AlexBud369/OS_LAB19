using TutorCostCalc.Interfaces;

namespace TestCostCalc.Stubs;

public class FixedMaterialsFeeService : IMaterialsFeeService
{
    private readonly decimal _fixedFee;

    public FixedMaterialsFeeService(decimal fixedFee = 200)
    {
        _fixedFee = fixedFee;
    }

    public decimal GetCustomMaterialsFee(string subject, string difficulty) => _fixedFee;
}
