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
    public partial class FormAddAdmin : Form
    {
        public FormAddAdmin()
        {
            InitializeComponent();
        }

    
            private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLast.Text)) { MessageBox.Show("Введите фамилию."); return; }

            try
            {
                DbHelper.Execute("INSERT INTO [Администраторы] ([фамилия], [имя], [отчество]) VALUES (?, ?, ?)",
                    txtLast.Text.Trim(), txtFirst.Text.Trim(), txtMiddle.Text.Trim());

                MessageBox.Show("Администратор добавлен!");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

       
            private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
    
    
}
