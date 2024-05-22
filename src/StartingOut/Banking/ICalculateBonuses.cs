namespace Banking;

public interface ICalculateBonuses
{
    decimal HowMuchBonusFor(decimal balance, decimal amountToDeposit);
}