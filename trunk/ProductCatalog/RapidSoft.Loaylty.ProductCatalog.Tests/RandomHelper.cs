using System;
using System.Text;

namespace RapidSoft.Loaylty.ProductCatalog.Tests.Common
{
    using API.Entities;

    using RapidSoft.Loaylty.ProductCatalog.ImportTests;
    using RapidSoft.Loaylty.ProductCatalog.Tests.DataSources;

    public static class RandomHelper
    {
        private static Random randomSeed = new Random();

        public static Order RandomOrder()
        {
            return TestDataStore.GetOrder();
        }

         public static Partner RandomPartner()
         {
             Partner p = new Partner();
             p.InsertedUserId = RandomString(64);
             p.Name = RandomString(256);
             p.Type = 0;
             p.Status = 0;
             p.ThrustLevel = 0;
             p.Description = RandomString(256);
             p.InsertedDate = DateTime.Now;

             return p;
         }

         public static DeliveryRate RandomDeliveryRate()
         {
             var p = new DeliveryRate
                         {
                             PartnerId = 1,
                             Kladr = RandomString(32),
                             MinWeightGram = RandomNumber(0, 1000),
                             MaxWeightGram = RandomNumber(1000, 3000),
                             PriceRUR = RandomNumber(100, 1000)
                         };

             return p;
         }

        public static string RandomString(int size, bool lowerCase = false)
        {
            /* StringBuilder is faster than using strings (+=)*/
            StringBuilder RandStr = new StringBuilder(size);

            /* Ascii start position (65 = A / 97 = a)*/
            int Start = (lowerCase) ? 97 : 65;

            /* Add random chars*/
            for (int i = 0; i < size; i++)
                RandStr.Append((char)(26 * randomSeed.NextDouble() + Start));

            return RandStr.ToString();
        }

        /* Returns a random number.*/
        public static int RandomNumber(int Minimal, int Maximal)
        {
            return randomSeed.Next(Minimal, Maximal);
        }

        /* Returns a random boolean value*/
        public static bool RandomBool()
        {
            return (randomSeed.NextDouble() > 0.5);
        }

        /* Returns DateTime in the range [min, max)*/
        public static DateTime RandomDateTime(DateTime min, DateTime max)
        {
            if (max <= min)
            {
                string message = "Max must be greater than min.";
                throw new ArgumentException(message);
            }
            long minTicks = min.Ticks;
            long maxTicks = max.Ticks;
            double rn = (Convert.ToDouble(maxTicks)
               - Convert.ToDouble(minTicks)) * randomSeed.NextDouble()
               + Convert.ToDouble(minTicks);
            return new DateTime(Convert.ToInt64(rn));
        }

        public static int NextInt32(this Random rng)
        {
            unchecked
            {
                int firstBits = rng.Next(0, 1 << 4) << 28;
                int lastBits = rng.Next(0, 1 << 28);
                return firstBits | lastBits;
            }
        }


        public static decimal RandomDecimal()
        {            
            //The low 32 bits of a 96-bit integer. 
            int lo = randomSeed.Next(int.MinValue, int.MaxValue);
            //The middle 32 bits of a 96-bit integer. 
            int mid = randomSeed.Next(int.MinValue, int.MaxValue);
            //The high 32 bits of a 96-bit integer. 
            int hi = 0;            
            byte scale = Convert.ToByte(randomSeed.Next(29));

            Decimal randomDecimal = new Decimal(lo, mid, hi, false, scale);

            return randomDecimal;
        }
    }
}
