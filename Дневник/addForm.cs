using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Дневник
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        #region ButtonsClick
        private void addTodayDayButton_Click(object sender, EventArgs e)
        {
            nameTextBox.Text += DateTime.Now;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var connection = new SQLiteConnection("Data Source=SaveDiary.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {Model.Login} (name) VALUES(:newName)";
            command.Parameters.AddWithValue("newName", nameTextBox.Text); //Можно было просто выше написать вместо :newName, но можно и так

            command.ExecuteNonQuery();

            Close();
        }
        #endregion
    }
}
