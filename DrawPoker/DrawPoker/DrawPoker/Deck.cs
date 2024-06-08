using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker
{
    internal class Deck
    {
        public List<Card> CardsDeck { get; private set; }

        public Deck()
        {
            CreateDeck();
        }

        private void CreateDeck() //создание карты, добавление ее в колоду
        {
            CardsDeck = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    CardsDeck.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle() //перемешивание колоды
        {
            Random random = new Random();
            CardsDeck = CardsDeck.OrderBy(card => random.Next()).ToList();
        }

        public Card Deal() //взятие карты из колоды
        {
            Card card = CardsDeck[0];
            CardsDeck.RemoveAt(0);
            return card;
        }
    }
}
