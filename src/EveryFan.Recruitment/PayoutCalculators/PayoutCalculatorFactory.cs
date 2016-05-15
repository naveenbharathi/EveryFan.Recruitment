using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveryFan.Recruitment.PayoutCalculators
{
    public class PayoutCalculatorFactory
    {
        public static IPayoutCalculator CreatePayoutCalculator(PayoutScheme payoutScheme)
        {
            switch (payoutScheme)
            {
                case PayoutScheme.FIFTY_FIFY:
                        return new FiftyFiftyPayoutCalculator();

                case PayoutScheme.WINNER_TAKES_ALL:
                        return new WinnerTakesAllPayoutCalculator();

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(payoutScheme));
                    }
            }
        }
    }
}
