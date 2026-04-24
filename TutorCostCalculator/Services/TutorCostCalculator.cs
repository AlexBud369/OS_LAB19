using TutorCostCalculatorLib.Enums;
using TutorCostCalculatorLib.Interfaces;
using TutorCostCalculatorLib.Models;

namespace TutorCostCalculatorLib.Services;

public class TutorCostCalculator
{
    private readonly IMaterialsFeeService _materialsFeeService;
    private readonly IMusicSpecializationChecker _musicSpecializationChecker;
    private readonly PackageDiscountCalculator _discountCalculator;

    private static readonly Dictionary<string, decimal> SubjectRates = new()
    {
        ["Math"] = 800,
        ["Physics"] = 900,
        ["English"] = 700,
        ["Programming"] = 1000,
        ["Music"] = 1200
    };

    private static readonly Dictionary<string, decimal> QualificationCoeffs = new()
    {
        ["Junior"] = 0.8m,
        ["Experienced"] = 1.0m,
        ["Expert"] = 1.8m
    };

    private static readonly Dictionary<DifficultyLevel, decimal> DifficultyCoeffs = new()
    {
        [DifficultyLevel.School] = 1.0m,
        [DifficultyLevel.University] = 1.3m,
        [DifficultyLevel.Olympiad] = 1.7m,
        [DifficultyLevel.Exam] = 1.5m
    };

    private static readonly Dictionary<LessonFormat, decimal> FormatCoeffs = new()
    {
        [LessonFormat.Individual] = 1.0m,
        [LessonFormat.Pair] = 0.7m,
        [LessonFormat.MiniGroup] = 0.5m
    };

    public TutorCostCalculator(
        IMaterialsFeeService materialsFeeService,
        IMusicSpecializationChecker musicSpecializationChecker)
    {
        _materialsFeeService = materialsFeeService;
        _musicSpecializationChecker = musicSpecializationChecker;
        _discountCalculator = new PackageDiscountCalculator();
    }

    public TutoringQuote CalculatePackageCost(TutoringRequest request)
    {
        var errors = new List<string>();

        if (request.DurationMinutes < 45 || request.DurationMinutes > 180) {
            errors.Add("Длительность занятия должна быть от 45 до 180 минут.");
        }
            

        if (request.NumberOfLessons < 1 || request.NumberOfLessons > 40) {
            errors.Add("Количество занятий должно быть от 1 до 40.");
        }
           

        if (string.IsNullOrEmpty(request.Subject) || !SubjectRates.ContainsKey(request.Subject)) {
            errors.Add($"Недопустимый предмет: {request.Subject ?? "null"}.");
        }
            
        if (string.IsNullOrEmpty(request.TutorQualification) ||
            !QualificationCoeffs.ContainsKey(request.TutorQualification)) {
            errors.Add($"Недопустимая квалификация: {request.TutorQualification ?? "null"}.");
        }
           

        if (!DifficultyCoeffs.ContainsKey(request.Difficulty)) {
            errors.Add($"Недопустимый уровень сложности: {request.Difficulty}.");
        }
        
        if (!FormatCoeffs.ContainsKey(request.Format)) {
            errors.Add($"Недопустимый формат занятия: {request.Format}.");
        }
           

        if (request.Subject == "Music" && request.Difficulty == DifficultyLevel.Olympiad) {
            if (request.TutorQualification != "Expert") {
                errors.Add("Для предмета Music и уровня Olympiad допускается только квалификация Expert.");
            }   
        }

        if (request.Subject == "Music" && request.TutorQualification == "Expert") {
            string tutorId = "default-tutor-id";
            if (!_musicSpecializationChecker.HasRequiredSpecialization(tutorId, "Piano")){
                errors.Add("Репетитор Expert по Music не имеет необходимой специализации.");
            }
              
        }

        if (errors.Any()) {
            return new TutoringQuote(0, 0, errors);
        }
          

        decimal subjectRate = SubjectRates[request.Subject];
        decimal qualCoeff = QualificationCoeffs[request.TutorQualification];
        decimal diffCoeff = DifficultyCoeffs[request.Difficulty];
        decimal baseRate = subjectRate * qualCoeff * diffCoeff;

        decimal formatCoeff = FormatCoeffs[request.Format];
        decimal lessonCost = baseRate * (request.DurationMinutes / 60m) * formatCoeff;

        if (request.NeedMaterials) {
            lessonCost += _materialsFeeService.GetCustomMaterialsFee(request.Subject, request.Difficulty.ToString());
        }
           

        decimal discount = GetPackageDiscount(request.NumberOfLessons);
        decimal totalCost = lessonCost * request.NumberOfLessons * discount;

        return new TutoringQuote(totalCost, lessonCost, errors);
    }

    private decimal GetPackageDiscount(int numberOfLessons) => _discountCalculator.GetDiscount(numberOfLessons);

    /* первая реализация для пункта 1
     * private decimal GetPackageDiscount(int numberOfLessons)
     {
         if (numberOfLessons == 1) return 1.0m;
         if (numberOfLessons >= 2 && numberOfLessons <= 10) return 0.95m;
         if (numberOfLessons >= 11 && numberOfLessons <= 30) return 0.90m;
         if (numberOfLessons >= 31) return 0.85m;
         throw new ArgumentOutOfRangeException(nameof(numberOfLessons), "Количество занятий должно быть от 1 до 40.");
     }*/
}