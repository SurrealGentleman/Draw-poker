using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawPoker
{
    public partial class GameScreenForm : Form
    {
        private Label[] namePlayerLabels;
        private Label[] bankPlayerLabels;
        private Label[] movePlayerLabels;
        private PictureBox[] picturePlayerPictureBox;
        private PictureBox[] pictureCardPictureBox;
        private Button[] buttonCardRep;
        private Label[] listLabelNameShowdown;
        private Label[] labelsWinner;
        private List<List<PictureBox>> listCardsShowdown;

        public GameScreenForm()
        {
            InitializeComponent();
            namePlayerLabels = new Label[] { namePlayer1, namePlayer2, namePlayer3, namePlayer4, namePlayer5 };
            bankPlayerLabels = new Label[] { bankPlayer1, bankPlayer2, bankPlayer3, bankPlayer4, bankPlayer5 };
            movePlayerLabels = new Label[] { movePlayer1, movePlayer2, movePlayer3, movePlayer4, movePlayer5 };
            picturePlayerPictureBox = new PictureBox[] { null, picturePlayer2, picturePlayer3, picturePlayer4, picturePlayer5 };
            pictureCardPictureBox = new PictureBox[] { card1, card2, card3, card4, card5};
            buttonCardRep = new Button[] { buttonCardRep1, buttonCardRep2, buttonCardRep3, buttonCardRep4, buttonCardRep5 };
            listLabelNameShowdown = new Label[] { labelName1Showdown, labelName2Showdown, labelName3Showdown, labelName4Showdown, labelName5Showdown };
            labelsWinner = new Label[] { labelWinner1, labelWinner2, labelWinner3, labelWinner4, labelWinner5 };
            listCardsShowdown = new List<List<PictureBox>>
            {
                new List<PictureBox> {P1card1Showdown, P1card2Showdown, P1card3Showdown, P1card4Showdown, P1card5Showdown},
                new List<PictureBox> {P2card1Showdown, P2card2Showdown, P2card3Showdown, P2card4Showdown, P2card5Showdown},
                new List<PictureBox> {P3card1Showdown, P3card2Showdown, P3card3Showdown, P3card4Showdown, P3card5Showdown},
                new List<PictureBox> {P4card1Showdown, P4card2Showdown, P4card3Showdown, P4card4Showdown, P4card5Showdown},
                new List<PictureBox> {P5card1Showdown, P5card2Showdown, P5card3Showdown, P5card4Showdown, P5card5Showdown}
            };
            PrintPlayer();
            PrintPot();
        }

        private void PrintPlayer() //отображение данных об игроках внесенных ante
        {
            for(int i=0; i<5; i++)
            {
                namePlayerLabels[i].Text = "";
                bankPlayerLabels[i].Text = "";
                movePlayerLabels[i].Text = "";
                if(picturePlayerPictureBox[i] != null)
                {
                    picturePlayerPictureBox[i].Visible = false;
                }
            }
            for (int i = 0; i < Game.Instance.Players.Count; i++)
            {
                if (Game.Instance.Players[i].Status == true)
                {
                    for (int j = 0; j < namePlayerLabels.Length; j++)
                    {
                        if (string.IsNullOrEmpty(namePlayerLabels[j].Text))
                        {
                            namePlayerLabels[j].Text = Game.Instance.Players[i].Name;
                            namePlayerLabels[j].Visible = true;
                            bankPlayerLabels[j].Text = Game.Instance.Players[i].Bank.ToString();
                            bankPlayerLabels[j].Visible = true;
                            if (Game.Instance.Players[i].Move != null)
                            {
                                movePlayerLabels[j].Text = Game.Instance.Players[i].Move;
                                movePlayerLabels[j].Visible = true;
                            }
                            if (j > 0)
                            {
                                picturePlayerPictureBox[j].Visible = true;
                            }
                            break;
                        }
                    }
                }
            }
        }
        private void PrintPot() //отобразить Pot
        {
            labelPot.Text = "Pot=" + Game.Instance.Pot.ToString();
        }
        private void HideCards() //скрыть карты активного игрока
        {
            buttonHideCards.Visible = false;
            buttonShowCards.Visible = true;
            foreach (PictureBox pictureBox in pictureCardPictureBox)
            {
                pictureBox.Visible = true;
                LoadCardImage(pictureBox);
            }
        }
        private void LoadCardImage(PictureBox pictureBox, string rank = "JOKER", string suit = "") //вставка картинки в pictureBox
        {
            string imagePath = Path.Combine(Application.StartupPath, @"..\..\images", "cards128", $"{rank}{suit}.png");
            pictureBox.Image = Image.FromFile(imagePath);
        }



        private void buttonDealingCards_Click(object sender, EventArgs e)
        {
            Game.Instance.DealingCards();//раздача карт
            buttonDealingCards.Visible = false;
            HideCards();
            RoundDefinition();
        }
        

        private void buttonShowCards_Click(object sender, EventArgs e) //отобразить карты активного игрока
        {
            buttonShowCards.Visible = false;
            buttonHideCards.Visible = true;
            int indexActivePlayer = Game.Instance.IdentifyActivePlayer();

            for (int i = 0; i < Game.Instance.Players[indexActivePlayer].Hand.Cards.Count; i++)
            {
                LoadCardImage(pictureCardPictureBox[i], 
                    Game.Instance.Players[indexActivePlayer].Hand.Cards[i].Rank.ToString(), 
                    Game.Instance.Players[indexActivePlayer].Hand.Cards[i].Suit.ToString());
            }
        }
        private void buttonHideCards_Click(object sender, EventArgs e) //кнопка скрытия карт активного игрока
        {
            HideCards();
        }
        

       



        private void RoundDefinition()
        {
            Game.Instance.ClearMovePlayer();
            PrintPlayer();
            if (Game.Instance.Rounds.Count == 0){
                Trade();
            }
            else if ((Game.Instance.Rounds.Last() == NameRound.REPLACEMENTS) && (Game.Instance.Rounds.Count == 2))
            {
                Trade();
            }
            else if ((Game.Instance.Rounds.Last() == NameRound.TRADE) && (Game.Instance.Rounds.Count == 1))
            {
                Replacements();
            }
            else if ((Game.Instance.Rounds.Last() == NameRound.TRADE) && (Game.Instance.Rounds.Count == 3))
            {
                Showdown();
            }
        }

        private void Trade()
        {
            Game.Instance.ClearBigRate();
            Game.Instance.AddRound(NameRound.TRADE);
            Game.Instance.MoveTransition();
            PrintPlayer();
            if (Game.Instance.Players[Game.Instance.IdentifyActivePlayer()].Bank != 0)
            {
                buttonBet.Visible = true;
            }
            buttonCheck.Visible = true;
            buttonFold.Visible = true;
            buttonShowCards.Visible = true;
            buttonCall.Visible = false;
            buttonRaise.Visible = false;
        }
        private void Replacements()
        {
            Game.Instance.AddRound(NameRound.REPLACEMENTS);
            Game.Instance.MoveTransition();
            PrintPlayer();
            buttonBet.Visible = false;
            buttonCheck.Visible = false;
            buttonFold.Visible = false;
            buttonRaise.Visible = false;
            buttonCall.Visible = false;
            buttonShowCards.Visible = true;
            buttonReplacements.Visible = true;
            foreach (Button button in buttonCardRep)//показать кнопки
            {
                button.Visible = true;
                button.Enabled = true;
            }

        }
        private void Showdown(bool flag=false)
        {
            Game.Instance.AddRound(NameRound.SHOWDOWN);
            buttonBet.Visible = false;
            buttonCheck.Visible = false;
            buttonFold.Visible = false;
            buttonRaise.Visible = false;
            buttonCall.Visible = false;
            buttonShowCards.Visible = false;
            buttonHideCards.Visible = false;

            List<Player> winnersPlayers = Game.Instance.IdentifyWinner();

            foreach (PictureBox pictureBox in pictureCardPictureBox)
            {
                pictureBox.Visible = false;
            }

            if (flag==true)
            {
                winnersPlayers[0].IncreaseBank(Game.Instance.Pot);
                Game.Instance.MoveTransition();
                PrintPlayer();
                Game.Instance.ClearPot();
                PrintPot();
                listLabelNameShowdown[0].Text = winnersPlayers[0].Name;
                listLabelNameShowdown[0].Visible = true;
                labelsWinner[0].Text = $"Победитель";
                labelsWinner[0].Visible = true;
                buttonNextGame.Visible = true;
                buttonUpdateDataPlayer.Visible = true;
                return;
            }

            int index = 0;
            foreach (Player player in Game.Instance.Players)
            {
                if (player.Status == true && player.Hand.Cards.Count() == 5)
                {
                    listLabelNameShowdown[index].Text = player.Name;
                    listLabelNameShowdown[index].Visible = true;
                    if (winnersPlayers.Contains(player))
                    {
                        labelsWinner[index].Text = $"Победитель\n{player.Hand.HandCombination}";
                        labelsWinner[index].Visible = true;
                    }
                    for (int j = 0; j < player.Hand.Cards.Count(); j++)
                    {
                        listCardsShowdown[index][j].Visible = true;
                        LoadCardImage(listCardsShowdown[index][j], player.Hand.Cards[j].Rank.ToString(), player.Hand.Cards[j].Suit.ToString());
                    }
                    index++;
                }
            }
            if (winnersPlayers.Count > 1)
            {
                int splitPot = Game.Instance.Pot / winnersPlayers.Count;
                foreach (Player player in winnersPlayers)
                {
                    player.IncreaseBank(splitPot);
                }
            }
            else
            {
                winnersPlayers[0].IncreaseBank(Game.Instance.Pot);
            }
            PrintPlayer();
            Game.Instance.ClearPot();
            PrintPot();

            foreach (Player player in Game.Instance.Players)
            {
                if (player.Status == true && player.Bank == 0)
                {
                    player.Status = false;
                }
            }
            if (Game.Instance.CountStatusTruePlayers() < 2)
            {
                buttonUpdateDataPlayer.Visible = true;
            }
            else
            {
                buttonNextGame.Visible = true;
                buttonUpdateDataPlayer.Visible = true;
            }
            
            
        }

        private void FinishRound()
        {
            Game.Instance.AddToPot();
            PrintPot();
            RoundDefinition();
        }





        private void buttonBet_Click(object sender, EventArgs e)
        {
            buttonBetAndRaiseVisible(0);
            
        }
        private void buttonRaise_Click(object sender, EventArgs e)
        {
            buttonBetAndRaiseVisible(Game.Instance.BigRate);
            
        }
        private void buttonBetAndRaiseVisible(int minRate)
        {
            if(Game.Instance.Players[Game.Instance.IdentifyActivePlayer()].Bank != 0)
            {
                numericRate.Visible = true;
                buttonOk.Visible = true;
                int indexActivePlayer = Game.Instance.IdentifyActivePlayer();
                numericRate.Minimum = minRate;
                numericRate.Maximum = Game.Instance.Players[indexActivePlayer].Bank;
            }
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            BetOrFoldOrCheckOrNextRep(false, true, false, int.Parse(numericRate.Value.ToString()));
        }
        private void buttonCall_Click(object sender, EventArgs e)
        {
            if (Game.Instance.BigRate - Game.Instance.Players[Game.Instance.IdentifyActivePlayer()].Rate 
                > Game.Instance.Players[Game.Instance.IdentifyActivePlayer()].Bank)
            {
                BetOrFoldOrCheckOrNextRep(false, true, false, Game.Instance.Players[Game.Instance.IdentifyActivePlayer()].Bank);
            }
            else
            {
                BetOrFoldOrCheckOrNextRep(false, true, false, Game.Instance.BigRate - Game.Instance.Players[Game.Instance.IdentifyActivePlayer()].Rate);
            }
            
        }
        private void buttonFold_Click(object sender, EventArgs e)
        {
            BetOrFoldOrCheckOrNextRep(true, false, false);
        }
        private void buttonCheck_Click(object sender, EventArgs e)
        {
            BetOrFoldOrCheckOrNextRep(false, true, false);
        }

        private void buttonReplacements_Click(object sender, EventArgs e)
        {
            int indexActivePlayer = Game.Instance.IdentifyActivePlayer();
            for (int i = 0; i < buttonCardRep.Length; i++)
            {
                if (buttonCardRep[i].Enabled == false)
                {
                    Game.Instance.Replacements(Game.Instance.Players[indexActivePlayer], i);
                }
            }
            HideCards();
            buttonReplacements.Visible = false;
            buttonCall.Visible = false;
            buttonRaise.Visible = false;
            foreach (Button button in buttonCardRep)//скрыть кнопки
            {
                button.Visible = false;
            }
            buttonNextPlayer.Visible = true;
        }
        private void buttonNextPlayer_Click(object sender, EventArgs e)
        {
            BetOrFoldOrCheckOrNextRep(false, false, true);

        }


        private void BetOrFoldOrCheckOrNextRep(bool fold, bool bet, bool rep, int rate=0)
        {
            int indexActivePlayer = Game.Instance.IdentifyActivePlayer();
            if (bet == true)
            {
                Game.Instance.Players[indexActivePlayer].RememberBet(rate);//запомнить ставку
            }
            else if (fold == true)
            {
                Game.Instance.Players[indexActivePlayer].FoldCards();//очистить руку
            }
            bool isDealer = Game.Instance.CheckPlayerIsDealer(Game.Instance.Players[indexActivePlayer]);//является ли Игрок Дилером
            bool isNextPlayerDealer = Game.Instance.CheckPlayerIsDealer(Game.Instance.Players[indexActivePlayer + 1]);//является ли следующий Игрок Дилером
            bool is2NextPlayerDealer = false;
            bool is3NextPlayerDealer = false;
            if (Game.Instance.CountStatusTruePlayers() == 4)
            {
                is2NextPlayerDealer = Game.Instance.CheckPlayerIsDealer(Game.Instance.Players[indexActivePlayer + 2]);//является ли следующий2х Игрок Дилером
            }
            if (Game.Instance.CountStatusTruePlayers() == 5)
            {
                is2NextPlayerDealer = Game.Instance.CheckPlayerIsDealer(Game.Instance.Players[indexActivePlayer + 2]);//является ли следующий2х Игрок Дилером
                is3NextPlayerDealer = Game.Instance.CheckPlayerIsDealer(Game.Instance.Players[indexActivePlayer + 3]);//является ли следующий3х Игрок Дилером
            }
            HideCards();//скрыть карты

            if (Game.Instance.CountActivePlayers() < 2)
            {
                Showdown(true);
                return;
            }
            else
            {
                if (isDealer == true 
                    || (isNextPlayerDealer == true && Game.Instance.Players[indexActivePlayer + 1].Move == "FOLD") 
                    || (is2NextPlayerDealer == true && Game.Instance.Players[indexActivePlayer + 2].Move == "FOLD" && Game.Instance.Players[indexActivePlayer + 1].Move == "FOLD") 
                    || (is3NextPlayerDealer == true && Game.Instance.Players[indexActivePlayer + 3].Move == "FOLD" && Game.Instance.Players[indexActivePlayer + 2].Move == "FOLD" && Game.Instance.Players[indexActivePlayer + 1].Move == "FOLD"))
                {
                    if (Game.Instance.CheckRateIsEqual() == true)//являются ли ставки равными
                    {
                        if (rep == true)
                        {
                            buttonNextPlayer.Visible = false;//nextPlayerReplacements
                        }
                        FinishRound();//закончить раунд и перейти к следующему
                        return;
                    }
                }
                else
                {
                    buttonBet.Visible = false;
                }

                if (rep == true)
                {
                    foreach (Button button in buttonCardRep)//разблокировать кнопки
                    {
                        button.Enabled = true;
                        button.Visible = true;
                    }
                    buttonNextPlayer.Visible = false;
                    buttonReplacements.Visible = true;
                }
                Game.Instance.MoveTransition();
                PrintPlayer();

                if (rep == false)
                {
                    indexActivePlayer = Game.Instance.IdentifyActivePlayer();
                    numericRate.Visible = false;
                    buttonOk.Visible = false;
                    if (Game.Instance.Players[indexActivePlayer].Rate == Game.Instance.BigRate || Game.Instance.Players[indexActivePlayer].Bank == 0)
                    {
                        buttonCall.Visible = false;
                        buttonCheck.Visible = true;
                    }
                    else
                    {
                        buttonCheck.Visible = false;
                        buttonCall.Visible = true;
                    }
                    if (Game.Instance.BigRate > 0)
                    {
                        if ((Game.Instance.DealerInGame.DealerPlayer.Rate == Game.Instance.BigRate) && (Game.Instance.BigRate > 0))
                        {
                            buttonRaise.Visible = false;
                            buttonBet.Visible = false;
                        }
                        else
                        {
                            buttonBet.Visible = false;
                            buttonRaise.Visible = true;
                        }
                    }
                    else
                    {
                        buttonRaise.Visible = false;
                        buttonBet.Visible = true;
                    }
                }
            }

        }


        private void buttonCardRep1_Click(object sender, EventArgs e)
        {
            buttonCardRep1.Enabled = false;
        }
        private void buttonCardRep2_Click(object sender, EventArgs e)
        {
            buttonCardRep2.Enabled = false;
        }
        private void buttonCardRep3_Click(object sender, EventArgs e)
        {
            buttonCardRep3.Enabled = false;
        }
        private void buttonCardRep4_Click(object sender, EventArgs e)
        {
            buttonCardRep4.Enabled = false;
        }
        private void buttonCardRep5_Click(object sender, EventArgs e)
        {
            buttonCardRep5.Enabled = false;
        }

        private void buttonNextGame_Click(object sender, EventArgs e)
        {
            buttonNextGame.Visible = false;
            buttonUpdateDataPlayer.Visible = false;

            foreach (Label label in listLabelNameShowdown)
            {
                label.Visible = false;
            }
            foreach (Label label in labelsWinner)
            {
                label.Visible = false;
            }
            foreach (List<PictureBox> ListPictureBox in listCardsShowdown)
            {
                foreach(PictureBox pictureBox in ListPictureBox)
                {
                    pictureBox.Visible = false;
                }
            }

            Game.Instance.ClearRounds();
            Game.Instance.ClearBigRate();
            foreach (Player player in Game.Instance.Players)
            {
                if (player.Status == true)
                {
                    player.ClearAllMove();
                }
            }
            Game.Instance.ChangeDealer();
            foreach (Player player in Game.Instance.Players)
            {
                if(player.Status == true)
                {
                    player.RememberBet(Game.Instance.ConstAnte);
                    player.Hand.ClearHand();
                }
            }
            
            buttonDealingCards.Visible = true;
            Game.Instance.AddToPot();
            PrintPlayer();
            PrintPot();
        }

        private void buttonUpdateDataPlayer_Click(object sender, EventArgs e)
        {
            Game.Instance.ClearRounds();
            Game.Instance.ClearBigRate();
            foreach (Player player in Game.Instance.Players)
            {
                if (player.Status == true)
                {
                    player.RememberBet(Game.Instance.ConstAnte);
                    player.Hand.ClearHand();
                }
            }
            Game.Instance.ChangeDealer();
            CreatePlayerForm createPlayerForm = new CreatePlayerForm();
            createPlayerForm.Show();
            Hide();
            createPlayerForm.PrintDataPlayer();
        }
    }
}
