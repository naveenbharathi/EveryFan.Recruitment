using System;
using System.Collections.Generic;
using EveryFan.Recruitment.PayoutCalculators;

namespace EveryFan.Recruitment
{
    public class PayoutEngine
    {
        public IReadOnlyList<TournamentPayout> Calculate(Tournament tournament)
        {
            IPayoutCalculator calculator = PayoutCalculatorFactory.CreatePayoutCalculator(tournament.PayoutScheme);

            return calculator.Calculate(tournament);
        }
    }
}
