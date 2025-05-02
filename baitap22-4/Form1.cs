using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace baitap22_4
{
    public partial class Form1 : Form
    {
        // tạo 2 biến cục bộ
        // string strCon = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=QuanLySinhVien;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        string strCon = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=QuanLySinhVien;Integrated Security=True;Encrypt=False";
        // đối tượng kết nối
        SqlConnection sqlCon = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon == null || sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon = new SqlConnection(strCon);
                    sqlCon.Open();
                }

                string query = "SELECT * FROM SinhVien";
                using (SqlDataAdapter da = new SqlDataAdapter(query, sqlCon))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(strCon))
                {
                    if(string.IsNullOrWhiteSpace(txtMSSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text)||
                        string.IsNullOrWhiteSpace(txtNgaySinh.Text) || string.IsNullOrWhiteSpace(txtDiaChi.Text))
                    {
                        MessageBox.Show("Vui lòng nhập thông tin trước khi thêm ");
                        return;
                    }
                    sqlCon.Open();
                    string query = "INSERT INTO SinhVien (MSSV, HoTen,Ngaysinh,Diachi) VALUES (@MSSV, @HoTen,@NgaySinh,@DiaChi)";
                    using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                    {
                        cmd.Parameters.AddWithValue("@MSSV", txtMSSV.Text);
                        cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                        cmd.Parameters.AddWithValue("@Ngaysinh",txtNgaySinh.Text);
                        cmd.Parameters.AddWithValue("@Diachi", txtDiaChi.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm thành công!");
                        LoadData(); // Hàm hiển thị lại dữ liệu
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void LoadData()
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(strCon))
                {
                    sqlCon.Open();
                    string query = "SELECT * FROM SinhVien";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, sqlCon))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(strCon))
                {
                    sqlCon.Open();
                    string query = "UPDATE SinhVien SET HoTen = @HoTen, NgaySinh = @NgaySinh, DiaChi = @DiaChi WHERE MSSV = @MSSV";

                    using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                    {
                        cmd.Parameters.AddWithValue("@MSSV", txtMSSV.Text);
                        cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                        cmd.Parameters.AddWithValue("@NgaySinh", txtNgaySinh.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            MessageBox.Show("Sửa thành công!");
                        else
                            MessageBox.Show("Không tìm thấy sinh viên để sửa.");

                        LoadData(); // Cập nhật lại DataGridView
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMSSV.Text))
                {
                    MessageBox.Show("Vui lòng nhập MSSV cần xóa!");
                    return;
                }
                using (SqlConnection sqlCon = new SqlConnection(strCon))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM SinhVien WHERE MSSV = @MSSV";
                    using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                    {
                        cmd.Parameters.AddWithValue("@MSSV", txtMSSV.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa thành công!");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(strCon))
                {
                    sqlCon.Open();
                    string query = "SELECT * FROM SinhVien WHERE MSSV = @MSSV";

                    using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                    {
                        cmd.Parameters.AddWithValue("@MSSV", txtMSSV.Text.Trim()); // Loại bỏ khoảng trắng

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sinh viên có MSSV: " + txtMSSV.Text);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtNgaySinh.Clear();
            txtDiaChi.Clear();
            txtMSSV.Focus();
        }
    }
}
