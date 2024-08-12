﻿using ZorluKartela.Magaza;
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

namespace ZorluKartela.Zorlu {
    public partial class Zorlu_IadeBilgisi : Form {
        public Zorlu_IadeBilgisi() {
            InitializeComponent();
        }

        public void loadReturnedProduct() {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CONN_STR)) {
                try {
                    SqlCommand command = new SqlCommand("s_filter_returninfo", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Magazaadi", txtMagazaAdi.Text.ToString().Trim());
                    command.Parameters.AddWithValue("@Tedarikciadi", txtTedarikciAdi.Text.ToString().Trim());
                    command.Parameters.AddWithValue("@Kartelaadi", txtKartelaAdi.Text.ToString().Trim());
                    command.Parameters.AddWithValue("@Adet", txtAdet.Text.ToString().Trim());
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    try {
                        conn.Open();
                        sda.Fill(ds);
                        dgvIadeEdilenUrunler.DataSource = ds.Tables[0];
                    } catch (SqlException ex) {
                        DialogResult option = MessageBox.Show("Lütfen adet sorgusu için bir sayı giriniz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } finally {
                        conn.Close();
                    }
                } catch (SqlException ex) {
                    conn.Close();
                    MessageBox.Show("Hata oluştu." + ex.ToString());
                }
                dgvIadeEdilenUrunler.Columns[0].Visible = false;
                dgvIadeEdilenUrunler.Columns[1].HeaderText = "SİPARİŞ NO";
                dgvIadeEdilenUrunler.Columns[2].HeaderText = "MAĞAZA ADI";
                dgvIadeEdilenUrunler.Columns[3].HeaderText = "TEDARİKÇİ ADI";
                dgvIadeEdilenUrunler.Columns[4].HeaderText = "KARTELA ADI";
                dgvIadeEdilenUrunler.Columns[5].HeaderText = "MAĞAZA KODU";
                dgvIadeEdilenUrunler.Columns[6].HeaderText = "TEDARİKÇİ KODU";
                dgvIadeEdilenUrunler.Columns[7].HeaderText = "ADET";
                dgvIadeEdilenUrunler.Columns[8].HeaderText = "KARGO NO";
                dgvIadeEdilenUrunler.Columns[9].HeaderText = "İRSALİYE NO";
                dgvIadeEdilenUrunler.Columns[10].HeaderText = "SEVKİYAT TARİHİ";
                dgvIadeEdilenUrunler.Columns[11].HeaderText = "GEREKÇE";
            }
        }

        private void btnCikis_Click(object sender, EventArgs e) {
            DialogResult option = MessageBox.Show("Hesabınızdan çıkış yapılacak, onaylıyor musunuz?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (option == DialogResult.Yes) Application.Exit();
        }

        private void Zorlu_IadeBilgisi_Load(object sender, EventArgs e) {
            label4.Text = KullaniciGiris.Shopname;
            loadReturnedProduct();
        }

        private void btnSorgula_Click(object sender, EventArgs e) {
            loadReturnedProduct();
        }

        private void btnIadeNedeni_Click(object sender, EventArgs e) {
            if (dgvIadeEdilenUrunler.SelectedRows.Count > 0 && dgvIadeEdilenUrunler.SelectedRows[0].Cells[0].Value != null) {
                IadeGerekçesi frmIadeGerekcesi = new IadeGerekçesi();
                frmIadeGerekcesi.OrderID = (int)dgvIadeEdilenUrunler.SelectedRows[0].Cells[1].Value;
                frmIadeGerekcesi.magazaAdi = dgvIadeEdilenUrunler.SelectedRows[0].Cells[2].Value.ToString();
                frmIadeGerekcesi.tedarikciAdi = dgvIadeEdilenUrunler.SelectedRows[0].Cells[3].Value.ToString();
                frmIadeGerekcesi.kartelaAdi = dgvIadeEdilenUrunler.SelectedRows[0].Cells[4].Value.ToString();
                frmIadeGerekcesi.magazaKodu = dgvIadeEdilenUrunler.SelectedRows[0].Cells[5].Value.ToString();
                frmIadeGerekcesi.tedarikciKodu = dgvIadeEdilenUrunler.SelectedRows[0].Cells[6].Value.ToString();
                frmIadeGerekcesi.sevkiyatAdedi = dgvIadeEdilenUrunler.SelectedRows[0].Cells[7].Value.ToString();
                frmIadeGerekcesi.kargoNo = dgvIadeEdilenUrunler.SelectedRows[0].Cells[8].Value.ToString();
                frmIadeGerekcesi.irsaliyeNo = dgvIadeEdilenUrunler.SelectedRows[0].Cells[9].Value.ToString();
                frmIadeGerekcesi.sevkiyatTarihi = dgvIadeEdilenUrunler.SelectedRows[0].Cells[10].Value.ToString();
                frmIadeGerekcesi.iadeNedeni = dgvIadeEdilenUrunler.SelectedRows[0].Cells[11].Value.ToString();
                frmIadeGerekcesi.ShowDialog();
            } else if (dgvIadeEdilenUrunler.SelectedRows.Count > 0 && dgvIadeEdilenUrunler.SelectedRows[0].Cells[0].Value == null) {
                DialogResult option = MessageBox.Show("Lütfen iade nedeni görüntüleyeceğiniz bir sipariş seçiniz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                DialogResult option = MessageBox.Show("Lütfen iade nedeni görüntüleyeceğiniz bir sipariş seçiniz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
