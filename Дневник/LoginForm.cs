using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Дневник
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            UpdateLoginComdoBox();
        }

        private void UpdateLoginComdoBox()
        {
            if (!File.Exists("SaveDiary.db")) //Если файла базы данных нету
                SQLiteConnection.CreateFile("SaveDiary.db"); // то создаем его
            var connection = new SQLiteConnection("Data Source=SaveDiary.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS [UsersTable](
                                    [id] integer not null primary key autoincrement,
                                    [login] nvarchar(2048) not null,
                                    [password] nvarchar(2048) not null);"; //Создаем таблицу если её нету
            command.ExecuteNonQuery();

            loginComboBox.Items.Clear();

            command.CommandText = "SELECT (login) FROM UsersTable";
            var reader = command.ExecuteReader();

            while (reader.Read())
                loginComboBox.Items.Add(reader["login"]);
            reader.Close();

            if (loginComboBox.Items.Count == 0)
            {
                RegisterForm registerForm = new RegisterForm();
                registerForm.ShowDialog();
                UpdateLoginComdoBox();
            }
        }

        #region ButtonsClick
        private void registerButton_Click(object sender, System.EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
            UpdateLoginComdoBox();
        }

        private void loggedButton_Click(object sender, System.EventArgs e)
        {
            var connection = new SQLiteConnection("Data Source=SaveDiary.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM UsersTable WHERE login=:login";
            command.Parameters.AddWithValue("login", loginComboBox.Text);

            try
            {
                var reader = command.ExecuteReader();
                reader.Read();

                if (passwordTextBox.Text == reader["password"].ToString())
                {
                    Model.Login = reader["login"].ToString();
                    reader.Close();
                    ListForm listForm = new ListForm();
                    Visible = false;
                    listForm.ShowDialog();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
