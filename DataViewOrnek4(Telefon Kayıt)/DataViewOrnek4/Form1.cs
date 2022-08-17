using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataViewOrnek4
{
    public partial class Form1 : Form
    {
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        DataView dataView;
        string ConnectionString = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=Telefon.accdb";
        public Form1()
        {
            InitializeComponent();
            Goruntule();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Telefon_Ekle(textBox1.Text,textBox2.Text,Convert.ToInt32(textBox3.Text));
        }
        DataTable dt = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void Telefon_Ekle(string marka,string model, int fiyat)
        {
            
            try
            {
                DataRowView newRow = dataView.AddNew();
                newRow["Marka"] = marka;
                newRow["Model"] = model;
                newRow["Fiyat"] = fiyat;
                newRow.EndEdit();
                dataView.Sort = "Id";
               
                using (OleDbConnection baglanti = new OleDbConnection(ConnectionString))
                {
                 
                    OleDbCommand com = new OleDbCommand("INSERT INTO Telefon (Marka,Model,Fiyat) VALUES (@Marka,@Model,@Fiyat)");
                    com.Connection = baglanti;

                    baglanti.Open();
                    if (baglanti.State == ConnectionState.Open)
                    {
                      
                        com.Parameters.Add("@Marka", OleDbType.VarChar).Value = dataView[0][1];
                        com.Parameters.Add("@Model", OleDbType.VarChar).Value = dataView[0][2];
                        com.Parameters.Add("@Fiyat", OleDbType.Integer).Value = dataView[0][3];

                        try
                        {

                            com.ExecuteNonQuery();
                            MessageBox.Show("Telefon Eklendi");
                            Goruntule();
                        }
                        catch (OleDbException ex)
                        {
                            MessageBox.Show(ex.Source);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Bağlantı Hatası");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
              
            }
        }

        void Goruntule()
        {
            
            using (OleDbConnection baglanti = new OleDbConnection(ConnectionString))
            {
              
                dataView = new DataView();
                ds = new DataSet();
                cmd = new OleDbCommand("SELECT * FROM Telefon", baglanti);
                da = new OleDbDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds, "Add New");
                dataView = new DataView(ds.Tables[0]);
                dataView.Sort = "Id";

                listBox1.Items.Clear();
                
                for (int i = 0; i < dataView.Count; i++)
                {
                    listBox1.Items.Add(dataView[i][1]);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataView.RowFilter = "Marka = '" + listBox1.SelectedItem.ToString()+"'"; 
            dataGridView1.DataSource = dataView;

        }
    }
}
