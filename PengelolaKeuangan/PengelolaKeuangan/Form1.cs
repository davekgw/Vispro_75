﻿using System;
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

namespace PengelolaKeuangan
{
    public partial class FrmLogin : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;

        private DataSet ds = new DataSet();
        private string alamat, query;
        public FrmLogin()
        {
            alamat = "server=localhost; database=db_mahasiswa; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void btnSignin_Click(object sender, EventArgs e)
        {
            FrmSignIncs frmSignIncs = new FrmSignIncs();
            frmSignIncs.Show();
            this.Hide();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                query = string.Format("select * from tbl_pengguna where username = '{0}'", txtUsername.Text);
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
                        string sandi;
                        sandi = kolom["password"].ToString();
                        if (sandi == txtPassword.Text)
                        {
                            FrmMain frmMain = new FrmMain();
                            frmMain.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Anda salah input password");
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Username tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
