using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KrusovayaBD
{
    public partial class Main : Form
    {
        DataTableAdapters.Таблица_Продукты Продукты = new DataTableAdapters.Таблица_Продукты();
        Data.ПродуктыDataTable tempПродукты = new Data.ПродуктыDataTable();

        public Main()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Продукты.Fill(tempПродукты);
            dataGridView1.DataSource = tempПродукты;

            dataGridView1.Columns["name"].DataPropertyName          = tempПродукты.Columns["name"].ColumnName;
            dataGridView1.Columns["version"].DataPropertyName       = tempПродукты.Columns["version"].ColumnName;
            dataGridView1.Columns["description"].DataPropertyName   = tempПродукты.Columns["description"].ColumnName;
            dataGridView1.Columns["count"].DataPropertyName         = tempПродукты.Columns["count"].ColumnName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Продукты.Update(tempПродукты);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Orders o = new Orders();
            o.ShowDialog();
            Продукты.Fill(tempПродукты);
        }
    }
}
