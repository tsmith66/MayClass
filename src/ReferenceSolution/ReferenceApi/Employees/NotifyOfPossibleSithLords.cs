namespace ReferenceApi.Employees;

public class NotifyOfPossibleSithLords(ILogger<NotifyOfPossibleSithLords> logger) : INotifyOfPossibleSithLords
{
    public void Notify(string firstName, string lastName)
    {
        // do whatever - send a text, ring the alarm.
        logger.LogInformation("We Have a Possible Sith Lord {first} {last}", firstName, lastName);
    }
}
