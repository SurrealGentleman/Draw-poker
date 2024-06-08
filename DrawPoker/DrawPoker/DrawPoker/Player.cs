using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker
{
    internal class Player
    {
        public string Name { get; private set; }
        public string Move { get; private set; }
        public int Bank { get; private set; }
        public int Rate { get; private set; }
        public bool Status { get; set; }
        public Hand Hand { get; private set; }
        
        public Player(string name, int bank, bool status)
        {
            Name = name;
            Bank = bank;
            Status = status;
            Hand = new Hand();
        }

        public void RememberBet(int rate)
        {
            if (rate == 0)
            {
                Move = $"CHECK - {Rate}";
            }
            else if (Rate+rate == Game.Instance.BigRate)
            {
                Rate += rate;
                Move = $"CALL {rate} - {Rate}";
            }
            else if (Rate + rate > Game.Instance.BigRate && Game.Instance.BigRate > 0)
            {
                Rate += rate;
                Move = $"RAISE {rate} - {Rate}";
                Game.Instance.ChangeBigRate(Rate);
            }
            else if(Rate + rate > 0 && Game.Instance.BigRate == 0)
            {
                Rate += rate;
                Move = $"BET {rate} - {Rate}";
                Game.Instance.ChangeBigRate(Rate);
            }

            ReduceBank(rate);
        }
        public void ReduceBank(int rate)
        {
            Bank -= rate;
        }
        public void IncreaseBank(int bank)
        {
            Bank += bank;
        }
        public void ClearRate()
        {
            Rate = 0;
        }
        public void ClearMove()
        {
            if (Move != "FOLD")
            {
                Move = "";
            }
        }
        public void FoldCards()
        {
            Hand.Cards.Clear();
            Move = "FOLD";
            Rate += 0;
        }
        public void ClearAllMove()
        {
            Move = "";
        }
    }
}
