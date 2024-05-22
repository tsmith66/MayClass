
using Banking;
using NSubstitute;

namespace LearningDeveloperTests.BankAccount;
public class MakingDeposits
{
    [Theory]
    [InlineData(100)]
    public void MakingDepositsOnAccountsIncreasesBalance(decimal amountToDeposit)
    {
        // Given
        var account = new Account(Substitute.For<ICalculateBonuses>(), Substitute.For<IProvideCustomerRelations>());
        var openingBalance = account.GetBalance();

        // When
        account.Deposit(amountToDeposit);

        // Then
        Assert.Equal(openingBalance + amountToDeposit, account.GetBalance());
    }
}
