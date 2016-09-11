using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KrusovayaBD
{
    public partial class Orders : Form
    {
        public KrusovayaBD.Data.ПродуктыDataTable RemoveDuplicateRows(KrusovayaBD.Data.ПродуктыDataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            return dTable;
        }

        DataTableAdapters.Таблица_Заказы Заказы = new DataTableAdapters.Таблица_Заказы();
        Data.ЗаказыDataTable tempЗаказы = new Data.ЗаказыDataTable();

        DataTableAdapters.Таблица_Продукты Продукты = new DataTableAdapters.Таблица_Продукты();
        Data.ПродуктыDataTable tempПродукты = new Data.ПродуктыDataTable();
        Data.ПродуктыDataTable tempПродукты2 = new Data.ПродуктыDataTable();

        public Orders()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            Заказы.FillBy(tempЗаказы);
            dataGridView1.DataSource = tempЗаказы;

            dataGridView1.Columns["id"].DataPropertyName = "id";
            dataGridView1.Columns["count"].DataPropertyName = "count";
            dataGridView1.Columns["state"].DataPropertyName = "state";

            tempПродукты = Продукты.GetDataBy1();
            tempПродукты = RemoveDuplicateRows(tempПродукты, "name");
            tempПродукты2 = Продукты.GetDataBy();

            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dataGridView1.Columns["name"];
            name.ValueMember = tempЗаказы.Columns["name"].ColumnName;
            name.DataPropertyName = "name";
            name.DisplayMember = "name";
            name.Items.Clear();
            foreach (DataRow item in tempПродукты.Rows)
            {
                name.Items.Add(item["name"].ToString());
            }

            DataGridViewComboBoxColumn version = (DataGridViewComboBoxColumn)dataGridView1.Columns["version"];
            version.ValueMember = tempЗаказы.Columns["version"].ColumnName;
            version.DataPropertyName = "version";
            version.DisplayMember = "version";
            version.Items.Clear();
            foreach (DataRow item in tempПродукты2.Rows)
            {
                version.Items.Add(item["version"].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in tempЗаказы.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    row["product_id"] = Продукты.GetDataBy3(row["version"].ToString(), row["name"].ToString())[0]["id"];
                    if (row["state"].ToString() == "Не сохранено")
                    {
                        Продукты.UpdateQuery(int.Parse(row["count"].ToString()), row["name"].ToString(), row["version"].ToString());
                    }
                    if (row["state"].ToString() != "Отгружено") row["state"] = "Сохранено";
                }
            }
            Заказы.Update(tempЗаказы);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                tempПродукты2 = Продукты.GetByName(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                DataGridViewComboBoxCell version = (DataGridViewComboBoxCell)dataGridView1[1, e.RowIndex];

                version.Items.Clear();
                foreach (DataRow item in tempПродукты2.Rows)
                {
                    version.Items.Add(item["version"].ToString());
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message.ToString());
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex != -1)
            {
                int count = 0;
                int count_exists = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].RowIndex != dataGridView1.Rows.Count - 1)
                    {
                        if (row.Cells[0].Value.ToString() == dataGridView1[0, e.RowIndex].Value.ToString() && row.Cells[1].Value.ToString() == dataGridView1[1, e.RowIndex].Value.ToString() && row.Cells[4].Value.ToString() == "Не сохранено")
                        {
                            count += int.Parse(row.Cells[2].Value.ToString());
                        }
                    }
                }

                count_exists = int.Parse(Продукты.GetDataBy2(dataGridView1[0, e.RowIndex].Value.ToString(), dataGridView1[1, e.RowIndex].Value.ToString())[0]["count"].ToString());

                if (count_exists < count) {
                    MessageBox.Show("Недопустимое количество товара (На складе: " + count_exists.ToString() + ", Заказано:" + count);
                    dataGridView1[2, e.RowIndex].Value = 0;
                }
            }
        }

        private void Orders_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show(this, "Вы закончили редактирование заказа?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (res != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() == "Сохранено")
                Продукты.UpdateQuery1(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Не сохранено";
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Продукты.UpdateQuery1(int.Parse(e.Row.Cells[2].Value.ToString()), e.Row.Cells[0].Value.ToString(), e.Row.Cells[1].Value.ToString());
        }
    }
}
