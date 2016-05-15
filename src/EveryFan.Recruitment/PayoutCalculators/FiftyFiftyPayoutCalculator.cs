using System;
using System.Collections.Generic;
using System.Linq;

namespace EveryFan.Recruitment.PayoutCalculators
{
    /// <summary>
    /// FiftyFifty payout calculator. The 50/50 payout scheme returns double the tournament buyin to people
    /// who finish in the top half of the table. If the number of runners is odd the player in the middle position
    /// should get their stake back. Any tied positions should have the sum of the amount due to those positions
    /// split equally among them.
    /// </summary>
    public class FiftyFiftyPayoutCalculator : BasePayoutCalculator, IPayoutCalculator
    {
        private IReadOnlyList<PayingPosition> GetPayingPositions(Tournament tournament)
        {
            var prizePool = tournament.PrizePool;
            var rand = new Random();

            var winners = tournament.Entries.GroupBy(x => x.Chips).Select(x => new {
                Chips = x.Key,
                Count = x.Count()
            }).OrderByDescending(x => x.Chips);

            var payingPositions = new List<PayingPosition>();

            int position = 1;

            foreach (var item in winners)
            {
                if(prizePool == 0)
                {
                    break;
                }

                int payout = 0;
                int remainder = 0;

                if (tournament.BuyIn * item.Count * 2 <= prizePool)
                {
                    payout = tournament.BuyIn * 2;
                }
                else if(tournament.BuyIn * item.Count <= prizePool && position != 1)
                {
                    payout = tournament.BuyIn;
                }
                else
                {
                    payout = prizePool / item.Count;
                    remainder = prizePool % item.Count;
                }
                

                var tempPayingPositions = new List<PayingPosition>();
                for (int i = 0; i < item.Count; i++)
                {
                    tempPayingPositions.Add(new PayingPosition()
                    {
                        Position = position,
                        Payout = payout
                    });
                    prizePool = prizePool - payout;
                }

                if (remainder != 0)
                {
                    var randValue = rand.Next(0, item.Count - 1);
                    tempPayingPositions[randValue].Payout = tempPayingPositions[randValue].Payout + remainder;

                    prizePool = prizePool - remainder;
                }

                payingPositions.AddRange(tempPayingPositions);
                position++;
            }

            return payingPositions;
        }

        public IReadOnlyList<TournamentPayout> Calculate(Tournament tournament)
        {
            IReadOnlyList<PayingPosition> payingPositions = this.GetPayingPositions(tournament);
            return Calculate(tournament, payingPositions);
        }
    }
}
