namespace Banking;

public interface IProvideCustomerRelations
{
    void SendEmailAboutBalanceBeingZero(Account account);
}