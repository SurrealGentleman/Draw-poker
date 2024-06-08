using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker
{
    internal class Hand
    {
        public List<Card> Cards { get; private set; }
        public List<Card> Combination { get; set; }
        public List<Card> Kickers { get; set; }
        public int Rank = 0;
        public NameCombination HandCombination { get; set; }

        public Hand()
        {
            Cards = new List<Card>();
            Combination = new List<Card>();
            Kickers = new List<Card>();
        }

        public void AddCard(Card card) //добавление карты в руку
        {
            Cards.Add(card);
        }

        public void ClearHand() //очищение руки
        {
            Cards.Clear();
            Combination.Clear();
            Kickers.Clear();
            Rank = 0;
        }
    }
}
