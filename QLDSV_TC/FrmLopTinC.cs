using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//husing Microsoft.Data.Sqlite;
using System.Reflection;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLDSV_TC
{
    public partial class FrmLopTinC : DevExpress.XtraEditors.XtraForm
    {
        private int vitri;
        private SqlConnection conn_publisher = new SqlConnection();
        public FrmLopTinC()
        {
            InitializeComponent();
        }

        private void FrmLopTinC_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS1.DSMH' table. You can move, or remove it, as needed.
    //h        this.dSMHTableAdapter.Fill(this.dS1.DSMH);
            // TODO: This line of code loads data into the 'dS1.DSMH' table. You can move, or remove it, as needed.


            this.lOPTINCHITableAdapter.Fill(this.dS1.LOPTINCHI);

            dS1.EnforceConstraints = false;
    //h        this.dSGVTableAdapter.Connection.ConnectionString = Program.connstr;
    //h        this.dSGVTableAdapter.Fill(this.dS1.DSGV);

            //this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;

            this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
            this.lOPTINCHITableAdapter.Fill(this.dS1.LOPTINCHI);

            this.dANGKYTableAdapter.Connection.ConnectionString = Program.connstr;
            this.dANGKYTableAdapter.Fill(this.dS1.DANGKY);

            Program.bds_dspm.Filter = "TENCN not LIKE 'PKT%'  ";
            cmbKhoa.DataSource = Program.bds_dspm;
            cmbKhoa.DisplayMember = "TENCN";
            cmbKhoa.ValueMember = "TENSERVER";
            cmbKhoa.SelectedValue = Program.servername;



            //    cbKhoa.SelectedIndex = Program.mPhongBan;
            if (Program.mGroup == "PGV")
            {
                cmbKhoa.Enabled = true;
            }
            else
            {
                cmbKhoa.Enabled = false;
            }
            if (Program.mGroup == "SV")
            {

                btnThem.Enabled = btnSua.Enabled = btnGhi.Enabled =
                btnPhuchoi.Enabled = btnHtac.Enabled = btnXoa.Enabled = false;

            }
            else
            {
                btnThem.Enabled = btnSua.Enabled = btnHtac.Enabled = btnXoa.Enabled = true;
                btnGhi.Enabled = btnPhuchoi.Enabled = false;

            }

        }


        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click_1(object sender, EventArgs e)
        {

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void nHOMSpinEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void mAKHOATextEdit_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void BtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.Control = "edit";
            //gcLopTC.Dock = DockStyle.Right;
            vitri = BDSLopTinC.Position;
            gcLopTinC.Enabled = true;
            // this.EnablePanelControl(panelControl2);
            cbHuyLop.Enabled = true;
            BDSLopTinC.MoveFirst();
            btnGhi.Enabled = btnHtac.Enabled = true;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhuchoi.Enabled = false;
        }

        private void cmbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKhoa.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.servername = cmbKhoa.SelectedValue.ToString();
            if (cmbKhoa.SelectedIndex != Program.mChiNhanh)
            {
                Program.mlogin = Program.remotelogin;
                Program.mpassword = Program.remotepassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.mpassword = Program.passwordDN;
            }
            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Lỗi kết nối về chi nhánh mới", "", MessageBoxButtons.OK);
            }
            else
            {
                this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
                this.lOPTINCHITableAdapter.Fill(this.dS1.LOPTINCHI);

                this.dANGKYTableAdapter.Connection.ConnectionString = Program.connstr;
                this.dANGKYTableAdapter.Fill(this.dS1.DANGKY);
                Program.mMAKHOA = ((DataRowView)BDSLopTinC[0])["MAKHOA"].ToString();

            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.Control = "add";
            //gcLopTC.Dock = DockStyle.Right;
            vitri = BDSLopTinC.Position;
            //this.EnablePanelControl();
            BDSLopTinC.AddNew();
            txtNienKhoa.Focus();

            String cmd = "SELECT MAMH FROM dbo.MONHOC";
            DataTable dt = new DataTable();
            dt = Program.ExecSqlDataTable(cmd);
            cmbMAMH.DataSource = dt;
            cmbMAMH.DisplayMember = "MAMH";
            cmbMAMH.ValueMember = "MAMH";
            cmbMAMH.SelectedIndex = 0;

            String cmd1 = "SELECT * FROM dbo.GIANGVIEN";
            DataTable _dt1 = new DataTable();
            _dt1 = Program.ExecSqlDataTable(cmd1);
            cmbMaGV.DataSource = _dt1;
            cmbMaGV.DisplayMember = "MAGV";
            cmbMaGV.ValueMember = "MAGV";
            cmbMaGV.SelectedIndex = 0;



            txtMaKhoa.Text = Program.mMAKHOA;
            cbHuyLop.Enabled = false;
            cbHuyLop.CheckState = CheckState.Unchecked;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnHtac.Enabled = false;
            btnGhi.Enabled = btnHtac.Enabled = true;

            gcLopTinC.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bool check = this.checkfield();
            if (check)
            {
                try
                {
                    this.BDSLopTinC.EndEdit();
                    this.BDSLopTinC.ResetCurrentItem();// tự động render để hiển thị dữ liệu mới
                    this.lOPTINCHITableAdapter.Update(this.dS1.LOPTINCHI);
                    MessageBox.Show("Ghi thành công: ", "", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    BDSLopTinC.RemoveCurrent();
                    MessageBox.Show("Ghi dữ liệu thất lại. Vui lòng kiểm tra lại!\n" + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                gcLopTinC.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnHtac.Enabled = true;
                btnGhi.Enabled = btnHtac.Enabled = false;
                //this.DisablePanelControl();
            }
        }

        private int KiemTraTrung(String strLenh)
        {

            SqlDataReader dataReader = Program.ExecSqlDataReader(strLenh);

            // nếu null thì thoát luôn  ==> lỗi kết nối
            if (dataReader == null)
            {
                return -1;
            }
            dataReader.Read();
            int result = int.Parse(dataReader.GetValue(0).ToString());
            dataReader.Close();
            return result;
        }
        private bool checkfield()
        {
            if (Program.Control == "add")
            {
                if (txtNienKhoa.Text.Trim().Equals(" "))
                {
                    MessageBox.Show("Niên khoá không được để trống", "", MessageBoxButtons.OK);
                    txtNienKhoa.Focus();
                    return false;
                }

                if (spinSL.Text.Trim().Equals(" "))
                {
                    MessageBox.Show("Số lượng không được để trống", "", MessageBoxButtons.OK);
                    spinSL.Focus();
                    return false;
                }

                string strLenh = "DECLARE  @return_value int \n"
                + "EXEC	@return_value = [dbo].[CHECK_LOPTC] \n"
                               + "@NIENKHOA = N'" + txtNienKhoa.Text + "',@HOCKY = N'" + Decimal.ToInt32(spinHK.Value)
                               + "',@MAMH = N'" + cmbMAMH.SelectedValue.ToString() + "',@NHOM = N'" + Decimal.ToInt32(spinNhom.Value) + "' \n"
                               + "SELECT  'Return Value' = @return_value ";
                int result = this.KiemTraTrung(strLenh);
                if (result == 1)
                {
                    MessageBox.Show("Lớp tín chỉ đã có trong cơ sơ dữ liệu", "", MessageBoxButtons.OK);
                    txtNienKhoa.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnXoa_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BDSDangK.Count > 0)
            {
                MessageBox.Show("Không thể xoá lớp vì đã có đăng ký: ", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá lớp này ?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    vitri = BDSLopTinC.Position;
                    BDSLopTinC.RemoveCurrent();
                    this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
                    this.lOPTINCHITableAdapter.Update(this.dS1.LOPTINCHI);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("lỗi xoá lớp tín chỉ. Bạn hãy xoá lại\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
                    this.lOPTINCHITableAdapter.Fill(this.dS1.LOPTINCHI);
                    BDSLopTinC.Position = vitri;
                    return;
                }
            }
            if (BDSLopTinC.Count == 0) btnXoa.Enabled = false;
        }

        private void btnPhuchoi_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.lOPTINCHITableAdapter.Connection.ConnectionString = Program.connstr;
                this.lOPTINCHITableAdapter.Fill(this.dS1.LOPTINCHI);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload: " + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnHtac_ItemClick(object sender, ItemClickEventArgs e)
        {
            BDSLopTinC.CancelEdit();
            if (btnThem.Enabled == false) BDSLopTinC.Position = vitri;
            gcLopTinC.Enabled = true;
            gcLopTinC.Dock = DockStyle.Fill;
            //this.DisablePanelControl();
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhuchoi.Enabled = true;
            btnGhi.Enabled = btnHtac.Enabled = false;
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}