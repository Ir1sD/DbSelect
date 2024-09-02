using DbSelect.Controllers;
using DbSelect.Enums;
using DbSelect.Helpers;
using DbSelect.Models.ComponentModels;
using DbSelect.Models.StaticModels;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Collections.Generic;
using DbSelect.SystemModels.ComponentModels;
using System.Linq;

namespace DbSelect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartCm();
            Start();
        }

        private void Start()
        {
            timeTimer.Start();
            SystemController.ChangeAdminStatus(false);

            Time.Text = DateTime.Now.ToLongTimeString();
            SystemController.UpdateInfoTree();
            label33.Text = string.Empty;
            label35.Text = string.Empty;
            SystemHelper.PullIconsList();

            tabControl2.Appearance = TabAppearance.FlatButtons;
            tabControl2.ItemSize = new Size(0, 1);
            tabControl2.SizeMode = TabSizeMode.Fixed;

            button18.Enabled = false;

            LocatePanelStartPage();
            Cm.StartPage.LoadConfPanel.Visible = checkBox2.Checked;
            Cm.StartPage.SaveConfPanel.Visible = checkBox3.Checked;
            Cm.StartPage.ConnectionLabel.Visible = Cm.StartPage.ConnectionText.Visible = Cm.StartPage.ConnectionBut.Visible = Cm.StartPage.QueryText.Visible = checkBox4.Checked || checkBox5.Checked;

            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;



        }

        private void StartCm()
        {
            Cm.System = new CmSystem
            {
                Menu = treeView1,
                Tab = tabControl1,
                MenuTable = StripTable,
                MenuTables = StripTables,
                MenuFiles = StripFiles,
                MenuFile = StripFile,
                SaveConfig = сохранитьКонфигурациюToolStripMenuItem,
                AdminPanel = дляРазработчикаToolStripMenuItem,
                ErrorList = списокОшибокToolStripMenuItem,
                ErrorListData = dataGridView1,
                ErrorListText = textBox9
            };

            Cm.CreateTable = new CmCreateTable
            {
                d = DataGridCreateTable,
                Name = NameTable
            };

            Cm.Filling = new CmFilling
            {
                Tables = comboBox1,
                Columns = listBox1,
                Tab = tabControl2,
                bDown = button14,
                bUp = button15,
                CountText = textBox10,
                ConditionText = textBox6,
                ConditionList = listBox2,
                Type = label38,

                IntConst = new CmFillingComponent.IntConst { Value = numericUpDown1},
                IntRand = new CmFillingComponent.IntRand {ValueFrom = numericUpDown2 , ValueTo = numericUpDown3 , R1 = radioButton1 , R2 = radioButton2 , R3 = radioButton3},
                IntIdConst = new CmFillingComponent.IntIdConst { Table = comboBox2 , Column = comboBox3},
                IntIdRand = new CmFillingComponent.IntIdRand { Table = comboBox4 , Column = comboBox5 },
                StringConst = new CmFillingComponent.StringConst {Value = textBox4},
                StringFile = new CmFillingComponent.StringFile { File = comboBox6 , R1 = radioButton6 , R2 = radioButton5 , R3 = radioButton4},
                FloatConst = new CmFillingComponent.FloatConst { Value = numericUpDown4},
                FloatRand = new CmFillingComponent.FloatRand { ValueFrom = numericUpDown5 , ValueTo = numericUpDown6 , R1 = radioButton9 , R2 = radioButton8 , R3 = radioButton7},
                ImgConst = new CmFillingComponent.ImgConst { File = comboBox7},
                ImgFile = new CmFillingComponent.ImgFile { File = comboBox8 },
                DateConst = new CmFillingComponent.DateConst { Value = dateTimePicker1 },
                DateRand = new CmFillingComponent.DateRand { ValueFrom = dateTimePicker2 , ValueTo = dateTimePicker3 , R1 = radioButton12 , R2 = radioButton11 , R3 = radioButton10},
                BoolConst = new CmFillingComponent.BoolConst { Value = checkBox1 }
                
            };

            Cm.CreateFile = new CmCreateFile
            {
                FileStringName = textBox1,
                FileStringValues = textBox2,
                FileStringFileName = label33,
                FileStringAction = label32,
                FileImgName = textBox3,
                FileImgValue = pictureBox2,
                FileImgFileName = label35,
                FileGroupButtonNext = button13,
                FileGroupButtonPrev = button12,
                FileGroupName  = textBox5,
                FileGroupValues = new PictureBox[]
                {
                    pictureBox3 , pictureBox4 , pictureBox5 , pictureBox6 , pictureBox10 , pictureBox9 , pictureBox8 , pictureBox7 , pictureBox14 , pictureBox13 , pictureBox12 , pictureBox11
                }
            };

            Cm.StartPage = new CmStart()
            {
                GlPanel = panel6,
                LoadConfCheck = checkBox2,
                LoadConfPanel = panel7,
                SaveConfCheck = checkBox3,
                SaveConfPanel = panel8,
                CreateTableCheck = checkBox4,
                FillingCheck = checkBox5,
                ConnectionLabel = label42,
                ConnectionText = textBox8,
                ConnectionBut = button22,
                Tab = tabControl3,
                List = listBox3,
                Text = textBox7,
                LoadConfLabel = label40,
                SaveConfLabel = label41,
                QueryText = checkBox6
            };
        }

        // Таймер
        private void timeTimer_Tick(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToLongTimeString();
            //Time.Text = User.SelectedTableId.ToString() + " " + User.SelectedColumnId.ToString() + " " + User.SelectedFileId.ToString();
        }
        
        // При нажатии на "Создать таблицу"
        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigationHelper.CreateTable();
        }

        // При нажатии на "Удалить таблицу"
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User.SelectedTableId = TableHelper.GetTableByName(SystemHelper.GetSelectedItem().Text).Id;
            SystemController.DeleteTable();
        }

        // При нажатии ПКМ выделять элемент treeView
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                treeView1.SelectedNode = e.Node;
        }

        // Обновления типа автозаполнения
        private void DataGridCreateTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                TableController.UpdateFillingType(e.RowIndex);
            }
        }

        // Ошибка при изменении типа автозаполнения
        private void DataGridCreateTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //////
        }

        // При нажатии на "Автозаполнение", на владке Таблицы
        private void автозаполнениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigationHelper.Filling();
        }

        // Вывод столбцов в автозаполнении
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TableController.PullColumnsList();
        }

        // При нажатии на "Автозаполнение"
        private void автозаполнениеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var table = TableHelper.GetTableByName(SystemHelper.GetSelectedItem().Text);
            NavigationHelper.Filling(table.Id);
        }

        // Быстрая работа с DataGridView
        private void DataGridCreateTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewCell cell = DataGridCreateTable.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewComboBoxCell)
                {
                    DataGridCreateTable.BeginEdit(false);
                    (DataGridCreateTable.EditingControl as DataGridViewComboBoxEditingControl).DroppedDown = true;
                }
                else
                {
                    DataGridCreateTable.BeginEdit(false);
                }
            }
        }

        // Открытие нужного окна автозаполнения
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TableController.OpenWindowFilling();
        }

        // При нажатии на "Проект" в таблице
        private void проектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var table = TableHelper.GetTableByName(SystemHelper.GetSelectedItem().Text);
            NavigationHelper.UpdateTable(table.Id);
        }

        // При выборе таблицы в автозаполнении 
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TableController.PullColumnsListIdFilling(1);
        }

        // При выборе таблицы в автозаполнении 
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            TableController.PullColumnsListIdFilling(2);
        }

        // При нажатии на "Сохранить" при создании или изменении таблицы
        private void button1_Click(object sender, EventArgs e)
        {
            if (User.SelectPage == Enums.PagesEnum.CreateTable)
            {
                SystemController.CreateTable();
            }
            else
                SystemController.UpdateTable();
        }

        // При нажатии на "Выход" при создании или изменении таблицы
        private void button2_Click(object sender, EventArgs e)
        {
            NavigationHelper.StartWindow(true);
        }

        // При нажатии на "Сохранить" в автозаполнении
        private void button3_Click(object sender, EventArgs e)
        {
            if(TableController.CheckUpdateFilling())
            {
                User.SelectedTableId = -1;

                if (!User.CorrectionErrorStatus)
                {
                    NavigationHelper.StartWindow(true);
                }
                else
                {
                    User.CorrectionErrorStatus = false;
                    NavigationHelper.StartA(true);
                }
            }
        }

        private void TabCreateTable_Click(object sender, EventArgs e)
        {

        }

        // Загрузка текстового файла
        private void label32_Click(object sender, EventArgs e)
        {
            FileController.PushFileString();
        }

        // При нажатии на создать файл текста
        private void текстовыйФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigationHelper.File(FileTypeEnum.String);
        }

        // Сохранение тексового файла
        private void button5_Click(object sender, EventArgs e)
        {
            SystemController.CreateFileString();
        }

        // Выход с страницы текстового файла
        private void button4_Click(object sender, EventArgs e)
        {
            NavigationHelper.StartWindow(true);
        }

        // Удаление файла
        private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FileController.DeleteFile();
        }

        // Редактирование файла
        private void проектToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var file = FileHelper.GetFileByName(SystemHelper.GetSelectedItem().Text);
            NavigationHelper.EditFile(file.Id);
        }

        // Создание файл изображения
        private void изображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigationHelper.File(FileTypeEnum.Image);
        }

        // Загрузка изображения
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            FileController.FilePullImage();
        }

        // Сохранить файл картинку
        private void button8_Click(object sender, EventArgs e)
        {
            SystemController.CreateFileImg();
        }

        // Выйти из файла Картинка
        private void button7_Click(object sender, EventArgs e)
        {
            NavigationHelper.StartWindow(true);
        }

        // Загрузить файлы в группу изображений
        private void button11_Click(object sender, EventArgs e)
        {
            FileController.FileGroupPullList();
        }

        // Список изображений - далее
        private void button13_Click(object sender, EventArgs e)
        {
            FileController.FileGroupPullData();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            User.SelectedImageFromFile -= 24;
            FileController.FileGroupPullData();
        }

        // Создание группы изображений
        private void группаИзображенийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigationHelper.File(FileTypeEnum.ImageList);
        }

        // Сохранить группу картинок
        private void button10_Click(object sender, EventArgs e)
        {
            SystemController.CreateFileGroup();
        }

        // Выйти из группы картинок
        private void button9_Click(object sender, EventArgs e)
        {
            NavigationHelper.StartWindow(true);
        }

        private void сохранитьКонфигурациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigationHelper.SaveConf();
        }

        private void загрузитьКонфигурациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigationHelper.SelectConf();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            TableController.FiilingUpdateStatusButton(2);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            TableController.FiilingUpdateStatusButton(1);
        }

        private void загрузитьТестовоеЗаполнениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationController.FastConfSelect();
        }

		private void удалитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
		{
            TableController.DeleteTablesList();
		}

		private void удалитьВсеToolStripMenuItem1_Click(object sender, EventArgs e)
		{
            FileController.DeleteFilesList();
		}

		private void button6_Click(object sender, EventArgs e)
		{
            NavigationHelper.StartWindow(true);
		}

		private void button16_Click(object sender, EventArgs e)
		{
            TableController.AddConditionInList();
		}

		private void button17_Click(object sender, EventArgs e)
		{
            TableController.DeleteCondition();
		}

		private void запускToolStripMenuItem_Click(object sender, EventArgs e)
		{
            NavigationHelper.StartAction();
		}

		private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
		{
            SystemController.UpdateInfoTree();
		}

		private void NameTable_KeyDown(object sender, KeyEventArgs e)
		{
            if(e.KeyCode == Keys.F3)
            {
                SystemHelper.ChangeTextBoxText(NameTable);
            }
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3)
			{
				SystemHelper.ChangeTextBoxText(textBox1);
			}
		}

		private void textBox3_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3)
			{
				SystemHelper.ChangeTextBoxText(textBox3);
			}
		}

		private void textBox5_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F3)
			{
				SystemHelper.ChangeTextBoxText(textBox5);
			}
		}

        private void LocatePanelStartPage()
        {
            if(!Cm.StartPage.CreateTableCheck.Checked && !Cm.StartPage.LoadConfCheck.Checked && !Cm.StartPage.SaveConfCheck.Checked && !Cm.StartPage.FillingCheck.Checked)
            {
                Cm.StartPage.GlPanel.Size = new Size(699, 29);
                Cm.StartPage.Tab.Visible = false;
            }
            else
            {
                Cm.StartPage.GlPanel.Size = new Size(699, 106);
                Cm.StartPage.Tab.Visible = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Cm.StartPage.LoadConfPanel.Visible = checkBox2.Checked;
            button18.Enabled = false;
            LocatePanelStartPage();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Cm.StartPage.SaveConfPanel.Visible = checkBox3.Checked;
            LocatePanelStartPage();
            button18.Enabled = false;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Cm.StartPage.ConnectionLabel.Visible = Cm.StartPage.ConnectionText.Visible = Cm.StartPage.ConnectionBut.Visible = Cm.StartPage.QueryText.Visible = checkBox4.Checked || checkBox5.Checked;
            LocatePanelStartPage();
            button18.Enabled = false;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Cm.StartPage.ConnectionLabel.Visible = Cm.StartPage.ConnectionText.Visible = Cm.StartPage.ConnectionBut.Visible = Cm.StartPage.QueryText.Visible = checkBox4.Checked || checkBox5.Checked;
            LocatePanelStartPage();
            button18.Enabled = false;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            ConfigurationController.SelectConfLoad();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ConfigurationController.SelectFileConfCreate();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            SqlController.GetConnectionString();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            button18.Enabled = false;
            tabControl3.SelectedIndex = 0;

            if (TableController.CheckTable())
            {
                button18.Enabled = true;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedIndex = 0;

            if (Cm.StartPage.SaveConfCheck.Checked)
            {
                ConfigurationController.CreateConf();
            }

            if(Cm.StartPage.LoadConfCheck.Checked)
            {
                ConfigurationController.SelectConf();
                SystemController.UpdateInfoTree();
            }

            if(Cm.StartPage.CreateTableCheck.Checked)
            {
                SqlController.CreateTables();
            }

            if(Cm.StartPage.FillingCheck.Checked)
            {
                SqlController.FillingTable();
            }

            button18.Enabled = false;
        }

        private void listBox3_Click(object sender, EventArgs e)
        {
            try
            {
                TableController.CorrectionTableError(listBox3.Items[listBox3.SelectedIndex].ToString());
            }
            catch (Exception)
            {

            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void авторизацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(User.IsAdmin)
            {
                SystemController.ChangeAdminStatus(false);
                MessageHelper.InfoMessage("Админ статус сброшен");
            }
            else
            {
                NavigationHelper.Autorization();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                var error = User.ExeptionsList.FirstOrDefault(c => c.Id == Convert.ToInt32(Cm.System.ErrorListData.Rows[e.RowIndex].Cells[0].Value));

                Cm.System.ErrorListText.Text = error.DopInfo + "\n" + error.LogText;
            }
        }

        private void списокОшибокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigationHelper.ErrorList();
        }

        private void быстраяЗагрузкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileController.SelectAllFile();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if(!ValueHelper.IsInt(textBox10.Text))
            {
                textBox10.Text = "0";
            }
        }
    }
}
