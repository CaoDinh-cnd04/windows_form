using Empty_framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Load Form Event
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();  // Load data when form loads
        }

        // Method to load data into DataGridView and ComboBox
        private void LoadData()
        {
            try
            {
                using (StudentContextDB context = new StudentContextDB())
                {
                    // Load Faculties into ComboBox
                    List<Faculty> faculties = context.Faculties.ToList();
                    FillFacultyCombobox(faculties);

                    // Load Students into DataGridView
                    List<Student> students = context.Students.ToList();
                    BindGrid(students);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        // Method to bind students to DataGridView
        private void BindGrid(List<Student> students)
        {
            dgvStudent.Rows.Clear();
            foreach (var student in students)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = student.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = student.FullName;
                dgvStudent.Rows[index].Cells[2].Value = student.Faculty?.FacultyName;  // Null check for Faculty
                dgvStudent.Rows[index].Cells[3].Value = student.AverageScore;
            }
        }

        // Method to fill Faculty ComboBox
        private void FillFacultyCombobox(List<Faculty> faculties)
        {
            cmbKHOA.DataSource = faculties;
            cmbKHOA.DisplayMember = "FacultyName";
            cmbKHOA.ValueMember = "FacultyID";
        }

        // DataGridView Row Click Event (Load data into form fields)
        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudent.Rows[e.RowIndex];
                txtMSSV.Text = row.Cells[0].Value.ToString();
                txtHoTen.Text = row.Cells[1].Value.ToString();
                cmbKHOA.SelectedValue = row.Cells[2].Value;  // Set faculty value in combobox
                txtDTB.Text = row.Cells[3].Value.ToString();
            }
        }

        // Add Student Button Click Event
        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    using (StudentContextDB context = new StudentContextDB())
                    {
                        // Create new student object
                        Student newStudent = new Student
                        {
                            StudentID = int.Parse(txtMSSV.Text), // Ensure this matches the type in the database (string)
                            FullName = txtHoTen.Text,
                            FacultyID = (int)cmbKHOA.SelectedValue,
                            AverageScore = float.Parse(txtDTB.Text)
                        };

                        context.Students.Add(newStudent);
                        context.SaveChanges();

                        MessageBox.Show("Thêm mới sinh viên thành công!");
                    }

                    LoadData(); // Refresh DataGridView after adding
                    ResetForm(); // Clear form fields
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message);
            }
        }

        // Update Student Button Click Event
        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    using (StudentContextDB context = new StudentContextDB())
                    {
                        long studentID;
                        if (!long.TryParse(txtMSSV.Text, out studentID))
                        {
                            MessageBox.Show("MSSV phải là một số hợp lệ!");
                            return;
                        }

                        Student student = context.Students.FirstOrDefault(s => s.StudentID == studentID);
                        if (student != null)
                        {
                            // Cập nhật thông tin sinh viên
                            student.FullName = txtHoTen.Text;
                            student.FacultyID = (int)cmbKHOA.SelectedValue;
                            student.AverageScore = float.Parse(txtDTB.Text);

                            context.SaveChanges();
                            MessageBox.Show("Cập nhật thông tin sinh viên thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên với MSSV đã nhập!");
                        }
                    }

                    LoadData(); // Làm mới DataGridView sau khi cập nhật
                    ResetForm(); // Xóa dữ liệu trên form
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật sinh viên: " + ex.Message);
            }
        }


        // Delete Student Button Click Event
        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                long studentID;
                if (long.TryParse(txtMSSV.Text, out studentID))
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (StudentContextDB context = new StudentContextDB())
                        {
                            // Tìm sinh viên theo StudentID
                            Student student = context.Students.FirstOrDefault(s => s.StudentID == studentID);
                            if (student != null)
                            {
                                context.Students.Remove(student);
                                context.SaveChanges();
                                MessageBox.Show("Xóa sinh viên thành công!");
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sinh viên với MSSV đã nhập!");
                            }
                        }

                        LoadData(); // Làm mới DataGridView sau khi xóa
                        ResetForm(); // Xóa dữ liệu trên form
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập MSSV hợp lệ để xóa!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message);
            }
        }



        // Reset Form Fields Method
        private void ResetForm()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtDTB.Clear();
            cmbKHOA.SelectedIndex = -1; // Reset ComboBox
        }

        // Method to validate form fields before performing Add/Update operations
        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtMSSV.Text) || txtMSSV.Text.Length != 10)
            {
                MessageBox.Show("Mã số sinh viên phải có 10 kí tự!");
                return false;
            }

            if (string.IsNullOrEmpty(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên sinh viên!");
                return false;
            }

            if (!float.TryParse(txtDTB.Text, out float averageScore) || averageScore < 0 || averageScore > 10)
            {
                MessageBox.Show("Điểm trung bình không hợp lệ! Vui lòng nhập số trong khoảng từ 0 đến 10.");
                return false;
            }

            if (cmbKHOA.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn khoa.");
                return false;
            }

            return true;
        }

       
    }
}
