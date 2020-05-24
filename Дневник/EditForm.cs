using System.Data.SQLite;
using System.Windows.Forms;

namespace Дневник
{
    public partial class EditForm : Form
    {
        public EditForm()
        {
            InitializeComponent();
            UpdateValues();
        }

        private void UpdateValues()
        {
            var connection = new SQLiteConnection("Data Source=SaveDiary.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {Model.Login} WHERE (name)=(:name)";
            command.Parameters.AddWithValue("name", Model.SelectedItem); // Если без этого написать, то если в значении будет точка оно выдаст ошибку, а так не выдает.

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                nameTextBox.Text = reader["name"].ToString();
                richTextBox.Text = reader["text"].ToString();
            }

            reader.Close();
        }

        #region ButtonsClick
        private void saveButton_Click(object sender, System.EventArgs e)
        {
            var connection = new SQLiteConnection("Data Source=SaveDiary.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"UPDATE {Model.Login} " +
                                  "SET (name)=(:newName), (text)=(:newText) " +
                                  $"WHERE (name)=(:selectedItem)";
            command.Parameters.AddWithValue("newName", nameTextBox.Text);
            command.Parameters.AddWithValue("newText", richTextBox.Text);
            command.Parameters.AddWithValue("selectedItem", Model.SelectedItem);

            command.ExecuteNonQuery();

            Close();
        }

        private void deleteButton_Click(object sender, System.EventArgs e)
        {
            var connection = new SQLiteConnection("Data Source=SaveDiary.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {Model.Login} WHERE name=:selectedItem";
            command.Parameters.AddWithValue("selectedItem", Model.SelectedItem);

            command.ExecuteNonQuery();

            Close();
        }
        #endregion
    }
}
