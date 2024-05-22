namespace Banking;
public class StandardBonusCalculator
{
    public decimal CalculateBonus(decimal balance, decimal amount)
    {
        decimal amountOfBonus = 0;
        if (balance >= 6000)
        {
            amountOfBonus = amount * .10M;
        }
        return amountOfBonus;
    }
}
