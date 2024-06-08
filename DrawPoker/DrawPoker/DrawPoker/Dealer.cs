using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker
{
    internal class Dealer
    {
        public Player DealerPlayer { get; set; }

        public Dealer(Player dealerPlayer)
        {
            DealerPlayer = dealerPlayer;
        }
    }
}
