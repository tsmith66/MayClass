
using Banking;
using NSubstitute;

namespace LearningDeveloperTests.BankAccount;
public class NewAccountTests
{
    [Fact]
    public void NewAccountsHaveCorrectOpeningBalance()
    {

        var account = new Account(Substitute.For<ICalculateBonuses>(), Substitute.For<IProvideCustomerRelations>());

        decimal openingBalance = account.GetBalance();


        Assert.Equal(7000M, openingBalance);
    }
}

public class DummyBonusCalculator : ICalculateBonuses
{
    public decimal HowMuchBonusFor(decimal balance, decimal amountToDeposit)
    {
        return 0;
    }
}