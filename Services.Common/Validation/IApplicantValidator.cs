namespace Services.Common.Abstractions.Validation
{
    public interface IApplicantValidator
    {
        bool IsValidAge(DateOnly dateOfBirth, int minAge, int maxAge);
    }
}
