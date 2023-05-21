using DevExpress.XtraReports.UI;
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
    public partial class Frpt_InDiemTheoMon : Form
    {
        string nienKhoa = "";
        string hocKy = "";
        string nhom = "";
        string monHoc = "";
        string khoa = "";
        public Frpt_InDiemTheoMon()
        {
            InitializeComponent();

        }

        private void lOPTINCHIBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.lOPTINCHIBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS1);

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

        private void Frpt_InDiemTheoMon_Load(object sender, EventArgs e)
        {


            dS1.EnforceConstraints = false;
            this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;

            this.lOPTINCHITableAdapter.Fill(this.dS1.LOPTINCHI);
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

            if (cbxKhoa.Text.Equals("KHOA CÔNG NGHỆ THÔNG TIN"))
            {
                khoa = "CNTT";
            }
            else khoa = "VT";
            string strlenh = "SELECT DISTINCT NIENKHOA FROM LOPTINCHI WHERE MAKHOA = '" + khoa + "'";
            MessageBox.Show(strlenh);

            layDS_NK(strlenh);
            try
            {
                nienKhoa = cbxNienKhoa.SelectedValue.ToString();
            }
            catch (Exception ex) { }


            string strlenh1 = "SELECT DISTINCT HOCKY FROM LOPTINCHI WHERE NIENKHOA='" + nienKhoa + "' AND MAKHOA='" + khoa + "'";

            layDS_HK(strlenh1);

            string strlenh2 = "SELECT m.MAMH, m.TENMH FROM MONHOC m JOIN(SELECT DISTINCT MAMH FROM LOPTINCHI"
                 + " WHERE NIENKHOA = '" + nienKhoa + "' AND HOCKY = '" + hocKy + "' AND MAKHOA = '" + khoa + "') ltc ON m.MAMH = ltc.MAMH";


            layDS_MH(strlenh2);






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

        private void cbxNienKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            nienKhoa = cbxNienKhoa.SelectedValue.ToString();


            string strlenh = "SELECT DISTINCT HOCKY FROM LOPTINCHI WHERE NIENKHOA='" + nienKhoa + "' AND MAKHOA='" + khoa + "'";

            layDS_HK(strlenh);
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

        private void cbxHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            hocKy = cbxHocKy.SelectedValue.ToString();

            string strlenh = "SELECT m.MAMH, m.TENMH FROM MONHOC m JOIN(SELECT DISTINCT MAMH FROM LOPTINCHI"
                  + " WHERE NIENKHOA = '" + nienKhoa + "' AND HOCKY = '" + hocKy + "' AND MAKHOA = '" + khoa + "') ltc ON m.MAMH = ltc.MAMH";


            layDS_MH(strlenh);
        }

        private void cbxNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            nhom = cbxNhom.SelectedValue.ToString();
        }

        private void cbxMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            monHoc = cbxMonHoc.SelectedValue.ToString();

            string strlenh = "SELECT DISTINCT NHOM FROM LOPTINCHI WHERE NIENKHOA='" + nienKhoa + "' AND MAKHOA='" + khoa + "'" + " AND MAMH='" + monHoc + "'" + " AND HOCKY='" + hocKy + "'";


            layDS_Nhom(strlenh);
        }

        private void btnIN_Click(object sender, EventArgs e)
        {
            int hk = int.Parse(hocKy);
            int nh = int.Parse(nhom);

            Xrpt_SP_GETDSNHAPDIEM rpt = new Xrpt_SP_GETDSNHAPDIEM(nienKhoa, hk, monHoc, nh);
            string khoatmp = "";
            if (khoa == "CNTT") khoatmp = "CÔNG NGHỆ THÔNG TIN";
            else khoatmp = "VIỄN THÔNG";
            rpt.lblKhoa.Text = khoatmp;
            rpt.lblNienKhoa.Text = nienKhoa;
            rpt.lblHocKy.Text = hocKy;
            rpt.lblMonHoc.Text = cbxMonHoc.Text;
            rpt.lblNhom.Text = nhom;

            ReportPrintTool print = new ReportPrintTool(rpt);
            print.ShowPreviewDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
