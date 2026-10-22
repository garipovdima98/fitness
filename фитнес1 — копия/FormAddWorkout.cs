using System;
using System.Windows.Forms;

namespace Фитнес
{
    public partial class FormAddWorkout : Form
    {
        public FormAddWorkout()
        {
            InitializeComponent();
            dtpDate.Value = DateTime.Today;
            dtpTime.Value = DateTime.Now;
            this.dtpTime.MouseWheel += new MouseEventHandler(dtpTime_MouseWheel);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Проверяем, что ID — это числа
            if (!int.TryParse(txtIdTrainer.Text, out int idTrainer) ||
                !int.TryParse(txtIdClient.Text, out int idClient) ||
                !int.TryParse(txtIdAdmin.Text, out int idAdmin))
            {
                MessageBox.Show("Введите корректные числовые ID.");
                return;
            }

            try
            {
                // Вставляем тренировку
                DbHelper.Execute(
                    "INSERT INTO [Тренировки] ([id_тренера], [id_клиента], [id_администратора], [дата], [время], [забронировано], [оплачено]) VALUES (?, ?, ?, ?, ?, ?, ?)",
                    idTrainer, idClient, idAdmin,
                    dtpDate.Value.Date, dtpTime.Value,
                    chkBooked.Checked, chkPaid.Checked);

                MessageBox.Show("Тренировка добавлена!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        private void dtpTime_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                SendKeys.Send("{UP}");
            }
            else
            {
                SendKeys.Send("{DOWN}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}