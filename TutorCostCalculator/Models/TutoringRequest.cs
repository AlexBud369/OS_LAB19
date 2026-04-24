using TutorCostCalculatorLib.Enums;

namespace TutorCostCalculatorLib.Models;

public record TutoringRequest(
 string Subject,                 // "Math", "Physics", "English", "Programming", "Music"
 string TutorQualification,      // "Junior", "Experienced", "Expert"
 DifficultyLevel Difficulty,
 int DurationMinutes,            // 45..180
 LessonFormat Format,
 bool NeedMaterials,
 int NumberOfLessons             // 1..40
);
