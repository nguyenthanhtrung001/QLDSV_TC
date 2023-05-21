using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLDSV_TC
{
    public partial class frmNhapDiem : Form
    {

        string nienKhoa = "";
        string hocKy = "";
        string nhom = "";
        string monHoc = "";
        string khoa = "";

        DataTable dt_DS_DangKy = new DataTable();


        public frmNhapDiem()
        {
            InitializeComponent();
            dgvDiem.CellFormatting += dgvDiem_CellFormatting;
            this.dgvDiem.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiem_CellValueChanged);
            this.dgvDiem.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvDiem_CellValidating);

        }





        private void layDS_CN(String cmd)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(cmd, Program.connstr_publisher);
            da.Fill(dt);
            BindingSource bds = new BindingSource();
            bds.DataSource = dt;

            cbxKhoa.DataSource = bds;
            bds.Filter = "TENCN LIKE 'KHOA%'";

            cbxKhoa.DisplayMember = "TENCN";
            cbxKhoa.ValueMember = "TENSERVER";



        }

        private void frmNhapDiem_Load(object sender, EventArgs e)
        {





            btnCapNhat.Enabled = false;
            DS1.EnforceConstraints = false;
            this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
            this.lOPTINCHITableAdapter.Fill(this.DS1.LOPTINCHI);



            layDS_CN("SELECT * FROM Get_Subscribes");

            if (Program.mGroup == "PGV")
            {
                cbxKhoa.Enabled = true;

            }
            else
            {
                cbxKhoa.Enabled = false;
            }


            cbxKhoa.SelectedIndex = Program.mChiNhanh;

            /* layDSMon("SELECT * FROM V_DS_MonHoc");
             cbxMonHoc.SelectedIndex = 1; */




            // TODO: This line of code loads data into the 'dS1.LOPTINCHI' table. You can move, or remove it, as needed.
            //           
            if (Program.KetNoi() == 0) return;
            if (cbxKhoa.SelectedValue.ToString() == "LAPTOP-BB4P1S1G\\MSSQLSERVER01")
            {
                khoa = "CNTT";
            }
            else khoa = "VT";
            string strlenh = "SELECT DISTINCT NIENKHOA FROM LOPTINCHI WHERE MAKHOA = '" + khoa + "'";

            layDS_NK(strlenh);
            nienKhoa = cbxNienKhoa.SelectedValue.ToString();


            string strlenh1 = "SELECT DISTINCT HOCKY FROM LOPTINCHI WHERE NIENKHOA='" + nienKhoa + "' AND MAKHOA='" + khoa + "'";

            layDS_HK(strlenh1);

            string strlenh2 = "SELECT m.MAMH, m.TENMH FROM MONHOC m JOIN(SELECT DISTINCT MAMH FROM LOPTINCHI"
                 + " WHERE NIENKHOA = '" + nienKhoa + "' AND HOCKY = '" + hocKy + "' AND MAKHOA = '" + khoa + "') ltc ON m.MAMH = ltc.MAMH";


            layDS_MH(strlenh2);





        }


        private void layDSMon(String cmd)
        {
            DataTable dt = new DataTable();
            this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
            SqlDataAdapter da = new SqlDataAdapter(cmd, Program.connstr);
            da.Fill(dt);


            cbxMonHoc.DataSource = dt;
            cbxMonHoc.DisplayMember = "TENMH";
            cbxMonHoc.ValueMember = "MAMH";

        }




        private void btnBatdau_Click(object sender, EventArgs e)
        {

            btnCapNhat.Enabled = true;

            try
            {
                String strlenh = "EXEC SP_GETDSNHAPDIEM '" + nienKhoa + "', '" + hocKy + "', '" + monHoc + "' ,'" + nhom + "'";



                dt_DS_DangKy = Program.ExecSqlDataTable(strlenh);
                // bdsNhapDiem.DataSource = dt_DS_DangKy;
                // Thiết lập ReadOnly cho cột "Name"
                dt_DS_DangKy.Columns["MASV"].ReadOnly = true;
                dt_DS_DangKy.Columns["HOTEN"].ReadOnly = true;

                bdsDSNhapDiem.DataSource = dt_DS_DangKy;







            }
            catch (System.Exception ex)
            {
                MessageBox.Show("HELLOcc" + ex.Message);
                MessageBox.Show(ex.StackTrace);
            }

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MALTC", typeof(int));
            dt.Columns.Add("MASV", typeof(string));
            dt.Columns.Add("DIEM_CC", typeof(int));
            dt.Columns.Add("DIEM_GK", typeof(float));
            dt.Columns.Add("DIEM_CK", typeof(float));

            for (int i = 0; i < dt_DS_DangKy.Rows.Count; i++)
            {
                dt.Rows.Add(dt_DS_DangKy.Rows[i]["MALTC"], dt_DS_DangKy.Rows[i]["MASV"], dt_DS_DangKy.Rows[i]["DIEM_CC"], dt_DS_DangKy.Rows[i]["DIEM_GK"], dt_DS_DangKy.Rows[i]["DIEM_CK"]);

            }
            try
            {
                SqlParameter para = new SqlParameter();
                para.SqlDbType = SqlDbType.Structured;
                para.TypeName = "dbo.TYPE_DANGKY";
                para.ParameterName = "@DIEMTHI";
                para.Value = dt;

                Program.KetNoi();
                SqlCommand Sqlcmd = new SqlCommand("SP_UPDATE_DIEM", Program.conn);
                Sqlcmd.Parameters.Clear();
                Sqlcmd.CommandType = CommandType.StoredProcedure;
                Sqlcmd.Parameters.Add(para);
                Sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Điểm đã được cập nhật!!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }


        }

        private void cbxKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxKhoa.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.servername = cbxKhoa.SelectedValue.ToString();
            if (cbxKhoa.SelectedIndex != Program.mChiNhanh)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.mpassword;
            }
            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Kết nối csdl thất bại!", "", MessageBoxButtons.OK);
                return;
            }

            if (cbxKhoa.SelectedValue.ToString() == "LAPTOP-BB4P1S1G\\MSSQLSERVER01")
            {
                khoa = "CNTT";
            }
            else khoa = "VT";
            string strlenh = "SELECT DISTINCT NIENKHOA FROM LOPTINCHI WHERE MAKHOA = '" + khoa + "'";

            layDS_NK(strlenh);



        }

        private void layDS_NK(String cmd)
        {
            DataTable dt = new DataTable();
            // this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
            SqlDataAdapter da = new SqlDataAdapter(cmd, Program.connstr);
            da.Fill(dt);


            cbxNienKhoa.DataSource = dt;
            cbxNienKhoa.DisplayMember = "NIENKHOA";
            cbxNienKhoa.ValueMember = "NIENKHOA";
            try
            {
                cbxNienKhoa.SelectedIndex = 0;
                nienKhoa = cbxNienKhoa.SelectedValue.ToString();


            }
            catch (Exception ex) { }


        }
        private void layDS_HK(String cmd)
        {
            DataTable dt = new DataTable();
            // this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
            SqlDataAdapter da = new SqlDataAdapter(cmd, Program.connstr);
            da.Fill(dt);


            cbxHocKy.DataSource = dt;
            cbxHocKy.DisplayMember = "HOCKY";
            cbxHocKy.ValueMember = "HOCKY";
            try
            {
                cbxHocKy.SelectedIndex = 0;
                hocKy = cbxHocKy.SelectedValue.ToString();
            }
            catch (Exception ex) { }


        }
        private void gvDiem_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName == "DIEM_CC")
            {
                int diem = 0;
                if (string.IsNullOrEmpty(e.Value as string))
                    return;
                try
                {
                    diem = Int32.Parse(e.Value as string);
                }
                catch (Exception ex)
                {
                    e.Valid = false;
                    e.ErrorText = "Điểm chưa đúng định dạng 0-10";
                }
                if (diem < 0 || diem > 10)
                {

                    e.Valid = false;
                    e.ErrorText = "Điểm phải từ 0-10";
                }
            }
            if (view.FocusedColumn.FieldName == "DIEM_GK" || view.FocusedColumn.FieldName == "DIEM_CK")
            {
                float diem = 0;
                if (string.IsNullOrEmpty(e.Value as string))
                    return;
                try
                {
                    diem = float.Parse(e.Value as string);
                }
                catch (Exception ex)
                {
                    e.Valid = false;
                    e.ErrorText = "Điểm phải là một số 0-10";
                }

                if (diem < 0 || diem > 10)
                {

                    e.Valid = false;
                    e.ErrorText = "Điểm phải từ 0-10";
                }
                else
                {
                    e.Value = Math.Round(diem * 2, MidpointRounding.AwayFromZero) / 2;
                }
            }

        }
        private void gvDiem_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle == view.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.Azure;
            }
        }


        private void dgvDiem_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvDiem.Columns["DIEMHETMON"].Index && e.RowIndex >= 0)
            {
                // Lấy giá trị của các cột dIEMCCDataGridViewTextBoxColumn, dIEMGKDataGridViewTextBoxColumn và dIEMCKDataGridViewTextBoxColumn
                double diemCC = 0;
                if (dgvDiem.Rows[e.RowIndex].Cells["dIEMCCDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    diemCC = Convert.ToDouble(dgvDiem.Rows[e.RowIndex].Cells["dIEMCCDataGridViewTextBoxColumn"].Value);
                }

                double diemGK = 0;
                if (dgvDiem.Rows[e.RowIndex].Cells["dIEMGKDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    diemGK = Convert.ToDouble(dgvDiem.Rows[e.RowIndex].Cells["dIEMGKDataGridViewTextBoxColumn"].Value);
                }

                double diemCK = 0;
                if (dgvDiem.Rows[e.RowIndex].Cells["dIEMCKDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    diemCK = Convert.ToDouble(dgvDiem.Rows[e.RowIndex].Cells["dIEMCKDataGridViewTextBoxColumn"].Value);
                }
                // Tính giá trị của DIEMHETMON
                double diemHM = diemCC * 0.1 + diemGK * 0.3 + diemCK * 0.6;

                // Gán giá trị của DIEMHETMON vào cell
                e.Value = diemHM;
            }
        }
        private void dgvDiem_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 2 && e.ColumnIndex <= 4) // Kiểm tra chỉ tính toán và cập nhật khi thay đổi giá trị ở cột DIEMCC, DIEMGK, hoặc DIEMCK
            {
                DataGridViewRow row = dgvDiem.Rows[e.RowIndex];
                double diemCC = 0;
                if (dgvDiem.Rows[e.RowIndex].Cells["dIEMCCDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    diemCC = Convert.ToDouble(dgvDiem.Rows[e.RowIndex].Cells["dIEMCCDataGridViewTextBoxColumn"].Value);
                }

                double diemGK = 0;
                if (dgvDiem.Rows[e.RowIndex].Cells["dIEMGKDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    diemGK = Convert.ToDouble(dgvDiem.Rows[e.RowIndex].Cells["dIEMGKDataGridViewTextBoxColumn"].Value);
                }

                double diemCK = 0;
                if (dgvDiem.Rows[e.RowIndex].Cells["dIEMCKDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    diemCK = Convert.ToDouble(dgvDiem.Rows[e.RowIndex].Cells["dIEMCKDataGridViewTextBoxColumn"].Value);
                }
                double diemHM = diemCC * 0.1 + diemGK * 0.3 + diemCK * 0.6;
                row.Cells["DIEMHETMON"].Value = diemHM;
            }
        }

        private void dgvDiem_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvDiem.Columns[e.ColumnIndex].Name == "dIEMCCDataGridViewTextBoxColumn")
            {
                // Kiểm tra giá trị ô dIEMCCDataGridViewTextBoxColumn
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvDiem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                }
                else
                {
                    int diemCC = 0;
                    if (!int.TryParse(e.FormattedValue.ToString(), out diemCC))
                    {
                        MessageBox.Show("Giá trị của ô dIEMCCDataGridViewTextBoxColumn phải là số nguyên từ 0 đến 10", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                    else if (diemCC < 0 || diemCC > 10)
                    {
                        MessageBox.Show("Giá trị của ô dIEMCCDataGridViewTextBoxColumn phải từ 0 đến 10", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                }
            }
            else if (dgvDiem.Columns[e.ColumnIndex].Name == "dIEMGKDataGridViewTextBoxColumn" || dgvDiem.Columns[e.ColumnIndex].Name == "dIEMCKDataGridViewTextBoxColumn")
            {
                // Kiểm tra giá trị của ô dIEMGKDataGridViewTextBoxColumn hoặc dIEMCKDataGridViewTextBoxColumn
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvDiem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                }
                else
                {
                    double diem = 0;
                    if (!double.TryParse(e.FormattedValue.ToString(), out diem))
                    {
                        MessageBox.Show("Giá trị của ô " + dgvDiem.Columns[e.ColumnIndex].HeaderText + " phải là số từ 0 đến 10", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                    else if (diem < 0 || diem > 10)
                    {
                        MessageBox.Show("Giá trị của ô " + dgvDiem.Columns[e.ColumnIndex].HeaderText + " phải từ 0 đến 10", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void cbxNienKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            nienKhoa = cbxNienKhoa.SelectedValue.ToString();


            string strlenh = "SELECT DISTINCT HOCKY FROM LOPTINCHI WHERE NIENKHOA='" + nienKhoa + "' AND MAKHOA='" + khoa + "'";

            layDS_HK(strlenh);
        }

        private void cbxHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            hocKy = cbxHocKy.SelectedValue.ToString();

            string strlenh = "SELECT m.MAMH, m.TENMH FROM MONHOC m JOIN(SELECT DISTINCT MAMH FROM LOPTINCHI"
                  + " WHERE NIENKHOA = '" + nienKhoa + "' AND HOCKY = '" + hocKy + "' AND MAKHOA = '" + khoa + "') ltc ON m.MAMH = ltc.MAMH";


            layDS_MH(strlenh);
        }

        private void cbxMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            monHoc = cbxMonHoc.SelectedValue.ToString();

            string strlenh = "SELECT DISTINCT NHOM FROM LOPTINCHI WHERE NIENKHOA='" + nienKhoa + "' AND MAKHOA='" + khoa + "'" + " AND MAMH='" + monHoc + "'" + " AND HOCKY='" + hocKy + "'";


            layDS_Nhom(strlenh);
        }

        private void cbxNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            nhom = cbxNhom.SelectedValue.ToString();

        }
        private void layDS_MH(String cmd)
        {
            DataTable dt = new DataTable();
            // this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
            SqlDataAdapter da = new SqlDataAdapter(cmd, Program.connstr);
            da.Fill(dt);


            cbxMonHoc.DataSource = dt;
            cbxMonHoc.DisplayMember = "TENMH";
            cbxMonHoc.ValueMember = "MAMH";
            try
            {
                cbxMonHoc.SelectedIndex = 0;
                monHoc = cbxMonHoc.SelectedValue.ToString();
            }
            catch (Exception ex) { }


        }
        private void layDS_Nhom(String cmd)
        {
            DataTable dt = new DataTable();
            // this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
            SqlDataAdapter da = new SqlDataAdapter(cmd, Program.connstr);
            da.Fill(dt);


            cbxNhom.DataSource = dt;
            cbxNhom.DisplayMember = "NHOM";
            cbxNhom.ValueMember = "NHOM";
            try
            {
                cbxNhom.SelectedIndex = 0;
                monHoc = cbxNhom.SelectedValue.ToString();
            }
            catch (Exception ex) { }


        }

    }
}
