

using Services.Common.Abstractions.Helpers;

namespace Services.Common.Abstractions.Validation
{
    public class ApplicantValidator : IApplicantValidator
    {
        readonly IAgeHelpers _ageHelpers;

        public ApplicantValidator(IAgeHelpers ageHelpers)
        {
            _ageHelpers = ageHelpers;
        }

        public bool IsValidAge(DateOnly dateOfBirth, int minAge, int maxAge)
        {
            var age = _ageHelpers.GetAge(dateOfBirth);

            if (age >= minAge && age <= maxAge) return true;

            return false;   
        }
    }
}