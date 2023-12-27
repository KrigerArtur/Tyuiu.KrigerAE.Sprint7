using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tyuiu.KrigerAE.Sprint7
{
    public partial class formMainApp_KAE : Form
    {
        private DateTimePicker DateTimePickerDateOfSupply;
        public formMainApp_KAE()
        {
            InitializeComponent();
            this.DateTimePickerDateOfSupply = new DateTimePicker();

            //Adding DateTimePicker control into DataGridView   
            dataGridViewSupply_KAE.Controls.Add(DateTimePickerDateOfSupply);

            // Setting the format (i.e. 2014-10-10)  
            DateTimePickerDateOfSupply.Format = DateTimePickerFormat.Short;

            DateTimePickerDateOfSupply.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }


        private void buttonGoodsOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "csv File|*.csv";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = new DataTable();
                string[] lines = System.IO.File.ReadAllLines(opf.FileName);
                if (lines.Length > 0)
                {
                    //first line to create header
                    string firstLine = lines[0];
                    List<string> validHeaders = new List<string>{ "Code","GoodsName","Quantity","Price","Comments" };
                    string[] headerLabels = firstLine.Split(',');
                    if (!headerLabels.SequenceEqual(validHeaders)) 
                    {
                        MessageBox.Show("Файл не является файлом базы данных товаров приложения");
                        
                        return;
                    }
                    foreach (DataGridViewColumn headerWord in dataGridViewGoods_KAE.Columns)
                    {
                        dt.Columns.Add(new DataColumn(headerWord.Name));
                        
                    }
                    //For Data
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (lines[i] != "")
                        {
                            string[] dataWords = lines[i].Split(',');
                            DataRow dr = dt.NewRow();
                            int columnIndex = 0;
                            foreach (string headerWord in headerLabels)
                            {
                                dr[headerWord] = dataWords[columnIndex++];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                if (dt.Rows.Count >= 0)
                {   
                    bindingSourceGoods_KAE.DataSource = dt;
                    dataGridViewGoods_KAE.Columns[0].DataPropertyName = "Code";
                    dataGridViewGoods_KAE.Columns[1].DataPropertyName = "GoodsName";
                    dataGridViewGoods_KAE.Columns[2].DataPropertyName = "Quantity";
                    dataGridViewGoods_KAE.Columns[3].DataPropertyName = "Price";
                    dataGridViewGoods_KAE.Columns[4].DataPropertyName = "Comments";
                    dataGridViewGoods_KAE.DataSource = bindingSourceGoods_KAE;
                    bindingNavigatorGoods_KAE.BindingSource = bindingSourceGoods_KAE;
                }
            }
        }

        private void buttonGoodsSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "csv File|*.csv";
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dataGridViewGoods_KAE.Columns)
            {

                csv += column.Name + ",";
            }
            csv = csv.Remove(csv.Length - 1);
            //Add new line.
            csv += "\r\n";

            //Adding the Rows
            foreach (DataGridViewRow row in dataGridViewGoods_KAE.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                    {
                        break;
                    }

                    //Add the Data rows.
                    csv += cell.Value.ToString().Replace(",", ";") + ",";
                }

                //Add new line.
                csv = csv.Remove(csv.Length - 1);
                csv += "\r\n";

            }

            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = sfd.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, csv);
        }


        private void checkBoxGoodsFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxGoodsFilter_KAE.Checked == true)
            {
                labelFilterGoodsName_KAE.Enabled = true;
                labelFilterGoodsCode_KAE.Enabled = true;
                labelFilterGoodsQuantity_KAE.Enabled = true;
                labelFilterGoodsPrice_KAE.Enabled = true;
                labelFilterGoodsQuantity_KAE.Enabled = true;
                labelFilterQuantityLess_KAE.Enabled = true;
                labelFilterQuantityMore_KAE.Enabled = true;
                labelFilterPriceLess_KAE.Enabled = true;
                labelFilterPriceMore_KAE.Enabled = true;

                labelFilterQuantityLess_KAE.Visible = true;
                labelFilterQuantityMore_KAE.Visible = true;
                labelFilterPriceLess_KAE.Visible = true;
                labelFilterPriceMore_KAE.Visible = true;

                checkBoxFilterPriceLessEq_KAE.Enabled = true;
                checkBoxFilterPriceMoreEq_KAE.Enabled = true;
                checkBoxFilterQuantityLessEq_KAE.Enabled = true;
                checkBoxFilterQuantityMoreEq_KAE.Enabled = true;

                textBoxFilterGoodsName_KAE.Enabled = true;
                textBoxFilterCode_KAE.Enabled = true;
                textBoxFilterQuantityLess_KAE.Enabled = true;
                textBoxFilterQuantityMore_KAE.Enabled = true;
                textBoxFilterPriceLess_KAE.Enabled = true;
                textBoxFilterPriceMore_KAE.Enabled = true;
                textBoxFilterComments_KAE.Enabled = true;
            }

            if (checkBoxGoodsFilter_KAE.Checked == false)
            {
                labelFilterGoodsName_KAE.Enabled = false;
                labelFilterGoodsCode_KAE.Enabled = false;
                labelFilterGoodsQuantity_KAE.Enabled = false;
                labelFilterGoodsPrice_KAE.Enabled = false;
                labelFilterGoodsComments_KAE.Enabled = false;
                labelFilterQuantityLess_KAE.Enabled = false;
                labelFilterQuantityMore_KAE.Enabled = false;
                labelFilterPriceLess_KAE.Enabled = false;
                labelFilterPriceMore_KAE.Enabled = false;

                labelFilterQuantityLess_KAE.Visible = false;
                labelFilterQuantityMore_KAE.Visible = false;
                labelFilterPriceLess_KAE.Visible = false;
                labelFilterPriceMore_KAE.Visible = false;

                checkBoxFilterPriceLessEq_KAE.Enabled = false;
                checkBoxFilterPriceMoreEq_KAE.Enabled = false;
                checkBoxFilterQuantityLessEq_KAE.Enabled = false;
                checkBoxFilterQuantityMoreEq_KAE.Enabled = false;


                textBoxFilterGoodsName_KAE.Enabled = false;
                textBoxFilterCode_KAE.Enabled = false;
                textBoxFilterQuantityLess_KAE.Enabled = false;
                textBoxFilterQuantityMore_KAE.Enabled = false;
                textBoxFilterPriceLess_KAE.Enabled = false;
                textBoxFilterPriceMore_KAE.Enabled = false;
                textBoxFilterComments_KAE.Enabled = false;

                bindingSourceGoods_KAE.Filter = "";
            }
        }

        private void toolStripTextBoxGoodsSearch_TextChanged(object sender, EventArgs e)
        {
            bindingSourceGoods_KAE.Position = bindingSourceGoods_KAE.Find("GoodsName", toolStripTextBoxGoodsSearch_KAE.Text);
        }

        private void textBoxfilterGoodsName_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods(); ;
        }

        private void textBoxfilterCode_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void textBoxfilterQuantity_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void textBoxfilterPrice_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void textBoxfilterComments_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void textBoxFilterQuantityLess_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }
        private void textBoxFilterQuantityMore_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void textBoxFilterPriceLess_KAE_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }
        private void textBoxFilterPriceMore_KAE_TextChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }
        private void reset_filter_goods()
        {
            List<string> filters = new List<string> { };

            if (textBoxFilterGoodsName_KAE.Text != "" && textBoxFilterGoodsName_KAE.Text != null)
            {
                string filterGoodsName = string.Format("( GoodsName LIKE '%{0}%' )", textBoxFilterGoodsName_KAE.Text);
                filters.Add(filterGoodsName);
            }

            if (textBoxFilterCode_KAE.Text != "" && textBoxFilterCode_KAE.Text != null)
            {

                string filterCode = string.Format("( Code LIKE '%{0}%' )",textBoxFilterCode_KAE.Text);
                filters.Add(filterCode);
            }

            if (textBoxFilterQuantityLess_KAE.Text != "" && textBoxFilterQuantityLess_KAE.Text != null)
            {
                string oper = "<";
                if (checkBoxFilterQuantityLessEq_KAE.Checked == true)
                {
                    oper = "<=";
                }

                string filterQuantityLess = string.Format("( Quantity {0} {1} )", oper , textBoxFilterQuantityLess_KAE.Text);
                filters.Add(filterQuantityLess);
            }

            if (textBoxFilterQuantityMore_KAE.Text != "" && textBoxFilterQuantityMore_KAE.Text != null)
            {
                string oper = ">";
                if (checkBoxFilterQuantityMoreEq_KAE.Checked == true)
                {
                    oper = ">=";
                }

                string filterQuantityMore = string.Format("( Quantity {0} {1} )",oper, textBoxFilterQuantityMore_KAE.Text);
                filters.Add(filterQuantityMore);
            }

            if (textBoxFilterPriceLess_KAE.Text != "" && textBoxFilterPriceLess_KAE.Text != null)
            {
                string oper = "<";
                if (checkBoxFilterPriceLessEq_KAE.Checked == true)
                {
                    oper = "<=";
                }
                string filterPriceLess = string.Format("( Price {0} {1} )",oper, textBoxFilterPriceLess_KAE.Text); 
                filters.Add(filterPriceLess);
            }

            if (textBoxFilterPriceMore_KAE.Text != "" && textBoxFilterPriceMore_KAE.Text != null)
            {
                string oper = ">";
                if (checkBoxFilterPriceMoreEq_KAE.Checked == true)
                {
                    oper = ">=";
                }
                string filterPriceMore = string.Format("( Price {0} {1} )", oper, textBoxFilterPriceMore_KAE.Text); 
                filters.Add(filterPriceMore);
            }

            if (textBoxFilterComments_KAE.Text != "" && textBoxFilterComments_KAE.Text != null)
            {
                string filterComments = string.Format("( Comments Like '%{0}%' )", textBoxFilterComments_KAE.Text);
                filters.Add(filterComments);
            }

            try
            {
                string finalFilter = String.Join(" AND ", filters);
                bindingSourceGoods_KAE.Filter = finalFilter;
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Неверные параметры фильтров. Ошибка: {0}",e.Message));
            }
        }

        private void textBoxfilterGoodsName_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void textBoxfilterQuantity_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (textBoxFilterQuantityLess_KAE.Text != "")
                { 
                int.Parse(textBoxFilterQuantityLess_KAE.Text);
                e.Cancel = false;
                }
            }
            catch {
                MessageBox.Show("Поле принимает только цифры");
                e.Cancel = true;
            }
        }

        private void dataGridViewGoods_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            checkBoxGoodsFilter_KAE.Enabled = true;
        }


        private void checkBoxGoodsFilter_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void buttonCreateSupplyDB_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "csv File|*.csv";
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dataGridViewSupply_KAE.Columns)
            {

                csv += column.Name + ",";
            }
            csv = csv.Remove(csv.Length - 1);
            //Add new line.
            csv += "\r\n";

            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = sfd.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, csv);

        }

        private void buttonCreateGoodsDB_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "csv File|*.csv";
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dataGridViewGoods_KAE.Columns)
            {

                csv += column.Name + ",";
            }
            csv = csv.Remove(csv.Length - 1);
            //Add new line.
            csv += "\r\n";

            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = sfd.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, csv);
        }

      

        private void textBoxFilterQuantityLess_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (textBoxFilterQuantityLess_KAE.Text != "")
                {
                    int.Parse(textBoxFilterQuantityLess_KAE.Text);
                    e.Cancel = false;
                }
            }
            catch
            {
                MessageBox.Show("Поле сравнение количества принимает только цифры");
                e.Cancel = true;
            }
        }

        private void textBoxFilterQuantityMore_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (textBoxFilterQuantityMore_KAE.Text != "")
                {
                    int.Parse(textBoxFilterQuantityMore_KAE.Text);
                    e.Cancel = false;
                }
            }
            catch
            {
                MessageBox.Show("Поле сравнение количества принимает только цифры");
                e.Cancel = true;
            }
        }

        private void checkBoxFilterQuantityLessEq_CheckedChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void checkBoxFilterQuantityMoreEq_CheckedChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void checkBoxFilterPriceLessEq_CheckedChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void checkBoxFilterPriceMoreEq_CheckedChanged(object sender, EventArgs e)
        {
            reset_filter_goods();
        }

        private void buttonOpenSupplyDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "csv File|*.csv";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = new DataTable();
                string[] lines = System.IO.File.ReadAllLines(opf.FileName);
                if (lines.Length > 0)
                {
                    //first line to create header
                    string firstLine = lines[0];
                    List<string> validHeaders = new List<string> { "SupplyNumber", "FIOSupply", "DateOfSupply", "QuantitySupply", "GoodsCodeSupply" };
                    string[] headerLabels = firstLine.Split(',');
                    if (!headerLabels.SequenceEqual(validHeaders))
                    {
                        MessageBox.Show("Файл не является файлом базы данных поставщиков приложения");

                        return;
                    }
                    foreach (DataGridViewColumn headerWord in dataGridViewSupply_KAE.Columns)
                    {
                        dt.Columns.Add(new DataColumn(headerWord.Name));

                    }
                    //For Data
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (lines[i] != "")
                        {
                            string[] dataWords = lines[i].Split(',');
                            DataRow dr = dt.NewRow();
                            int columnIndex = 0;
                            foreach (string headerWord in headerLabels)
                            {
                                dr[headerWord] = dataWords[columnIndex++];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                if (dt.Rows.Count >= 0)
                {
                    bindingSourceSupply_KAE.DataSource = dt;
                    dataGridViewSupply_KAE.Columns[0].DataPropertyName = "SupplyNumber";
                    dataGridViewSupply_KAE.Columns[1].DataPropertyName = "FIOSupply";
                    dataGridViewSupply_KAE.Columns[2].DataPropertyName = "DateOfSupply";
                    dataGridViewSupply_KAE.Columns[3].DataPropertyName = "QuantitySupply";
                    dataGridViewSupply_KAE.Columns[4].DataPropertyName = "GoodsCodeSupply";
                    dataGridViewSupply_KAE.DataSource = bindingSourceSupply_KAE;
                    bindingNavigatorSupply_KAE.BindingSource = bindingSourceSupply_KAE;
                }
            }
        }

        private void buttoSaveSupplyDB_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "csv File|*.csv";
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dataGridViewSupply_KAE.Columns)
            {

                csv += column.Name + ",";
            }
            csv = csv.Remove(csv.Length - 1);
            //Add new line.
            csv += "\r\n";

            //Adding the Rows
            foreach (DataGridViewRow row in dataGridViewSupply_KAE.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                    {
                        break;
                    }

                    //Add the Data rows.
                    csv += cell.Value.ToString().Replace(",", ";") + ",";
                }

                //Add new line.
                csv = csv.Remove(csv.Length - 1);
                csv += "\r\n";

            }

            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = sfd.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, csv);
        }

       

        private void dataGridViewSupply_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                //Initialized a new DateTimePicker Control  
               

                // It returns the retangular area that represents the Display area for a cell  
                Rectangle oRectangle = dataGridViewSupply_KAE.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                //Setting area for DateTimePicker Control  
                DateTimePickerDateOfSupply.Size = new Size(oRectangle.Width, oRectangle.Height);

                // Setting Location  
                DateTimePickerDateOfSupply.Location = new Point(oRectangle.X, oRectangle.Y);

                // An event attached to dateTimePicker Control which is fired when DateTimeControl is closed  
                DateTimePickerDateOfSupply.CloseUp += new EventHandler(DateTimePickerDateOfSupply_CloseUp);

                // An event attached to dateTimePicker Control which is fired when any date is selected  
                DateTimePickerDateOfSupply.TextChanged += new EventHandler(DateTimePickerDateOfSupply_OnTextChange);

                // Now make it visible  
                DateTimePickerDateOfSupply.Visible = true;
            }
        }

        private void DateTimePickerDateOfSupply_OnTextChange(object sender, EventArgs e)
        {
            // Saving the 'Selected Date on Calendar' into DataGridView current cell  
            dataGridViewSupply_KAE.CurrentCell.Value = DateTimePickerDateOfSupply.Text.ToString();
        }

        void DateTimePickerDateOfSupply_CloseUp(object sender, EventArgs e)
        {
            // Hiding the control after use   
            DateTimePickerDateOfSupply.Visible = false;
        }


        private void reset_filter_supply()
        {
            List<string> filters = new List<string> { };

            if (textBoxFilterSupplyNumber_KAE.Text != "" && textBoxFilterSupplyNumber_KAE.Text != null)
            {
                string filterSupplyNumber = string.Format("( SupplyNumber LIKE '%{0}%' )", textBoxFilterSupplyNumber_KAE.Text);
                filters.Add(filterSupplyNumber);
            }

            if (textBoxFilterFIOSupply_KAE.Text != "" && textBoxFilterFIOSupply_KAE.Text != null)
            {

                string filterFIOSupply = string.Format("( FIOSupply LIKE '%{0}%' )", textBoxFilterFIOSupply_KAE.Text);
                filters.Add(filterFIOSupply);
            }

            if (textBoxFilterQuantitySupplyLess_KAE.Text != "" && textBoxFilterQuantitySupplyLess_KAE.Text != null)
            {
                string oper = "<";
                if (checkBoxFilterQuantitySupplyLessEq_KAE.Checked == true)
                {
                    oper = "<=";
                }

                string filterQuantityLess = string.Format("( QuantitySupply {0} {1} )", oper, textBoxFilterQuantitySupplyLess_KAE.Text);
                filters.Add(filterQuantityLess);
            }

            if (textBoxFilterQuantitySupplyMore_KAE.Text != "" && textBoxFilterQuantitySupplyMore_KAE.Text != null)
            {
                string oper = ">";
                if (checkBoxFilterQuantitySupplyMoreEq_KAE.Checked == true)
                {
                    oper = ">=";
                }

                string filterQuantityMore = string.Format("( QuantitySupply {0} {1} )", oper, textBoxFilterQuantitySupplyMore_KAE.Text);
                filters.Add(filterQuantityMore);
            }

            if (dateTimePickerFilterDateOfSupplyPre_KAE.Value != null && dateTimePickerFilterDateOfSupplyPre_KAE.Checked == true)
            {
               
                string filterDateOfSupplyPre = string.Format("( DateOfSupply >= '{0:dd-MM-yyyy}' )", dateTimePickerFilterDateOfSupplyPre_KAE.Value);
                filters.Add(filterDateOfSupplyPre);
            }

            if (dateTimePickerFilterDateOfSupplyPost_KAE.Value != null && dateTimePickerFilterDateOfSupplyPost_KAE.Checked == true)
            {

                string filterDateOfSupplyPost = string.Format("( DateOfSupply <= '{0:dd-MM-yyyy}' )", dateTimePickerFilterDateOfSupplyPost_KAE.Value);
                filters.Add(filterDateOfSupplyPost);
            }

            if (textBoxFilterGoodsCodeSupply_KAE.Text != "" && textBoxFilterGoodsCodeSupply_KAE.Text != null)
            {
                string filterGoodsCode = string.Format("( GoodsCodeSupply Like '%{0}%' )", textBoxFilterGoodsCodeSupply_KAE.Text);
                filters.Add(filterGoodsCode);
            }

            try
            {
                string finalFilter = String.Join(" AND ", filters);
                bindingSourceSupply_KAE.Filter = finalFilter;
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Неверные параметры фильтров. Ошибка: {0}", e.Message));
            }
        }


        private void checkBoxFilterSupply_KAE_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFilterSupply_KAE.Checked == true)
            {
                labelFilterSupplyNumber_KAE.Enabled = true;
                labelFilterFIOSupply_KAE.Enabled = true;
                labelFilterDateOfSupply_KAE.Enabled = true;
                labelFilterDateOfSupplyPre_KAE.Enabled = true;
                labelFilterDateOfSupplyPost_KAE.Enabled = true;
                dateTimePickerFilterDateOfSupplyPre_KAE.Enabled = true;
                dateTimePickerFilterDateOfSupplyPost_KAE.Enabled = true;
                labelFilterQuantitySupply_KAE.Enabled = true;
                textBoxFilterSupplyNumber_KAE.Enabled = true;
                textBoxFilterFIOSupply_KAE.Enabled = true;
                textBoxFilterQuantitySupplyLess_KAE.Enabled = true;
                textBoxFilterQuantitySupplyMore_KAE.Enabled = true;
                labelFilterQuantitySupplyLess_KAE.Enabled = true;
                labelFilterQuantitySupplyMore_KAE.Enabled = true;
                checkBoxFilterQuantitySupplyLessEq_KAE.Enabled = true;
                checkBoxFilterQuantitySupplyMoreEq_KAE.Enabled = true;
                labelFilterGoodsCodeSupply_KAE.Enabled = true;
                textBoxFilterGoodsCodeSupply_KAE.Enabled = true;

                labelFilterQuantitySupplyLess_KAE.Visible = true;
                labelFilterQuantitySupplyMore_KAE.Visible = true;
            }

            if (checkBoxFilterSupply_KAE.Checked == false)
            {
                labelFilterSupplyNumber_KAE.Enabled = false;
                labelFilterFIOSupply_KAE.Enabled = false;
                labelFilterDateOfSupply_KAE.Enabled = false;
                labelFilterDateOfSupplyPre_KAE.Enabled = false;
                labelFilterDateOfSupplyPost_KAE.Enabled = false;
                textBoxFilterSupplyNumber_KAE.Enabled = false;
                textBoxFilterFIOSupply_KAE.Enabled = false;
                dateTimePickerFilterDateOfSupplyPre_KAE.Enabled = false;
                dateTimePickerFilterDateOfSupplyPost_KAE.Enabled = false;
                labelFilterQuantitySupply_KAE.Enabled = false;
                textBoxFilterQuantitySupplyLess_KAE.Enabled = false;
                textBoxFilterQuantitySupplyMore_KAE.Enabled = false;
                labelFilterQuantitySupplyLess_KAE.Enabled = false;
                labelFilterQuantitySupplyMore_KAE.Enabled = false;
                checkBoxFilterQuantitySupplyLessEq_KAE.Enabled = false;
                checkBoxFilterQuantitySupplyMoreEq_KAE.Enabled = false;
                labelFilterGoodsCodeSupply_KAE.Enabled = false;
                textBoxFilterGoodsCodeSupply_KAE.Enabled = false;

                labelFilterQuantitySupplyLess_KAE.Visible = false;
                labelFilterQuantitySupplyMore_KAE.Visible = false;

                bindingSourceSupply_KAE.Filter = "";
            }
        }

        private void dataGridViewSupply_KAE_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            checkBoxFilterSupply_KAE.Enabled = true;
        }

        private void dateTimePickerFilterDateOfSupplyPre_KAE_ValueChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        private void dateTimePickerFilterDateOfSupplyPost_KAE_ValueChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        private void textBoxFilterSupplyNumber_KAE_TextChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        private void textBoxFilterFIOSupply_KAE_TextChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        private void textBoxFilterQuantitySupplyLess_KAE_TextChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        private void textBoxFilterQuantitySupplyMore_KAE_TextChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        private void textBoxFilterGoodsCodeSupply_KAE_TextChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        private void textBoxFilterGoodsCodeSupply_KAE_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (textBoxFilterQuantityLess_KAE.Text != "")
                {
                    int.Parse(textBoxFilterQuantityLess_KAE.Text);
                    e.Cancel = false;
                }
            }
            catch
            {
                MessageBox.Show("Поле кода товара принимает только цифры");
                e.Cancel = true;
            }
        }

        private void textBoxFilterQuantitySupplyMore_KAE_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (textBoxFilterQuantityLess_KAE.Text != "")
                {
                    int.Parse(textBoxFilterQuantityLess_KAE.Text);
                    e.Cancel = false;
                }
            }
            catch
            {
                MessageBox.Show("Поле сравнение количества принимает только цифры");
                e.Cancel = true;
            }
        }

        private void textBoxFilterQuantitySupplyLess_KAE_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (textBoxFilterQuantityLess_KAE.Text != "")
                {
                    int.Parse(textBoxFilterQuantityLess_KAE.Text);
                    e.Cancel = false;
                }
            }
            catch
            {
                MessageBox.Show("Поле сравнение количества принимает только цифры");
                e.Cancel = true;
            }
        }

        private void checkBoxFilterQuantitySupplyMoreEq_KAE_CheckedChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        private void checkBoxFilterQuantitySupplyLessEq_KAE_CheckedChanged(object sender, EventArgs e)
        {
            reset_filter_supply();
        }

        
    }

}
