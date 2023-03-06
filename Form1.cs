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


namespace Scanning_Tool__Quality_
{
    public partial class Form1 : Form
    {

        string connectionString = @"Data Source=10.42.6.29;Initial Catalog = ScanningTool;User ID =sa;Password=Password1";
        public Form1()
        {
            InitializeComponent();
            textBox5.Enabled = false;
            textBox5.BackColor = Color.Gray;
            textBox3.Enabled = false;
            textBox3.BackColor = Color.Gray;
            textBox1.Enabled = false;
            textBox1.BackColor = Color.Gray;
            textBox4.Enabled = false;
            textBox4.BackColor = Color.Gray;
            textBox2.Enabled = false;
            textBox2.BackColor = Color.Gray;
            button2.Enabled = false;
            button2.BackColor = Color.Gray;
            pictureBox10.Hide();
            pictureBox4.Hide();
            pictureBox9.Hide();
            pictureBox5.Hide();
            pictureBox8.Hide();
            pictureBox7.Hide();
            pictureBox6.Hide();
            pictureBox11.Hide();
            pictureBox12.Hide();
            label11.Hide();
            label12.Hide();
            label13.Hide();
            label14.Hide();
            label15.Hide();
            TimeUpdater();
            


            if ((Globals.horaMinuto.Hour >= 0) && (Globals.horaMinuto.Hour <= 17 && Globals.horaMinuto.Minute <= 59))
            {
                label16.Text = Globals.primerTurno;
                Globals.primerHoraRev = "00:00:00";
                Globals.segundaHoraRev = "17:59:00";
            }
            else
            {
                label16.Text = Globals.segundoTurno;
                Globals.primerHoraRev = "18:00:00";
                Globals.segundaHoraRev = " 23:59:59";
            }


            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM GM_12LRecParts WHERE RecoveryDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + " " + Globals.primerHoraRev + "' and RecoveryDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + " " + Globals.segundaHoraRev + "'", sqlCon);
                Int32 count = (Int32)cmd2.ExecuteScalar();
                string NumReg = count.ToString();
                label10.Text = NumReg;


                sqlCon.Close();
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Bearing Housing
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pictureBox5.Hide();
                pictureBox9.Hide();
                label12.Hide();
                if (textBox2.Text.Length == 32)
                {
                    bool found = textBox2.Text.ToLower().IndexOf("16001500036") >= 0;
                    if (found)
                    {
                        try
                        {
                            using (SqlConnection sqlCon = new SqlConnection(connectionString))
                            {
                                sqlCon.Open();

                                SqlCommand sqlDa = new SqlCommand("SELECT date FROM GM_12L WHERE BearingHousing = '" + textBox2.Text + "' ", sqlCon);
                                var existe = sqlDa.ExecuteScalar();
                                var result = (existe == null ? "FirstTime" : "Repetido");

                                if (result == "FirstTime")
                                {
                                    pictureBox9.Show();
                                    textBox2.SelectionStart = textBox2.Text.Length;
                                    textBox2.SelectionLength = textBox2.Text.Length;
                                    textBox2.Focus();
                                    label12.Show();
                                    label12.Text = "Este Bearing Housing NO esta en la base de datos productiva";
                                    return;

                                }
                                else if (result == "Repetido")
                                {
                                    SqlCommand sqlLP = new SqlCommand("SELECT linea FROM GM_12L WHERE BearingHousing = '" + textBox2.Text + "' ", sqlCon);
                                    var getLP = sqlLP.ExecuteScalar();
                                    SqlCommand sqlSW = new SqlCommand("SELECT SW FROM GM_12L WHERE BearingHousing = '" + textBox2.Text + "' ", sqlCon);
                                    var getSW = sqlSW.ExecuteScalar();
                                    textBox3.Text = getSW.ToString();
                                    SqlCommand sqlBC = new SqlCommand("SELECT BearingCover FROM GM_12L WHERE BearingHousing = '" + textBox2.Text + "' ", sqlCon);
                                    var getBC = sqlBC.ExecuteScalar();
                                    textBox1.Text = getBC.ToString();
                                    SqlCommand sqlCW = new SqlCommand("SELECT CompressorWheel FROM GM_12L WHERE BearingHousing = '" + textBox2.Text + "' ", sqlCon);
                                    var getCW = sqlCW.ExecuteScalar();
                                    textBox4.Text = getCW.ToString();

                                    Globals.lineaPieza = getLP.ToString();
                                    pictureBox5.Show();
                                    textBox3.BackColor = Color.White;
                                    textBox5.Text = existe.ToString();
                                    textBox5.BackColor = Color.White;
                                    textBox1.BackColor = Color.White;
                                    textBox4.BackColor = Color.White;
                                    textBox3.Focus();
                                    button2.Enabled = true;
                                    button2.BackColor = Color.FromArgb(17, 133, 61);
                                }

                            }



                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            MessageBox.Show(error, "Error registrando datos BearingHousing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        pictureBox9.Show();
                        textBox2.SelectionStart = 0;
                        textBox2.SelectionLength = textBox2.Text.Length;
                        label12.Show();
                        label12.Text = "Pieza escaneada NO es un Bearing Housing";
                        textBox3.Enabled = false;
                        textBox3.BackColor = Color.Gray;

                    }
                }
                else
                {
                    textBox3.Enabled = false;
                    textBox3.BackColor = Color.Gray;
                }
            }

        }


        // Boton Borrar
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox5.BackColor = Color.Gray;
            button2.Enabled = false;
            button2.BackColor = Color.Gray;
            textBox1.BackColor = Color.Gray;
            textBox4.BackColor = Color.Gray;
            if (radioButton1.Checked)
            {
                textBox3.BackColor = Color.Gray;
                textBox2.Focus();
            }
            if (radioButton2.Checked)
            {
                textBox2.BackColor = Color.Gray;
                textBox3.Focus();
            }
        }

