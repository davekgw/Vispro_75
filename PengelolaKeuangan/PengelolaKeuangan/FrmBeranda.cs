using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PengelolaKeuangan
{
    public partial class FrmBeranda : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat;

        public FrmBeranda()
        {
            alamat = "server=localhost; database=db_mahasiswa; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void FrmBeranda_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close(); // Tutup koneksi jika sudah terbuka
                }
                koneksi.Open();
                string query = "SELECT * FROM tbl_catatan";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, koneksi);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView.DataSource = ds.Tables[0];
                dataGridView.Columns[0].Width = 350;
                dataGridView.Columns[0].HeaderText = "Catatan";
                dataGridView.Columns[1].Width = 150;
                dataGridView.Columns[1].HeaderText = "Jumlah";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
            finally
            {
                koneksi.Close(); // Pastikan koneksi ditutup
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCatatan.Text) || string.IsNullOrWhiteSpace(txtJumlah.Text))
            {
                MessageBox.Show("Silakan isi semua field.");
                return;
            }

            try
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close(); // Tutup koneksi jika sudah terbuka
                }
                koneksi.Open();
                string query = "INSERT INTO tbl_catatan (nama_catatan, jumlah) VALUES (@nama_catatan, @jumlah)";
                using (MySqlCommand command = new MySqlCommand(query, koneksi))
                {
                    command.Parameters.AddWithValue("@nama_catatan", txtCatatan.Text);
                    command.Parameters.AddWithValue("@jumlah", Convert.ToDecimal(txtJumlah.Text));
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Catatan berhasil ditambahkan.");
                LoadData();
                txtCatatan.Clear();
                txtJumlah.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
            finally
            {
                koneksi.Close(); // Pastikan koneksi ditutup
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Ambil nama_catatan dari baris yang dipilih
                string namaCatatan = dataGridView.SelectedRows[0].Cells[0].Value.ToString();

                if (string.IsNullOrWhiteSpace(txtCatatan.Text) || string.IsNullOrWhiteSpace(txtJumlah.Text))
                {
                    MessageBox.Show("Silakan isi semua field.");
                    return;
                }

                try
                {
                    if (koneksi.State == ConnectionState.Open)
                    {
                        koneksi.Close(); // Tutup koneksi jika sudah terbuka
                    }
                    koneksi.Open();
                    string query = "UPDATE tbl_catatan SET nama_catatan = @nama_catatan, jumlah = @jumlah WHERE nama_catatan = @old_nama_catatan";
                    using (MySqlCommand command = new MySqlCommand(query, koneksi))
                    {
                        command.Parameters.AddWithValue("@old_nama_catatan", namaCatatan);
                        command.Parameters.AddWithValue("@nama_catatan", txtCatatan.Text);
                        command.Parameters.AddWithValue("@jumlah", Convert.ToDecimal(txtJumlah.Text));
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Catatan berhasil diperbarui.");
                    LoadData();
                    txtCatatan.Clear();
                    txtJumlah.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                }
                finally
                {
                    koneksi.Close(); // Pastikan koneksi ditutup
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih catatan yang ingin diedit.");
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Anda yakin ingin menghapus data yang dipilih?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        if (koneksi.State == ConnectionState.Open)
                        {
                            koneksi.Close(); // Tutup koneksi jika sudah terbuka
                        }
                        koneksi.Open();

                        foreach (DataGridViewRow row in dataGridView.SelectedRows)
                        {
                            string namaCatatan = row.Cells[0].Value.ToString();
                            string query = "DELETE FROM tbl_catatan WHERE nama_catatan = @nama_catatan";
                            using (MySqlCommand command = new MySqlCommand(query, koneksi))
                            {
                                command.Parameters.AddWithValue("@nama_catatan", namaCatatan);
                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Data berhasil dihapus.");
                        LoadData();
                        txtCatatan.Clear();
                        txtJumlah.Clear();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Terjadi kesalahan saat menghapus catatan: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                    }
                    finally
                    {
                        koneksi.Close(); // Pastikan koneksi ditutup
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih catatan yang ingin dihapus.");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}