namespace Services.Common.Abstractions.Model;

public abstract record DomainEvent;

public record InvestorCreated(Guid UserId, string InvestorId) : DomainEvent;

public record InvestorCreationFailed(Guid UserId) : DomainEvent;

public record AccountCreated(string InvestorId, ProductCode Product, string AccountId) : DomainEvent;

public record AccountCreationFailed(string InvestorId, ProductCode Product) : DomainEvent;

public record KycFailed(Guid UserId, Guid ReportId) : DomainEvent;

public record PaymentFailed(string InvestorId, string AccountId, Guid ApplicationId) : DomainEvent;

public record ApplicationCompleted(Guid ApplicationId) : DomainEvent;

public record ApplicationDenied(Guid ApplicationId, string reason) : DomainEvent;