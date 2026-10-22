using System;
using System.Windows.Forms;

namespace Фитнес
{
    public partial class FormAddClient : Form
    {
        public FormAddClient()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLast.Text) || string.IsNullOrWhiteSpace(txtFirst.Text))
            {
                MessageBox.Show("Введите фамилию и имя.");
                return;
            }

            try
            {
                string cardNum = "CARD-" + new Random().Next(1000, 9999);
                DbHelper.Execute("INSERT INTO [Карта лояльности] ([номер карты], [баллов], [скидка]) VALUES (?, 0, 0)", cardNum);

                int idCard = Convert.ToInt32(DbHelper.Scalar("SELECT MAX(id_карты) FROM [Карта лояльности]"));

                DbHelper.Execute("INSERT INTO [Клиенты] ([фамилия], [имя], [отчество], [телефон], [id_карты]) VALUES (?, ?, ?, ?, ?)",
                    txtLast.Text.Trim(), txtFirst.Text.Trim(), txtMiddle.Text.Trim(), txtPhone.Text.Trim(), idCard);

                MessageBox.Show("Клиент успешно добавлен!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}