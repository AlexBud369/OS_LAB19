/*using TutorCostCalculatorLib.Enums;

namespace TutorCostCalculatorLib.Models
{
    public record TutoringRequest(
     string Subject,                 // "Math", "Physics", "English", "Programming", "Music"
     string TutorQualification,      // "Junior", "Experienced", "Expert"
     DifficultyLevel Difficulty,
     int DurationMinutes,            // 45..180
     LessonFormat Format,
     bool NeedMaterials,
     int NumberOfLessons             // 1..40
    );
}
*/

using TutorCostCalcLib.Enums;

namespace TutorCostCalcLib.Models
{
    public class TutoringRequest
    {
        public string Subject { get; }
        public string TutorQualification { get; }
        public DifficultyLevel Difficulty { get; }
        public int DurationMinutes { get; }
        public LessonFormat Format { get; }
        public bool NeedMaterials { get; }
        public int NumberOfLessons { get; }

        public TutoringRequest(
            string subject,
            string tutorQualification,
            DifficultyLevel difficulty,
            int durationMinutes,
            LessonFormat format,
            bool needMaterials,
            int numberOfLessons)
        {
            Subject = subject;
            TutorQualification = tutorQualification;
            Difficulty = difficulty;
            DurationMinutes = durationMinutes;
            Format = format;
            NeedMaterials = needMaterials;
            NumberOfLessons = numberOfLessons;
        }
    }
}