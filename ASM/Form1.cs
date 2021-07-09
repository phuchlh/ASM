using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ASM
{
    public partial class Form1 : Form
    {
        SqlDataAdapter dAdapt;
        public static DataSet myds = new DataSet("QLSVien");
        public static SqlConnection cnStr =
                new SqlConnection(@"server=localhost\HPHUC;User ID =sa;Pwd=HongPhuc228;database=QLSVien");
        public Form1()
        {
            InitializeComponent();
            initDataSet();
            loadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLSVienDataSet1.KHOA' table. You can move, or remove it, as needed.
            this.kHOATableAdapter.Fill(this.qLSVienDataSet1.KHOA);
            // TODO: This line of code loads data into the 'qLSVienDataSet.SVIEN' table. You can move, or remove it, as needed.
            this.sVIENTableAdapter.Fill(this.qLSVienDataSet.SVIEN);
            dataSinhVien.ReadOnly = true;
        }

        public void initDataSet()
        {
            dAdapt = new SqlDataAdapter("Select * from SVIEN", cnStr);
            SqlCommandBuilder svBuilder = new SqlCommandBuilder(dAdapt);
        }

        public void loadData()
        {
            try
            {
                dAdapt.Fill(myds, "SVIEN");
                dataSinhVien.DataSource = myds.Tables["SVIEN"];
            }
            catch (Exception e)
            {
                MessageBox.Show("Can not load data " + e.Message);
                dataSinhVien.DataSource = null;
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            cancelClick();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnCanel_Click(object sender, EventArgs e)
        {
            if (btnCanel != null)
            {
                cancelClick();
            }
        }

        public void cancelClick()
        {
            txtTen.Text = string.Empty;
            txtMSSV.Text = string.Empty;
            txtMaKH.Text = string.Empty;
            txtNam.Text = string.Empty;
        }

        private void dataSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataSinhVien.CurrentRow.Index;
            txtMSSV.ReadOnly = true;

            this.txtTen.Text = dataSinhVien.Rows[index].Cells[0].Value.ToString();
            this.txtMSSV.Text = dataSinhVien.Rows[index].Cells[1].Value.ToString();
            this.txtNam.Text = dataSinhVien.Rows[index].Cells[2].Value.ToString();
            this.txtMaKH.Text = dataSinhVien.Rows[index].Cells[3].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            addNew();
        }

        public void addNew()
        {
            bool checkMSSV = checkInputMSSV(Int32.Parse(this.txtMSSV.Text));
            bool checkNam = checkInputNam(Int32.Parse(this.txtNam.Text));
            bool checkMaKH = checkInputMAKH(this.txtMaKH.Text);
            
            if (!checkMSSV || !checkNam || !checkMaKH)
            {
                MessageBox.Show("Err ");
            }
            else
            {
                DataRow newStudent = (DataRow)myds.Tables["SVIEN"].NewRow();
                newStudent["TEN"] = this.txtTen.Text;
                newStudent["MASV"] = Int32.Parse(this.txtMSSV.Text);
                newStudent["NAM"] = Int32.Parse(this.txtNam.Text);
                newStudent["MAKH"] = this.txtMaKH.Text;
                myds.Tables["SVIEN"].Rows.Add(newStudent);
                cancelClick();
            }
            try
            {
                dAdapt.Update(myds.Tables["SVIEN"]);
                //cancelClick();
            }
            catch (Exception e)
            {
                MessageBox.Show("Have problem with add new student " + e.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteData();
            cancelClick();
        }

        public void deleteData()
        {
            try
            {
                if (txtMSSV.Text != null)
                {
                    int delRow = dataSinhVien.CurrentCell.RowIndex;
                    myds.Tables["SVIEN"].Rows[delRow].Delete();
                    dAdapt.Update(myds.Tables["SVIEN"]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Have error with delete student " + e.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            addNew();
        }
        public bool checkInputMSSV(int mssv)
        {
            if (mssv == 0) return false;
            else return true;
        }

        public bool checkInputNam (int nam)
        {
            if(nam == 0 || nam < 1 || nam > 4) return false;
            else return true;
        }

        public bool checkInputMAKH (string ma)
        {
            if (Regex.IsMatch(ma, "SE|IA")) return true;
            else return false;
        }

        
    }
}
