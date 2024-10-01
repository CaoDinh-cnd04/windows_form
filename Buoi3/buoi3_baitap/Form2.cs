using System;
using System.Windows.Forms;

namespace buoi3_baitap
{
    public delegate void CapNhatNhanVienDelegate(string maSoNV, string tenNV, string luongCB);

    public partial class Form2 : Form
    {
        // Khai báo delegate để cập nhật thông tin nhân viên
        public CapNhatNhanVienDelegate CapNhatNhanVien { get; set; }

        // Các thuộc tính để lưu dữ liệu nhân viên
        public string MSNV
        {
            get { return txtMSNV.Text; }
            set { txtMSNV.Text = value; }
        }

        public string TenNhanVien
        {
            get { return txtTenNV.Text; }
            set { txtTenNV.Text = value; }
        }

        public string LuongCoBan
        {
            get { return txtLuongCB.Text; }
            set { txtLuongCB.Text = value; }
        }

        public Form2()
        {
            InitializeComponent();
        }

        // Xử lý khi nhấn nút Đồng ý
        private void btnDongY_Click(object sender, EventArgs e)
        {
            if (CapNhatNhanVien != null)
            {
                // Gọi delegate để cập nhật dữ liệu
                CapNhatNhanVien(MSNV, TenNhanVien, LuongCoBan);
            }
            this.Close();
        }

        // Xử lý khi nhấn nút Bỏ qua
        private void btnBoQua_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
