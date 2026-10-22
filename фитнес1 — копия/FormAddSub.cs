using System;
using System.Windows.Forms;

namespace Фитнес
{
    public partial class FormAddSub : Form
    {
        public FormAddSub()
        {
            InitializeComponent();

            dtpStart.Value = DateTime.Today;

            cmbType.SelectedIndexChanged += (s, e) => UpdateEndDate();
            dtpStart.ValueChanged += (s, e) => UpdateEndDate();

            UpdateEndDate();
        }

        private void UpdateEndDate()
        {
            if (string.IsNullOrEmpty(cmbType.Text)) return;

            var start = dtpStart.Value.Date;
            var end = cmbType.Text.StartsWith("Годовой") ? start.AddYears(1) : start.AddMonths(1);
            var status = DateTime.Today <= end ? "активно" : "неактивен";

            lblEndDate.Text = $"{end:dd.MM.yyyy} ({status})";
            lblEndDate.ForeColor = status == "активно" ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdClient.Text, out int id))
            {
                MessageBox.Show("Введите корректный числовой ID клиента.");
                return;
            }

            if (string.IsNullOrEmpty(cmbType.Text))
            {
                MessageBox.Show("Выберите тип абонемента.");
                return;
            }

            var start = dtpStart.Value.Date;
            var end = cmbType.Text.StartsWith("Годовой") ? start.AddYears(1) : start.AddMonths(1);
            var status = DateTime.Today <= end ? "активно" : "неактивен";

            try
            {
                DbHelper.Execute(
                    "INSERT INTO [Абонементы] ([id_клинета], [тип_абонемента], [начало], [конец], [статус]) VALUES (?, ?, ?, ?, ?)",
                    id, cmbType.Text, start, end, status);

                MessageBox.Show("Абонемент успешно добавлен!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}