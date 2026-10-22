using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Фитнес
{
    public partial class FormAddTrainer : Form
    {
        public FormAddTrainer()
        {
            InitializeComponent();
        }

     
        
            private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLast.Text)) { MessageBox.Show("Введите фамилию."); return; }

            try
            {
                DbHelper.Execute("INSERT INTO [Тренеры] ([фамилия], [имя], [отчество], [специальность]) VALUES (?, ?, ?, ?)",
                    txtLast.Text.Trim(), txtFirst.Text.Trim(), txtMiddle.Text.Trim(), txtSpec.Text.Trim());

                MessageBox.Show("Тренер добавлен!");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        
            private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
    
    
}
