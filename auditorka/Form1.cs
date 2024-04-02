using auditorka.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace auditorka
{
    public partial class Form1 : Form
    {
        DateTime selectedDate1 = DateTime.Today;
        string stringSelectedDate1 = "";
        DateTime selectedDate2 = DateTime.Today;
        string stringSelectedDate2 = "";
        bool isStartDateConfirmed = false;
        string savedActions= "";
        public Form1()
        {
            InitializeComponent();
            if (!File.Exists("saves.txt"))
                File.WriteAllLines("saves.txt", new string[0]);
            savedActions = File.ReadAllText("saves.txt");
            if (savedActions.Length > 0)
                label12.Text = label12.Text + "\n" + savedActions;
            using (StringReader reader = new StringReader(savedActions))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    comboBox1.Items.Add(line);
                }
            }

            savedActions = "";
            monthCalendar1.MaxSelectionCount = 1;
            button3.Visible = false;
            button3.Enabled = false;
            button4.Visible = false;
            button4.Enabled = false;
            label11.Visible = false;
            label7.Visible = false;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            label1.Text = "Выбранная дата: ";
            isStartDateConfirmed = false;
                button3.Enabled = true;
                button3.Visible = true;
                selectedDate1 = monthCalendar1.SelectionRange.Start;
                label1.Text += selectedDate1.Day + "/" + selectedDate1.Month + "/" + selectedDate1.Year + "\nПодтвердить дату?";
            if (!isStartDateConfirmed)
            {
                button4.Enabled = false;
                button4.Visible = false;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            label2.Text = "Выбранная дата: ";
            if (monthCalendar2.SelectionRange.Start < selectedDate1)
            {
                label2.Text = "Выбранная дата не может быть\nраньше выбранной";
            }
            else
            {
                selectedDate2 = monthCalendar2.SelectionRange.Start;
                label2.Text += selectedDate2.Day + "/" + selectedDate2.Month + "/" + selectedDate2.Year + "\nПодтвердить дату?";
                if (isStartDateConfirmed)
                {
                    button4.Enabled = true;
                    button4.Visible = true;
                }
                else 
                {
                    button4.Enabled = false;
                    button4.Visible = false;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isStartDateConfirmed = true;
            if (selectedDate2 >= selectedDate1)
            {
                button4.Enabled = true;
                button4.Visible = true;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (StringReader reader = new StringReader(label12.Text))
            {
                string line;
                DateTime date = DateTime.Now;
                int index = 0, i = 0;
                bool isFirst = true;
                while ((line = reader.ReadLine()) != null)
                {
                    index += line.Length;
                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }
                    index++;
                    if (line[line.Length - 1] == '!')
                        continue;
                    i = line.IndexOf(" ") + 1;
                    line = line.Substring(i);
                    i = line.IndexOf(" ") + 3;
                    line = line.Substring(i);
                    date = DateTime.ParseExact(line, "dd/MM/yyyy", null);
                    if (date < DateTime.Today)
                        label12.Text = label12.Text.Insert(index," Просрочено!");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedTab = tabPage5;
        }

        private void monthCalendar2_DateChanged_1(object sender, DateRangeEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            label11.Visible = false;

            stringSelectedDate1 = selectedDate1.ToString("dd/MM/yyyy");
            stringSelectedDate2 = selectedDate2.ToString("dd/MM/yyyy");

            if (textBox1.Text.Length > 0 && textBox1.Text.IndexOf(" ") == -1) 
            {
                savedActions += textBox1.Text + " " + stringSelectedDate1 + " - " + stringSelectedDate2;
                label12.Text += "\n" + savedActions;
                tabControl1.SelectedTab = tabPage1;
                tabControl2.SelectedTab = tabPage4;
                comboBox1.Items.Add(savedActions);
            }
            else
            {
                label11.Visible = true;
            }
            savedActions = "";
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            tabControl2.SelectedTab = tabPage4;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            label7.Visible = false;
            int length = 0;
            int index = 0;
            if (radioButton2.Checked && comboBox1.SelectedItem != null)
            {
                index = label12.Text.IndexOf(comboBox1.SelectedItem.ToString());
                for (int i = index; i < label12.Text.Length; i++)
                    if (i == label12.Text.Length - 1 || label12.Text[i] == '\n')
                    {
                        length = i - index + 1;
                        if (label12.Text[index - 2] == ':' && i != label12.Text.Length - 1)
                        {
                            index++;
                            length--;
                        }
                        else if (i != label12.Text.Length - 1)
                            length--;

                        break;
                    }
                label12.Text = label12.Text.Remove(index - 1, length + 1);
                tabControl1.SelectedTab = tabPage1;
                comboBox1.Items.Remove(comboBox1.SelectedItem);
                comboBox1.SelectedItem = null;
            }
            else if (radioButton1.Checked && comboBox1.SelectedItem != null)
            {
                index = label12.Text.IndexOf(comboBox1.SelectedItem.ToString());
                for (int i = index; i < label12.Text.Length; i++)
                {
                    if (label12.Text[i] == '\n')
                    {
                        label12.Text = label12.Text.Insert(i, " Завершено!");
                        break;
                    }
                    if (i == label12.Text.Length - 1)
                    {
                        label12.Text += " Завершено!";
                        break;
                    }
                }
                tabControl1.SelectedTab = tabPage1;
            }
            else
            {
                label7.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void form1Closing(object sender, FormClosingEventArgs e)
        {
            string text = "";
            int index = 0;
            foreach (char letter in label12.Text)
            {
                index++;
                if (index == label12.Text.Length)
                {
                    index = 0;
                    break;
                }
                if (letter == '\n')
                    break;
            }
            if (index != 0)
            {
                text = label12.Text.Substring(index);
                File.WriteAllText("saves.txt", text);
            }  
        }
    }
}
