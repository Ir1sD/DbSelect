using DbSelect.Controllers;
using DbSelect.Helpers;
using DbSelect.StaticModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Forms
{
    public partial class DataForm : Form
    {
        public DataForm()
        {
            InitializeComponent();
            textBox1.UseSystemPasswordChar = true;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (Library.IsPasswordAdmin(textBox1.Text))
                {
                    SystemController.ChangeAdminStatus(true);
                    MessageHelper.InfoMessage("Вход выполнен");
                    this.Hide();
                }
                else
                {
                    MessageHelper.ErrorMessage("Ошибка при авторизации");
                    this.Hide();
                }
            }
            else if(e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }
    }
}
