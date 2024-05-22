

using Banking;
using NSubstitute;

namespace LearningDeveloperTests.BankAccount;
public class MakingWithdrawals
{
    private readonly Account account;
    public MakingWithdrawals()
    {
        account = new Account(Substitute.For<ICalculateBonuses>(), Substitute.For<IProvideCustomerRelations>());
    }

    [Theory]
    [InlineData(25.23)]

    public void MakingWithdrawalsDecreasesBalance(decimal amountToWithdraw)
    {

        var openingBalance = account.GetBalance();


        account.Withdraw(amountToWithdraw);

        Assert.Equal(openingBalance - amountToWithdraw, account.GetBalance());
    }

    [Fact]
    public void OverdraftNotAllowed()
    {

        var openingBalance = account.GetBalance();

        Assert.Throws<AccountOverdraftException>(() => account.Withdraw(openingBalance + .01M));

        Assert.Equal(openingBalance, account.GetBalance());

    }

    [Fact]
    public void CanWithdrawFullAmount()
    {


        account.Withdraw(account.GetBalance());

        Assert.Equal(0, account.GetBalance());
    }
}
