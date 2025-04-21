using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace PengelolaKeuangan
{
    public partial class FrmSignIncs : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query; 
        public FrmSignIncs()
        {
            alamat = "server=localhost; database=db_mahasiswa; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text != "" && txtPassword.Text != "" && txtNamaPengguna.Text != "" && Picbox1.Image != null)
                {
                    string folderPath = Path.Combine(Application.StartupPath, "C:\\Users\\BUDZ\\source\\repos\\PengelolaKeuangan\\Images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Membuat nama unik untuk file gambar agar tidak tertimpa
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    string filePath = Path.Combine(folderPath, fileName);

                    // Simpan gambar dari PictureBox ke folder
                    Picbox1.Image.Save(filePath);

                    query = string.Format("insert into tbl_pengguna  values ('{0}','{1}','{2}','{3}','{4}','{5}');", txtID.Text, txtUsername.Text, txtPassword.Text, txtNamaPengguna.Text, bxLevel.Text, fileName);


                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();
                    if (res == 1)
                    {
                        MessageBox.Show("Insert Data Suksess ...");
                        FrmSignIncs_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Gagal inser Data . . . ");
                    }
                }
                else
                {
                    MessageBox.Show("Data Tidak lengkap !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text != "" || txtID.Text != "")
                {
                    query = string.Format("select * from tbl_pengguna where username = '{0}' or id_pengguna ='{1}'", txtUsername.Text, txtID.Text);
                    ds.Clear();
                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    adapter = new MySqlDataAdapter(perintah);
                    perintah.ExecuteNonQuery();
                    adapter.Fill(ds);
                    koneksi.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow kolom in ds.Tables[0].Rows)
                        {
                            txtID.Text = kolom["id_pengguna"].ToString();
                            txtPassword.Text = kolom["password"].ToString();
                            txtNamaPengguna.Text = kolom["nama_pengguna"].ToString();
                            bxLevel.Text = kolom["level"].ToString();
                            string foto = kolom["foto"].ToString();

                            string folderPath = Path.Combine(Application.StartupPath, "C:\\Users\\BUDZ\\source\\repos\\PengelolaKeuangan\\Images");
                            string filePath = Path.Combine(folderPath, foto);

                            // Cek apakah file foto ada
                            if (File.Exists(filePath))
                            {
                                // Tampilkan gambar di PictureBox
                                Picbox1.Image = Image.FromFile(filePath);
                                Picbox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            else
                            {
                                MessageBox.Show("File gambar tidak ditemukan.");
                            }

                        }

                        dataGridView1.DataSource = ds.Tables[0];
                        btnSave.Enabled = false;
                        btnUpdate.Enabled = true;
                        btnDelete.Enabled = true;
                        btnSearch.Enabled = false;
                        btnClear.Enabled = true;
                        LblPic.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Ada !!");
                        FrmSignIncs_Load(null, null);
                    }

                }
                else
                {
                    MessageBox.Show("Data Yang Anda Pilih Tidak Ada !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text != "" && txtNamaPengguna.Text != "" && txtUsername.Text != "" && txtID.Text != "" && Picbox1.Image != null)
                {
                    string folderPath = Path.Combine(Application.StartupPath, "C:\\Users\\BUDZ\\source\\repos\\PengelolaKeuangan\\Images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Membuat nama unik untuk file gambar agar tidak tertimpa
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    string filePath = Path.Combine(folderPath, fileName);

                    // Simpan gambar dari PictureBox ke folder
                    Picbox1.Image.Save(filePath);

                    // Query untuk update data
                    query = "UPDATE tbl_pengguna SET password = @password, level = @level, foto = @foto, nama_pengguna = @nama_pengguna WHERE id_pengguna = @id_pengguna";

                    // Menggunakan blok using untuk koneksi dan perintah
                    using (MySqlConnection koneksi = new MySqlConnection("server=localhost; database=db_mahasiswa; username=root; password=;"))
                    {
                        using (MySqlCommand perintah = new MySqlCommand(query, koneksi))
                        {
                            // Menambahkan parameter untuk mencegah SQL Injection
                            perintah.Parameters.AddWithValue("@id_pengguna", txtID.Text);
                            perintah.Parameters.AddWithValue("@password", txtPassword.Text);
                            perintah.Parameters.AddWithValue("@level", bxLevel.Text);
                            perintah.Parameters.AddWithValue("@foto", fileName);
                            perintah.Parameters.AddWithValue("@nama_pengguna", txtNamaPengguna.Text);

                            koneksi.Open(); // Membuka koneksi
                            int res = perintah.ExecuteNonQuery(); // Menjalankan perintah
                            koneksi.Close(); // Menutup koneksi

                            if (res == 1)
                            {
                                MessageBox.Show("Update Data Suksess ...");
                                FrmSignIncs_Load(null, null);
                            }
                            else
                            {
                                MessageBox.Show("Gagal Update Data . . . ");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Data Tidak lengkap !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text != "")
                {
                    if (MessageBox.Show("Anda Yakin Menghapus Data Ini ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        query = string.Format("Delete from tbl_pengguna where id_pengguna = '{0}'", txtID.Text);
                        ds.Clear();
                        koneksi.Open();
                        perintah = new MySqlCommand(query, koneksi);
                        adapter = new MySqlDataAdapter(perintah);
                        int res = perintah.ExecuteNonQuery();
                        koneksi.Close();
                        if (res == 1)
                        {
                            MessageBox.Show("Delete Data Suksess ...");
                        }
                        else
                        {
                            MessageBox.Show("Gagal Delete data");
                        }
                    }
                    FrmSignIncs_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Data Yang Anda Pilih Tidak Ada !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                FrmSignIncs_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                FrmLogin frmLogin = new FrmLogin();
                frmLogin.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Picbox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Picbox1.Image = Image.FromFile(openFileDialog1.FileName);
                Picbox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            LblPic.Visible = false;
        }

        private void LblPic_Click(object sender, EventArgs e)
        {

        }

        private void FrmSignIncs_Load(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                query = string.Format("select * from tbl_pengguna");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);
                perintah.ExecuteNonQuery();
                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Width = 100;
                dataGridView1.Columns[0].HeaderText = "ID Pengguna";
                dataGridView1.Columns[1].Width = 150;
                dataGridView1.Columns[1].HeaderText = "Username";
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[2].HeaderText = "Password";
                dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[3].HeaderText = "Nama Pengguna";
                dataGridView1.Columns[4].Width = 120;
                dataGridView1.Columns[4].HeaderText = "Level";

                txtID.Clear();
                txtNamaPengguna.Clear();
                txtPassword.Clear();
                txtUsername.Clear();
                txtID.Focus();
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = true;
                btnSearch.Enabled = true;
                Picbox1.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
