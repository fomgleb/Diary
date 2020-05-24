using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Дневник
{
    public partial class ListForm : Form
    {
        public ListForm()
        {
            InitializeComponent();
            UpdateListBox();
        }

        #region Clicks
        private void addButton_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm();
            addForm.ShowDialog();
            UpdateListBox();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (elementListBox.SelectedItem != null)
            {
                Model.SelectedItem = elementListBox.SelectedItem.ToString(); //В свойство Item в классе SelectedItem добавляем текст выбраного элемента в списке
                EditForm editForm = new EditForm(); //Создаем экземпляр формы EditForm
                editForm.ShowDialog(); //Открываем её
                UpdateListBox();
            }
            else
                MessageBox.Show("Выбери элемент из списка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void elementListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && elementListBox.SelectedItem != null)
            {
                var connection = new SQLiteConnection("Data Source=SaveDiary.db");
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM :tableName WHERE name=:selectedItem";
                command.Parameters.AddWithValue("tableName", Model.Login);
                command.Parameters.AddWithValue("selectedItem", elementListBox.SelectedItem.ToString());

                command.ExecuteNonQuery();

                UpdateListBox();
            }
        }

        private void elementListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (elementListBox.SelectedItem != null)
                editButton_Click(editButton, null);
        }

        private void changePasswordButton_Click(object sender, EventArgs e)
        {
            ChangePasswordForm changePasswordForm = new ChangePasswordForm();
            changePasswordForm.ShowDialog();
        }

        private void deleteAccountButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы уверены что хотите удалить", "Сообщение", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
                return;
            else if (dialogResult == DialogResult.Yes)
            {
                var connection = new SQLiteConnection("Data Source=SaveDiary.db");
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"DROP TABLE {Model.Login}";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM UsersTable WHERE login=:login";
                command.Parameters.AddWithValue("login", Model.Login);
                command.ExecuteNonQuery();
                Close();
            }
        }
        #endregion

        private void UpdateListBox()
        {
            var connection = new SQLiteConnection("Data Source=SaveDiary.db"); //Создаем подключение к базе SaveDiary.db
            connection.Open(); // Открываем подключение

            var command = connection.CreateCommand();
            command.CommandText =   $@"CREATE TABLE IF NOT EXISTS [{Model.Login}](
                                    [id] integer not null primary key autoincrement,
                                    [name] nvarchar(2048) null,
                                    [text] text null);"; //Создаем таблицу если её нету
            command.ExecuteNonQuery();

            elementListBox.Items.Clear(); //Удаляем все элементы из elementListBox

            command = connection.CreateCommand(); //Создаем новую комманду для базы данных
            command.CommandText = $"SELECT (name) FROM {Model.Login}"; // Записываем комманду в command

            var reader = command.ExecuteReader(); // Отправляем команду command в базу данных и данные которые пришли записываем в переменную reader
            List<string> items = new List<string>();
            while (reader.Read()) // Читаем данные которые пришли из базы данных
                items.Add(reader["name"].ToString()); //Добавляем в список все названия, чтобы потом можно было его перевернуть

            items.Reverse(); //Переворачиваем список
            foreach (var item in items) //Добавляем все элементы из списка в listBox
                elementListBox.Items.Add(item);

            reader.Close();
        }

        
    }
}
