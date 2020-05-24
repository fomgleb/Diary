using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Дневник
{
    public partial class ChangePasswordForm : Form
    {
        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void changePasswordButton_Click(object sender, EventArgs e)
        {
            if (oldPasswordTextBox.Text == "" || newPasswordTextBox.Text == "")
            {
                MessageBox.Show("Заполни все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var connection = new SQLiteConnection("Data Source=SaveDiary.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM UsersTable WHERE login=:login";
            command.Parameters.AddWithValue("login", Model.Login);

            var reader = command.ExecuteReader();
            reader.Read();

            if (reader["password"].ToString() != oldPasswordTextBox.Text)
            {
                MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                reader.Close();
                return;
            }
            reader.Close();

            command.CommandText = "UPDATE UsersTable SET password=:newPassword WHERE password=:oldPassword";
            command.Parameters.AddWithValue("newPassword", newPasswordTextBox.Text);
            command.Parameters.AddWithValue("oldPassword", oldPasswordTextBox.Text);
            command.ExecuteNonQuery();

            MessageBox.Show("Пароль успешно изменён!", "Сообщение", MessageBoxButtons.OK);
            Close();
        }
    }
}
