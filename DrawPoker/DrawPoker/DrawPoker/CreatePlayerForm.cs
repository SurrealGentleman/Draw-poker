using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawPoker
{
    public partial class CreatePlayerForm : Form
    {
        public CreatePlayerForm() //Инициализация компонента
        {
           InitializeComponent();
        }

        private void AddPlayers_Click(object sender, EventArgs e) //добавление игроков, запоминание ставки, добавление в pot, переход к GameScreenForm
        {
            int rowCount = dataGridView1.RowCount;//получили количество записей в dataGridView
            if (rowCount <= 2)
            {
                MessageBox.Show("Количество игроков должно быть больше одного");
            }
            else if(rowCount > 6)
            {
                MessageBox.Show("Количество игроков должно быть меньше шести");
            }
            else
            {
                int checkedAnteCount = GetCheckedCheckboxCount();//получили количество игроков внесенных ante
                if (checkedAnteCount < 2)
                {
                    MessageBox.Show("Количество игроков внесенных ante должно быть больше одного");
                }
                else
                {
                    var playerData = GetDataFromDataGridView(); //получили данные об игроках
                    if(Game.Instance.Players.Count() == 0)
                    {
                        Game.Instance.AddPlayers(playerData); //добавили игроков
                    }
                    else
                    {
                        Game.Instance.ClearPlayers();
                        Game.Instance.AddPlayers(playerData); //добавили игроков
                    }
                    Game.Instance.IdentifyADealer(); //определили дилера
                    foreach (Player player in Game.Instance.Players)
                    {
                        if (player.Status == true)
                        {
                            player.RememberBet(Game.Instance.ConstAnte); //запомнить ставку как ante
                        }
                    }
                    Game.Instance.AddToPot(); //добавили ante в pot
                    GameScreenForm gameScreenForm = new GameScreenForm();
                    gameScreenForm.Show();
                    Hide();
                }
            }
            
        }

        private int GetCheckedCheckboxCount() //Получение количесвтва игроков которые внесли ante
        {
            int count = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Ante"].Value))
                {
                    count++;
                }
            }
            return count;
        }

        private List<(string, int, bool)> GetDataFromDataGridView() //Получение данных об игроках из dataGridView
        {
            List<(string Name, int Bank, bool Ante)> playerData = new List<(string, int, bool)>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string name = row.Cells["NamePlayer"].Value?.ToString() ?? "Игрок"; //если не введено имя, то вставится "Игрок"
                    int bank = int.TryParse(row.Cells["Bank"].Value?.ToString(), out int parsedBank) ? parsedBank : 10000; //если неверно введен банк, то вставится "10000"
                    bool ante = bool.TryParse(row.Cells["Ante"].Value?.ToString(), out bool parsedAnte) && parsedAnte;
                    playerData.Add((name, bank, ante));
                }
            }
            return playerData;
        }

        public void PrintDataPlayer()
        {
            dataGridView1.Rows.Clear();
            foreach (Player player in Game.Instance.Players)
            {
                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].Cells[0].Value = player.Name;
                dataGridView1.Rows[rowIndex].Cells[1].Value = player.Bank;
                if (player.Status == true)
                {
                    dataGridView1.Rows[rowIndex].Cells[2].Value = true;
                }
                else
                {
                    dataGridView1.Rows[rowIndex].Cells[2].Value = false;
                }
                
            }
        }
    }
}
