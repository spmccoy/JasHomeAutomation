namespace SmartHomeCore.Domain.Common;

public class DomainErrorEvent : DomainEvent
{
    public string ErrorMessage { get; }

    public DomainErrorEvent(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

public class PersonNotFoundErrorEvent(string personId) : DomainErrorEvent(string.Format(ErrorMessageTemplate, personId))
{
    private const string ErrorMessageTemplate = "Unable to find person with Id: {0}";
}