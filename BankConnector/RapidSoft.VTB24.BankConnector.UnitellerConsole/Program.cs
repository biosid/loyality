using System;
using System.IO;
using RapidSoft.VTB24.BankConnector.Acquiring;

namespace RapidSoft.VTB24.BankConnector.UnitellerConsole
{
    [Args.ArgsModel(SwitchDelimiter = "-")]
    public class Arguments
    {
        [Args.ArgsMemberSwitch(0)]
        public string Command { get; set; }

        [Args.ArgsMemberSwitch("s", "shopid")]
        public string ShopId { get; set; }

        [Args.ArgsMemberSwitch("c", "client")]
        public string Client { get; set; }

        [Args.ArgsMemberSwitch("o", "orderid")]
        public string OrderId { get; set; }

        [Args.ArgsMemberSwitch("a", "amount")]
        public decimal Amount { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var config = Args.Configuration.Configure<Arguments>();

            Arguments arguments;

            try
            {
                arguments = config.CreateAndBind(args);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid arguments");
                Console.WriteLine();
                return;
            }

            switch (arguments.Command.ToLower())
            {
                case "pay":
                    ExecutePay(arguments);
                    break;

                case "payverify":
                    ExecutePayVerify(arguments);
                    break;

                default:
                    Console.WriteLine("Unknown command \"{0}\"", arguments.Command);
                    break;
            }
        }

        private static void WritePayArgs(Arguments arguments)
        {
            Console.WriteLine("Arguments:");
            Console.WriteLine("    ShopId  = " + arguments.ShopId);
            Console.WriteLine("    Client  = " + arguments.Client);
            Console.WriteLine("    OrderId = " + arguments.OrderId);
            Console.WriteLine("    Amount  = " + arguments.Amount);
        }

        private static void ExecutePay(Arguments arguments)
        {
            Console.WriteLine("Command: pay");
            WritePayArgs(arguments);

            try
            {
                var provider = new UnitellerProvider();

                provider.Pay(arguments.ShopId, arguments.Amount, arguments.Client, arguments.OrderId);

                Console.WriteLine("Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("ExceptionCaught: " + e);
            }
        }

        private static void ExecutePayVerify(Arguments arguments)
        {
            Console.WriteLine("Command: payverify");
            WritePayArgs(arguments);
        }
    }
}
