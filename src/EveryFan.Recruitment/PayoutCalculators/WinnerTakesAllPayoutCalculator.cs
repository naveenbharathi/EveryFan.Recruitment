using System;
using System.Collections.Generic;
using System.Linq;

namespace EveryFan.Recruitment.PayoutCalculators
{
    /// <summary>
    /// Winner takes all payout calculator, the winner recieves the entire prize pool. In the event of a tie for the winning position the
    /// prize pool is split equally between the tied players.
    /// </summary>
    public class WinnerTakesAllPayoutCalculator : BasePayoutCalculator, IPayoutCalculator
    {
        private IReadOnlyList<PayingPosition> GetPayingPositions(Tournament tournament)
        {
            var maxChip = tournament.Entries.Max(x => x.Chips);
            var winners = tournament.Entries.Where(x => x.Chips == maxChip).ToList();

            var payout = tournament.PrizePool / winners.Count;
            var remainder = tournament.PrizePool % winners.Count;

            var payingPositions = new List<PayingPosition>();
            payingPositions.AddRange(winners.Select(w => new PayingPosition()
            {
                Position = 1,
                Payout = payout
            }));

            var rand = new Random();
            var randValue = rand.Next(0, winners.Count - 1);
            payingPositions[randValue].Payout = payingPositions[randValue].Payout + remainder;

            return payingPositions;
        }

        public IReadOnlyList<TournamentPayout> Calculate(Tournament tournament)
        {
            IReadOnlyList<PayingPosition> payingPositions = this.GetPayingPositions(tournament);
            return Calculate(tournament, payingPositions);
        }
    }
}
