using Banking;
using NSubstitute;

namespace LearningDeveloperTests.BankAccount;
public class AccountInteractions
{
    [Fact]
    public void AccountUsesTheBonusCalculator()
    {
        var stubbedBonusCalculator = Substitute.For<ICalculateBonuses>();
        var account = new Account(stubbedBonusCalculator, Substitute.For<IProvideCustomerRelations>());
        var openingBalance = account.GetBalance();
        var amountToDeposit = 113.82M;
        stubbedBonusCalculator.HowMuchBonusFor(openingBalance, amountToDeposit).Returns(420.69M);

        account.Deposit(amountToDeposit);

        // How will I verify that the bonus calculator got called with the right balance, right amount of deposit
        // AND it added it to the balance?

        Assert.Equal(openingBalance + amountToDeposit + 420.69M, account.GetBalance());

    }
    [Fact]
    public void NotifiesOfZeroBalanceOnWithdrawal()
    {

        var stubbedPR = Substitute.For<IProvideCustomerRelations>();
        var account = new Account(Substitute.For<ICalculateBonuses>(), stubbedPR);

        account.Withdraw(account.GetBalance()); // Go to zero

        stubbedPR
            .Received()
            .SendEmailAboutBalanceBeingZero(account);
    }

    [Fact]
    public void NotNotifiedWhenBalanceIsAboutZero()
    {
        var stubbedPR = Substitute.For<IProvideCustomerRelations>();
        var account = new Account(Substitute.For<ICalculateBonuses>(), stubbedPR);

        account.Withdraw(account.GetBalance() - .01M); // Go to zero

        stubbedPR
            .DidNotReceive()
            .SendEmailAboutBalanceBeingZero(Arg.Any<Account>());
    }
}
