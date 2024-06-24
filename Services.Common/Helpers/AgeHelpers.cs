
namespace Services.Common.Abstractions.Helpers
{
    public class AgeHelpers : IAgeHelpers
    {
        public int GetAge(DateOnly dateOfBirth)
        {
            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;

            //if we have not yet reached birth month and day in the current year
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            return age;
        }
    }
}
