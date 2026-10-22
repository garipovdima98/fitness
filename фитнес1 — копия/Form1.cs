using System;
using System.Windows.Forms;

namespace Фитнес
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SubscriptionsHelper.ОбновитьСтатусы();
            LoadAllData();
        }

        public void LoadAllData()
        {
            try
            {
                dgvClients.DataSource = DbHelper.Query("SELECT * FROM [Клиенты]");
                dgvTrainers.DataSource = DbHelper.Query("SELECT * FROM [Тренеры]");
                dgvAdmins.DataSource = DbHelper.Query("SELECT * FROM [Администраторы]");
                dgvSubscriptions.DataSource = DbHelper.Query("SELECT * FROM [Абонементы]");
                dgvWorkouts.DataSource = DbHelper.Query("SELECT * FROM [Тренировки]");
                dgvLoyalty.DataSource = DbHelper.Query("SELECT * FROM [Карта лояльности]");

                if (dgvWorkouts.Columns.Contains("время"))
                {
                    dgvWorkouts.Columns["время"].DefaultCellStyle.Format = "t";
                }

                if (dgvWorkouts.Columns.Contains("дата"))
                {
                    dgvWorkouts.Columns["дата"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridView currentGrid = null;
            string tableName = "";
            string pkColumn = "";

            switch (tabControl1.SelectedTab.Text)
            {
                case "Клиенты":
                    currentGrid = dgvClients; tableName = "Клиенты"; pkColumn = "id_клиента"; break;
                case "Тренеры":
                    currentGrid = dgvTrainers; tableName = "Тренеры"; pkColumn = "id_тренера"; break;
                case "Администраторы":
                    currentGrid = dgvAdmins; tableName = "Администраторы"; pkColumn = "id_администратор"; break;
                case "Абонементы":
                    currentGrid = dgvSubscriptions; tableName = "Абонементы"; pkColumn = "id_абонимента"; break;
                case "Тренировки":
                    currentGrid = dgvWorkouts; tableName = "Тренировки"; pkColumn = "id_тренировки"; break;
                case "Карты лояльности":
                    currentGrid = dgvLoyalty; tableName = "Карта лояльности"; pkColumn = "id_карты"; break;
                default: return;
            }

            if (currentGrid.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для удаления.");
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(currentGrid.CurrentRow.Cells[pkColumn].Value);

                    DbHelper.Execute($"DELETE FROM [{tableName}] WHERE [{pkColumn}] = ?", id);

                    LoadAllData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string q = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(q))
            {
                LoadAllData(); 
                return;
            }

            try
            {
                string query = "%" + q + "%";

                switch (tabControl1.SelectedTab.Text)
                {
                    case "Клиенты":
                        dgvClients.DataSource = DbHelper.Query("SELECT * FROM [Клиенты] WHERE фамилия LIKE ? OR имя LIKE ? OR телефон LIKE ?", query, query, query);
                        break;
                    case "Тренеры":
                        dgvTrainers.DataSource = DbHelper.Query("SELECT * FROM [Тренеры] WHERE фамилия LIKE ? OR специальность LIKE ?", query, query);
                        break;
                    case "Администраторы":
                        dgvAdmins.DataSource = DbHelper.Query("SELECT * FROM [Администраторы] WHERE фамилия LIKE ?", query);
                        break;
                    case "Абонементы":
                        dgvSubscriptions.DataSource = DbHelper.Query("SELECT * FROM [Абонементы] WHERE тип_абонемента LIKE ? OR статус LIKE ?", query, query);
                        break;
                    case "Тренировки":
                        dgvWorkouts.DataSource = DbHelper.Query("SELECT * FROM [Тренировки] WHERE дата LIKE ?", query);
                        break;
                    case "Карты лояльности":
                            dgvLoyalty.DataSource = DbHelper.Query(
                            "SELECT * FROM [Карта лояльности] WHERE [номер карты] LIKE ? OR [номер карты] LIKE ?",
                            "%" + q + "%", "%CARD-" + q + "%");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Text)
            {
                case "Клиенты":
                    new FormAddClient().ShowDialog();
                    break;
                case "Тренеры":
                    new FormAddTrainer().ShowDialog();
                    break;
                case "Администраторы":
                    new FormAddAdmin().ShowDialog();
                    break;
                case "Абонементы":
                    new FormAddSub().ShowDialog();
                    break;
                case "Тренировки":
                    new FormAddWorkout().ShowDialog();
                    break;
            }
            LoadAllData();
        }


        private void dgvLoyalty_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex < 0) return;

            int cardId = Convert.ToInt32(dgvLoyalty.Rows[e.RowIndex].Cells["id_карты"].Value);

            FormEditLoyalty editForm = new FormEditLoyalty(cardId);
            editForm.ShowDialog();

            LoadAllData();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
}