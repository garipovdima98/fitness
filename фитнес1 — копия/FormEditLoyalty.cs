using System;
using System.Data;
using System.Windows.Forms;

namespace Фитнес
{
    public partial class FormEditLoyalty : Form
    {
        private int _cardId; 

        public FormEditLoyalty(int cardId)
        {
            InitializeComponent();
            _cardId = cardId;
        }

        private void FormEditLoyalty_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DbHelper.Query("SELECT * FROM [Карта лояльности] WHERE [id_карты] = ?", _cardId);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    string fullCardNum = row["номер карты"].ToString();
                    txtCardNum.Text = fullCardNum.Replace("CARD-", "").Trim();

                    txtPoints.Text = row["баллов"].ToString();
                    txtDiscount.Text = row["скидка"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки карты: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCardNum.Text))
            {
                MessageBox.Show("Введите номер карты (только цифры).");
                return;
            }

            if (!int.TryParse(txtPoints.Text, out int points) || !int.TryParse(txtDiscount.Text, out int discount))
            {
                MessageBox.Show("Баллы и скидка должны быть числами.");
                return;
            }

            try
            {
                string finalCardNum = "CARD-" + txtCardNum.Text.Trim();

                    
                DbHelper.Execute(
                    "UPDATE [Карта лояльности] SET [номер карты] = ?, [баллов] = ?, [скидка] = ? WHERE [id_карты] = ?",
                    finalCardNum, points, discount, _cardId);

                MessageBox.Show("Карта обновлена!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}