        // Boton enviar
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {


                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlCommand cmd = new SqlCommand("RecoverPart", sqlCon);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RecoveryDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OriginalDate", textBox5.Text.ToString());
                        cmd.Parameters.AddWithValue("@BearingHousing", textBox2.Text.ToString());
                        cmd.Parameters.AddWithValue("@SW", textBox3.Text.ToString());
                        cmd.Parameters.AddWithValue("@BearingCover", textBox1.Text.ToString());
                        cmd.Parameters.AddWithValue("@CompressorWheel", textBox4.Text.ToString());
                        cmd.Parameters.AddWithValue("@linea", "Recuperado con BH");
                        cmd.ExecuteNonQuery();
                        SqlCommand sqlDa = new SqlCommand("DELETE FROM GM_12L WHERE CompressorWheel = '" + textBox4.Text + "' ", sqlCon);
                        sqlDa.ExecuteNonQuery();
                        SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM GM_12LRecParts WHERE RecoveryDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + " " + Globals.primerHoraRev + "' AND RecoveryDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + " " + Globals.segundaHoraRev + "' ", sqlCon);
                        Int32 count = (Int32)cmd2.ExecuteScalar();
                        string NumReg = count.ToString();
                        label10.Text = NumReg;
                    }

                    //textBox1.Text = "";
                    //textBox2.Text = "";
                    //textBox3.Text = "";
                    //textBox4.Text = "";
                    //textBox5.Text = "";
                    //textBox5.BackColor = Color.Gray;
                    //textBox3.BackColor = Color.Gray;
                    //textBox1.BackColor = Color.Gray;
                    //textBox4.BackColor = Color.Gray;

                    label11.Visible = true;
                    System.Threading.Tasks.Task.Delay(2500).ContinueWith(_ =>
                    {
                        Invoke(new MethodInvoker(() => { label11.Visible = false; }));
                    });

                    pictureBox4.Visible = true;
                    System.Threading.Tasks.Task.Delay(2500).ContinueWith(_ =>
                    {
                        Invoke(new MethodInvoker(() => { pictureBox4.Visible = false; }));
                    });

                    button2.Enabled = false;
                    button2.BackColor = Color.Gray;
                    button1.PerformClick();
                }

                if (radioButton2.Checked)
                {


                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlCommand cmd = new SqlCommand("RecoverPart", sqlCon);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RecoveryDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OriginalDate", textBox5.Text.ToString());
                        cmd.Parameters.AddWithValue("@BearingHousing", textBox2.Text.ToString());
                        cmd.Parameters.AddWithValue("@SW", textBox3.Text.ToString());
                        cmd.Parameters.AddWithValue("@BearingCover", textBox1.Text.ToString());
                        cmd.Parameters.AddWithValue("@CompressorWheel", textBox4.Text.ToString());
                        cmd.Parameters.AddWithValue("@linea", "Recuperado con SW");
                        cmd.ExecuteNonQuery();
                        SqlCommand sqlDa = new SqlCommand("DELETE FROM GM_12L WHERE CompressorWheel = '" + textBox4.Text + "' ", sqlCon);
                        sqlDa.ExecuteNonQuery();
                        SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM GM_12LRecParts WHERE RecoveryDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + " " + Globals.primerHoraRev + "' AND RecoveryDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + " " + Globals.segundaHoraRev + "' ", sqlCon);
                        Int32 count = (Int32)cmd2.ExecuteScalar();
                        string NumReg = count.ToString();
                        label10.Text = NumReg;
                    }

                    //textBox1.Text = "";
                    //textBox2.Text = "";
                    //textBox3.Text = "";
                    //textBox4.Text = "";
                    //textBox5.Text = "";
                    //textBox5.BackColor = Color.Gray;
                    //textBox3.BackColor = Color.Gray;
                    //textBox1.BackColor = Color.Gray;
                    //textBox4.BackColor = Color.Gray;

                    label11.Visible = true;
                    System.Threading.Tasks.Task.Delay(2500).ContinueWith(_ =>
                    {
                        Invoke(new MethodInvoker(() => { label11.Visible = false; }));
                    });

