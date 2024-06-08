using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawPoker
{
    internal class Game
    {
        /// <summary>
        /// В этом классе реализован паттерн Singleton, который обеспечивает уникальность экземпляра класса Game
        /// </summary>

        private static Game instance;

        public List<Player> Players { get; private set; }
        public List<NameRound> Rounds { get; private set; }
        public Dealer DealerInGame { get; private set; }
        public Deck Deck { get; private set; }
        public int Pot { get; private set; }
        public int BigRate { get; private set; }
        public int ConstAnte = 200;


        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game();
                }
                return instance;
            }
        }

        private Game()
        {
            Players = new List<Player>();
            Rounds = new List<NameRound>();
            BigRate = 0;
        }

        public void ClearBigRate()
        {
            BigRate = 0;
        }

        public void ClearPlayers()
        {
            Players.Clear();
        }

        public void ClearRounds()
        {
            Rounds.Clear();
        }

        public void ClearPot()
        {
            Pot = 0;
        }

        public void ClearMovePlayer()
        {
            foreach (Player player in Players.Where(p => p.Status))
            {
                player.ClearMove();
            }
        }

        public void AddPlayers(List<(string Name, int Bank, bool Status)> playerData) 
        {
            foreach ((string Name, int Bank, bool Status) in playerData)
            {
                Player player = new Player(Name, Bank, Status);
                Players.Add(player);
            }
        }

        public void AddToPot()
        {
            foreach (Player player in Players)
            {
                if ((player.Status == true) && (player.Rate != 0))
                {
                    Pot += player.Rate;
                    player.ClearRate();
                }
                else if (player.Status == false)
                {
                    player.ClearRate();
                }
            }
        }

        public void AddRound(NameRound round) //добавление Раунда
        {
            if (Rounds.Count == 4)
            {
                Rounds.Clear();
            }
            Rounds.Add(round);
        }

        public void IdentifyADealer() //определение Дилера
        {
            foreach (Player player in Players.Where(p => p.Status))
            {
                DealerInGame = new Dealer(player);
                break;
            }
        }

        public int IdentifyActivePlayer() //получить индекс активного Игрока
        {
            int indexActivePlayer = -1;
            foreach (Player player in Players)
            {
                if (player.Status == true)
                {
                    indexActivePlayer = Players.IndexOf(player);
                    break;
                }
            }
            return indexActivePlayer;
        }

        public int CountActivePlayers() //получить количество активных Игроков по статусу и картам
        {
            int countPlayers = 0;
            foreach (Player player in Players)
            {
                if (player.Status == true && player.Hand.Cards.Count == 5)
                {
                    countPlayers++;
                }
            }
            return countPlayers;
        }

        public int CountStatusTruePlayers() //получить количество активных Игроков по статусу
        {
            int countPlayers = 0;
            foreach (Player player in Players)
            {
                if (player.Status == true)
                {
                    countPlayers++;
                }
            }
            return countPlayers;
        }

        public void ChangeDealer() //переход Дилера следующему игроку
        {
            int currentDealerIndex = Players.IndexOf(DealerInGame.DealerPlayer);
            int newDealerIndex = (currentDealerIndex + 1) % Players.Count;
            DealerInGame = new Dealer(Players[newDealerIndex]);
            MoveTransition();
        }

        public void ChangeBigRate(int rate)
        {
            BigRate = rate;
        }

        public void ChangeQueue() //перемещение первого активного Игрока в конец
        {
            int index = Players.FindIndex(p => p.Status); // Находим индекс первого игрока с Status = true
            if (index != -1)
            {
                Player playerToMove = Players[index]; // Получаем игрока для перемещения
                Players.RemoveAt(index); // Удаляем игрока из текущей позиции
                Players.Add(playerToMove); // Добавляем игрока в конец списка
            }
        }

        public void MoveTransition()
        {
            ChangeQueue();
            while (Players[IdentifyActivePlayer()].Move == "FOLD")
            {
                ChangeQueue();
            }
        }

        public bool CheckPlayerIsDealer(Player player) //проверить является ли Игрок Дилером
        {
            int currentDealerIndex = Players.IndexOf(DealerInGame.DealerPlayer);
            if (player == Players[currentDealerIndex])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckRateIsEqual() //проверка на соответствие всех ставок
        {
            int? firstRate = null;

            foreach (Player player in Players)
            {
                if (player.Status == true && player.Hand.Cards.Count == 5)
                {
                    if (firstRate == null)
                    {
                        firstRate = player.Rate;
                    }
                    else if (player.Rate != firstRate && player.Bank != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void DealingCards() //раздача карт Игрокам
        {
            Deck = new Deck();
            Deck.Shuffle();
            foreach (Player player in Players)
            {
                if (player.Status == true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        player.Hand.AddCard(Deck.Deal());
                    }
                }
            }
        }

        public void Replacements(Player player, int indexCard)
        {
            player.Hand.Cards[indexCard] = Deck.Deal();
        } //замена карты у игрока



        public List<Player> IdentifyWinner()
        {
            foreach (Player player in Players)
            {
                if (player.Status == true && player.Hand.Cards.Count() == 5)
                {
                    player.Hand.HandCombination = DetermineHandCombination(player);
                }
            }

            NameCombination winnerCombination = NameCombination.HighCard;
            foreach (Player player in Players)
            {
                if (player.Status == true && player.Hand.Cards.Count() == 5 && player.Hand.HandCombination < winnerCombination)
                {
                    winnerCombination = player.Hand.HandCombination;
                }
            }

            List<Player> winnersPlayers = new List<Player>();
            foreach (Player player in Players)
            {
                if (player.Status == true && player.Hand.Cards.Count() == 5 && player.Hand.HandCombination == winnerCombination)
                {
                    winnersPlayers.Add(player);
                }
            }

            if (winnersPlayers.Count() != 1)
            {
                if(winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.HighCard) 
                    || winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.Pair)
                    || winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.ThreeOfAKind)
                    || winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.FourOfAKind))
                {
                    List<Player> winnersPlayersByRank = WinnersByRank(winnersPlayers);
                    if (winnersPlayersByRank.Count() != 1)
                    {
                        return WinnersByKicker(winnersPlayersByRank);
                    }
                    else
                    {
                        return winnersPlayersByRank;
                    }
                }
                else if (winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.TwoPairs)
                    || winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.FullHouse))
                {
                    List<Player> winnersPlayersByCombination = WinnersByCombination(winnersPlayers);
                    if (winnersPlayersByCombination.Count() != 1)
                    {
                        return WinnersByKicker(winnersPlayersByCombination);
                    }
                    else
                    {
                        return winnersPlayersByCombination;
                    }
                }
                else if (winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.Straight)
                    || winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.StraightFlush)
                    || winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.RoyalFlush))
                {
                    return WinnersByRank(winnersPlayers);
                }
                else if (winnersPlayers.All(player => player.Hand.HandCombination == NameCombination.Flush))
                {
                    List<Player> winnersPlayersByRank = WinnersByRank(winnersPlayers);
                    if (winnersPlayersByRank.Count() != 1)
                    {
                        return WinnersByCombination(winnersPlayers);
                    }
                    else
                    {
                        return winnersPlayersByRank;
                    }
                }
            }
            return winnersPlayers;
        }



        public List<Player> WinnersByRank(List<Player> winnersPlayers)
        {
            List<Player> winnersPlayersByRank = new List<Player>();
            int bigRank = winnersPlayers.Last().Hand.Rank;
            foreach (Player player in winnersPlayers)
            {
                if (player.Hand.Rank >= bigRank)
                {
                    bigRank = player.Hand.Rank;
                }
            }
            foreach (Player player in winnersPlayers)
            {
                if (player.Hand.Rank == bigRank)
                {
                    winnersPlayersByRank.Add(player);
                }
            }
            return winnersPlayersByRank;
        }
        public List<Player> WinnersByKicker(List<Player> winners)
        {
            List<Player> winnersPlayersByKicker = new List<Player>();
            Player winnerPlayer = winners[0];
            foreach (Player player in winners)
            {
                bool winner = false;
                for (int i = 0; i < player.Hand.Kickers.Count(); i++)
                {
                    if (player.Hand.Kickers[i].Rank > winnerPlayer.Hand.Kickers[i].Rank)
                    {
                        winnerPlayer = player;
                        winnersPlayersByKicker.Clear();
                        winner = true;
                        break;
                    }
                    else if (player.Hand.Kickers[i].Rank == winnerPlayer.Hand.Kickers[i].Rank)
                    {
                        winner = true;
                    }
                    else if (player.Hand.Kickers[i].Rank < winnerPlayer.Hand.Kickers[i].Rank)
                    {
                        winner = false;
                        break;
                    }
                }
                if (winner)
                {
                    winnersPlayersByKicker.Add(player);
                }
            }
            return winnersPlayersByKicker;
        }
        public List<Player> WinnersByCombination(List<Player> winnersPlayers)
        {
            List<Player> winnersPlayersByCombination = new List<Player>();
            Player winnerPlayer = winnersPlayers[0];
            foreach (Player player in winnersPlayers)
            {
                bool winner = false;
                for (int i = 0; i < player.Hand.Combination.Count(); i++)
                {
                    if (player.Hand.Combination[i].Rank > winnerPlayer.Hand.Combination[i].Rank)
                    {
                        winnerPlayer = player;
                        winnersPlayersByCombination.Clear();
                        winner = true;
                        break;
                    }
                    else if (player.Hand.Combination[i].Rank == winnerPlayer.Hand.Combination[i].Rank)
                    {
                        winner = true;
                    }
                    else if (player.Hand.Combination[i].Rank < winnerPlayer.Hand.Combination[i].Rank)
                    {
                        winner = false;
                        break;
                    }
                }
                if (winner)
                {
                    winnersPlayersByCombination.Add(player);
                }
            }
            return winnersPlayersByCombination;
        }














        public NameCombination DetermineHandCombination(Player player)
        {
            List<Card> cards = player.Hand.Cards.OrderBy(c => c.Rank).ToList();

            if (IsRoyalFlush(player, cards))
                return NameCombination.RoyalFlush;

            if (IsStraightFlush(player, cards))
                return NameCombination.StraightFlush;

            if (IsFourOfAKind(player, cards))
                return NameCombination.FourOfAKind;

            if (IsFullHouse(player, cards))
                return NameCombination.FullHouse;

            if (IsFlush(player, cards))
                return NameCombination.Flush;

            if (IsStraight(player, cards))
                return NameCombination.Straight;

            if (IsThreeOfAKind(player, cards))
                return NameCombination.ThreeOfAKind;

            if (IsTwoPairs(player, cards))
                return NameCombination.TwoPairs;

            if (IsPair(player, cards))
                return NameCombination.Pair;

            return IsHighCard(player, cards);
        }


        private NameCombination IsHighCard(Player player, List<Card> cards)
        {
            List<Rank> ranks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
            ranks.Reverse();
            cards.Reverse();

            foreach (Rank rank in ranks)
            {
                foreach (Card card in cards)
                {
                    if (card.Rank == rank)
                    {
                        player.Hand.Combination.Add(card);
                        player.Hand.Rank = (int)card.Rank;
                        player.Hand.Kickers = player.Hand.Cards.Where(c => c.Rank != card.Rank).OrderByDescending(c => c.Rank).ToList();
                        return NameCombination.HighCard;
                    }
                }
            }
            return NameCombination.HighCard;
        }
        private bool IsPair(Player player, List<Card> cards)
        {
            var groupsOfCardsByRank = cards.GroupBy(c => c.Rank).ToList();
            foreach (var group in groupsOfCardsByRank)
            {
                if (group.Count() == 2)
                {
                    player.Hand.Combination = new List<Card>(group);
                    player.Hand.Rank = (int)group.Key;
                    player.Hand.Kickers = player.Hand.Cards.Where(c => c.Rank != group.Key).OrderByDescending(c => c.Rank).ToList();
                    return true;
                }
            }
            return false;
        }
        private bool IsTwoPairs(Player player, List<Card> cards)
        {
            var groupsOfCardsByRank = cards.GroupBy(c => c.Rank).ToList();
            var pairs = groupsOfCardsByRank.Where(g => g.Count() == 2).ToList();
            if (pairs.Count() == 2)
            {
                var pairsRanks = pairs.Select(g => g.Key).ToList();
                player.Hand.Rank = (int)pairsRanks[1];
                player.Hand.Kickers = cards.Where(c => !pairsRanks.Contains(c.Rank)).ToList();
                player.Hand.Combination.AddRange(pairs[1]);
                player.Hand.Combination.AddRange(pairs[0]);
                return true;
            }
            return false;
        }
        private bool IsThreeOfAKind(Player player, List<Card> cards)
        {
            var groupsOfCardsByRank = cards.GroupBy(c => c.Rank).ToList();
            var three = groupsOfCardsByRank.Where(g => g.Count() == 3).ToList();
            if (three.Count() == 1)
            {
                var threeRanks = three.Select(g => g.Key).ToList();
                player.Hand.Rank = (int)threeRanks[0];
                player.Hand.Kickers = cards.Where(c => !threeRanks.Contains(c.Rank)).ToList();
                player.Hand.Combination.AddRange(three[0]);
                return true;
            }
            return false;
        }
        private bool IsStraight(Player player, List<Card> cards)
        {
            int index = (int)cards[0].Rank;
            bool flag;
            foreach (Card card in cards)
            {
                if ((int)card.Rank == index)
                {
                    index++;
                    continue;
                }
                else if (((int)card.Rank == 12) && ((int)cards[0].Rank == 0) && ((int)cards[3].Rank == 3))
                {
                    flag = true;
                }
                else
                {
                    return false;
                }
            }
            flag = true;
            if (flag)
            {
                player.Hand.Rank = (int)cards[4].Rank;
                player.Hand.Combination.Add(cards[0]);
                player.Hand.Combination.Add(cards[1]);
                player.Hand.Combination.Add(cards[2]);
                player.Hand.Combination.Add(cards[3]);
                player.Hand.Combination.Add(cards[4]);
            }
            return flag;
        }
        private bool IsFlush(Player player, List<Card> cards)
        {
            var groupsOfCardsBySuit = cards.GroupBy(c => c.Suit).ToList();
            var flush = groupsOfCardsBySuit.Any(g => g.Count() == 5);
            if (flush)
            {
                player.Hand.Rank = (int)cards[4].Rank;
                player.Hand.Combination.Add(cards[4]);
                player.Hand.Combination.Add(cards[3]);
                player.Hand.Combination.Add(cards[2]);
                player.Hand.Combination.Add(cards[1]);
                player.Hand.Combination.Add(cards[0]);
                return true;
            }
            return false;
        }
        private bool IsFullHouse(Player player, List<Card> cards)
        {
            var groupsOfCardsByRank = cards.GroupBy(c => c.Rank).ToList();
            var pair = groupsOfCardsByRank.Where(g => g.Count() == 2).ToList();
            var three = groupsOfCardsByRank.Where(g => g.Count() == 3).ToList();

            if (pair.Count() == 1 && three.Count() == 1)
            {
                player.Hand.Combination.AddRange(three[0]);
                player.Hand.Combination.AddRange(pair[0]);
                return true;
            }
            return false;
        }
        private bool IsFourOfAKind(Player player, List<Card> cards)
        {
            var groupsOfCardsByRank = cards.GroupBy(c => c.Rank).ToList();
            var four = groupsOfCardsByRank.Where(g => g.Count() == 4).ToList();
            if (four.Count() == 1)
            {
                var fourRanks = four.Select(g => g.Key).ToList();
                player.Hand.Rank = (int)fourRanks[0];
                player.Hand.Kickers = cards.Where(c => !fourRanks.Contains(c.Rank)).ToList();
                player.Hand.Combination.AddRange(four[0]);
                return true;
            }
            return false;
        }
        private bool IsStraightFlush(Player player, List<Card> cards)
        {
            var isFlush = IsFlush(player, cards);
            var isStraight = IsStraight(player, cards);
            return isFlush && isStraight;
        }
        private bool IsRoyalFlush(Player player, List<Card> cards)
        {
            var isFlush = IsFlush(player, cards);
            var isStraight = IsStraight(player, cards);
            return isFlush && isStraight && cards[0].Rank == Rank.Ten;
        }
    }
}
