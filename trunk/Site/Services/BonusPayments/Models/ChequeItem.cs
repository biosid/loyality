namespace Vtb24.Site.Services.BonusPayments.Models
{
    public class ChequeItem
    {
        public string Article { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string TrimTitle(int length)
        {
            if (Title.Length > length)
            {
                return Title.Substring(0, length - 3) + "...";
            }

            return Title;
        }
    }
}