using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace buoi3_baitap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Tạo các cột cho DataGridView
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Mã Số NV";
            dataGridView1.Columns[1].Name = "Tên Nhân Viên";
            dataGridView1.Columns[2].Name = "Lương Cơ Bản";
        }

        // Xử lý khi nhấn nút Thêm
        private void btnThem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();

            // Sử dụng delegate để nhận dữ liệu từ Form2
            form.CapNhatNhanVien = new CapNhatNhanVienDelegate(ThemNhanVien);
            form.ShowDialog();
        }

        // Xử lý khi nhấn nút Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow dongChon = dataGridView1.SelectedRows[0];
                Form2 form = new Form2
                {
                    MSNV = dongChon.Cells[0].Value.ToString(),
                    TenNhanVien = dongChon.Cells[1].Value.ToString(),
                    LuongCoBan = dongChon.Cells[2].Value.ToString()
                };

                // Sử dụng delegate để cập nhật dữ liệu khi sửa
                form.CapNhatNhanVien = (maSoNV, tenNV, luongCB) =>
                {
                    dongChon.Cells[0].Value = maSoNV;
                    dongChon.Cells[1].Value = tenNV;
                    dongChon.Cells[2].Value = luongCB;
                };

                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa.");
            }
        }

        // Xử lý khi nhấn nút Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.");
            }
        }

        // Phương thức thêm nhân viên mới
        private void ThemNhanVien(string maSoNV, string tenNV, string luongCB)
        {
            dataGridView1.Rows.Add(maSoNV, tenNV, luongCB);
        }

        // Xử lý khi nhấn nút Đóng
        private void btnDongy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
