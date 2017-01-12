namespace Vtb24.Site.Services.SmsBallance
{
    public interface ISmsBallanceService
    {
        decimal GetAccountBallance(string phoneNumber);

        bool ValidateRequestHash(string checkString, string hash);
    }
}