                    pictureBox4.Visible = true;
                    System.Threading.Tasks.Task.Delay(2500).ContinueWith(_ =>
                    {
                        Invoke(new MethodInvoker(() => { pictureBox4.Visible = false; }));
                    });

                    button2.Enabled = false;
                    button2.BackColor = Color.Gray;
                    button1.PerformClick();
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                MessageBox.Show(error, "Error registrando valores en base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // Para tener el timer que se actualiza en la pantalla principal
        async void TimeUpdater()
        {
            while (true)
            {
                label8.Text = DateTime.Now.ToString();
                await Task.Delay(1000);
            }
        }

        // Para evitar las acciones del enter automatico de los scanners
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                e.Handled = true;

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void radioButton2_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down))
            {
                e.SuppressKeyPress = true;
            }
        }

        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down))
            {
                e.SuppressKeyPress = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton1.ForeColor = Color.FromArgb(32, 191, 85);
                textBox2.Enabled = true;
                textBox2.BackColor = Color.White;
                textBox2.Focus();
            }
            else if (radioButton1.Checked == false)
            {
                radioButton1.ForeColor = Color.White;
                textBox2.Enabled = false;
                textBox2.BackColor = Color.Gray;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton2.ForeColor = Color.FromArgb(32, 191, 85);
                textBox3.Enabled = true;
                textBox3.BackColor = Color.White;
                textBox3.Focus();
            }
            else if (radioButton2.Checked == false)
            {
                radioButton2.ForeColor = Color.White;
                textBox3.Enabled = false;
                textBox3.BackColor = Color.Gray;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                pictureBox6.Hide();
                pictureBox10.Hide();
                label13.Hide();
                if (textBox3.Text.Length == 32)
                {
                    bool found = textBox3.Text.ToLower().IndexOf("16001210066") >= 0;
                    if (found)
                    {
                        try
                        {
                            using (SqlConnection sqlCon = new SqlConnection(connectionString))
                            {
                                sqlCon.Open();

                                SqlCommand sqlDa = new SqlCommand("SELECT date FROM GM_12L WHERE SW = '" + textBox3.Text + "' ", sqlCon);
                                var existe = sqlDa.ExecuteScalar();
                                var result = (existe == null ? "FirstTime" : "Repetido");

                                if (result == "FirstTime")
                                {
                                    pictureBox10.Show();
                                    textBox3.SelectionStart = textBox2.Text.Length;
                                    textBox3.SelectionLength = textBox2.Text.Length;
                                    textBox3.Focus();
                                    label13.Show();
                                    label13.Text = "Este Shaft and Wheel NO esta en la base de datos productiva";
                                    return;

                                }
                                else if (result == "Repetido")
                                {
                                    SqlCommand sqlLP = new SqlCommand("SELECT linea FROM GM_12L WHERE SW = '" + textBox3.Text + "' ", sqlCon);
                                    var getLP = sqlLP.ExecuteScalar();
                                    SqlCommand sqlSW = new SqlCommand("SELECT BearingHousing FROM GM_12L WHERE SW = '" + textBox3.Text + "' ", sqlCon);
                                    var getSW = sqlSW.ExecuteScalar();
                                    textBox2.Text = getSW.ToString();
                                    SqlCommand sqlBC = new SqlCommand("SELECT BearingCover FROM GM_12L WHERE SW = '" + textBox3.Text + "' ", sqlCon);
                                    var getBC = sqlBC.ExecuteScalar();
                                    textBox1.Text = getBC.ToString();
                                    SqlCommand sqlCW = new SqlCommand("SELECT CompressorWheel FROM GM_12L WHERE SW = '" + textBox3.Text + "' ", sqlCon);
                                    var getCW = sqlCW.ExecuteScalar();
                                    textBox4.Text = getCW.ToString();

                                    Globals.lineaPieza = getLP.ToString();
                                    pictureBox6.Show();
                                    textBox2.BackColor = Color.White;
                                    textBox5.Text = existe.ToString();
                                    textBox5.BackColor = Color.White;
                                    textBox1.BackColor = Color.White;
                                    textBox4.BackColor = Color.White;
                                    button2.Enabled = true;
                                    button2.BackColor = Color.FromArgb(17, 133, 61);
                
                                }

                            }



                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                            MessageBox.Show(error, "Error registrando datos Shaft and Wheel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        pictureBox10.Show();
                        textBox3.SelectionStart = 0;
                        textBox3.SelectionLength = textBox2.Text.Length;
                        label13.Show();
                        label13.Text = "Pieza escaneada NO es un Shaft And Wheel";
                        textBox2.Enabled = false;
                        textBox2.BackColor = Color.Gray;

                    }
                }
            }
        }
    }

    // MARK - Globales de hora
    internal static class Globals
    {
        public static DateTime horaMinuto = DateTime.Now;
        public static string primerTurno = "1er turno";
        public static string segundoTurno = "2do turno";
        public static string primerHoraRev;
        public static string segundaHoraRev;
        public static string lineaPieza;

    }

}
