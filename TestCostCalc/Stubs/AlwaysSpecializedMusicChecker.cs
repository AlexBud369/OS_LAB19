using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorCostCalc.Interfaces;

namespace TestCostCalc.Stubs;

public class AlwaysSpecializedMusicChecker : IMusicSpecializationChecker
{
    public bool HasRequiredSpecialization(string tutorId, string specialization) => true;
}
