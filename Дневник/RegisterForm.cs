using System.Data.SQLite;
using System.Windows.Forms;

namespace Дневник
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private bool RegisterPerson()
        {
            if (loginTextBox.Text == "" || passwordTextBox.Text == "" || confirmPasswordTextBox.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (passwordTextBox.Text != confirmPasswordTextBox.Text)
            {
                MessageBox.Show("Пароли должны совпадать", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var connection = new SQLiteConnection("Data Source=SaveDiary.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT login FROM UsersTable";

            var reader = command.ExecuteReader();
            while (reader.Read())
                if (loginTextBox.Text == reader["login"].ToString())
                {
                    MessageBox.Show("Такой логин уже зарегистрирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            reader.Close();

            command = connection.CreateCommand();
            command.CommandText = "INSERT INTO UsersTable (login, password) VALUES(:login, :password)";
            command.Parameters.AddWithValue("login", loginTextBox.Text);
            command.Parameters.AddWithValue("password", passwordTextBox.Text);
            command.ExecuteNonQuery();

            return true;
        }

        private void registerButton_Click(object sender, System.EventArgs e)
        {
            bool registerPerson = RegisterPerson();
            if (registerPerson == true)
                Close();
        }
    }
}
