using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveryFan.Recruitment.PayoutCalculators
{
    public abstract class BasePayoutCalculator
    {
        protected IReadOnlyList<TournamentPayout> Calculate(Tournament tournament, IReadOnlyList<PayingPosition> payingPositions)
        {
            IReadOnlyList<TournamentEntry> orderedEntries = tournament.Entries.OrderByDescending(p => p.Chips).ToList();

            List<TournamentPayout> payouts = new List<TournamentPayout>();
            payouts.AddRange(payingPositions.Select((p, i) => new TournamentPayout()
            {
                Payout = p.Payout,
                UserId = orderedEntries[i].UserId
            }));

            return payouts;
        }
    }
}
