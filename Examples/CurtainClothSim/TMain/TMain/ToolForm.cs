using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TMain {
    public partial class ToolForm : Form {
        
        private Globals g;
        private RenderWrapper rw;

        private class comboItem {
            public string name;
            public int value;
            public comboItem() { }
            public comboItem(string n, int v) {
                name = n;
                value = v;
            }
        }

        public ToolForm() {
            g = Globals.Instance;
            rw = RenderWrapper.Instance;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            // set modo di interazione: muovi nodi tenda 
            g.ui_mode = 1;
        }

        private void button2_Click(object sender, EventArgs e) {
            // set modo di interazione: track view
            g.ui_mode = 0;
        }

        private void button3_Click_1(object sender, EventArgs e) {
            rw.SwitchRenderingMode();
        }

        private void button4_Click(object sender, EventArgs e) {
            g.ui_mode = 2;
        }

        private void button5_Click(object sender, EventArgs e) {
            g.ui_mode = -1;
            rw.SetMouseAnchorVisibility(false); 
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = ".\\Textures\\";
            ofd.Filter = "Bitmap|*.bmp|All|*.*";
            //ofd.ShowDialog();
            if (ofd.ShowDialog () != DialogResult.OK) { 
                return;
            } else {
                try {
                    Bitmap bmp = new Bitmap(ofd.FileName);
                    rw.SetWallTexture(bmp);
                } catch (Exception) {
                    MessageBox.Show ("Error opening file");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e) {
            g.ui_mode = -1;
            rw.SetMouseAnchorVisibility(false);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = ".\\Textures\\";
            ofd.Filter = "Bitmap|*.bmp|All|*.*";
            //ofd.ShowDialog();
            if(ofd.ShowDialog() != DialogResult.OK) {
                return;
            } else {
                try {
                    Bitmap bmp = new Bitmap(ofd.FileName);
                    rw.SetAwningTexture(bmp);
                } catch(Exception) {
                    MessageBox.Show("Error opening file");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e) {
            g.ui_mode = 3;
        }

        // gestione dell'oggetto attivo
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            rw.SetSelected(comboBox1.SelectedIndex);
            rw.SetMouseAnchorVisibility(false); 
        }

        // aggiunta di un oggeto in tessuto
        private void button8_Click(object sender, EventArgs e) {
            try {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InstalledUICulture;
                System.Globalization.NumberFormatInfo ni = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();
                ni.NumberDecimalSeparator = ".";
                // aggiunta al menu
                string new_awn_name = "Tenda " + comboBox1.Items.Count;
                comboBox1.Items.Add(new_awn_name);
                rw.AddNewAwning(float.Parse(textBoxDimX.Text, ni),
                                float.Parse(textBoxDimY.Text, ni),
                                float.Parse(textBoxPosX.Text, ni),
                                float.Parse(textBoxPosY.Text, ni),
                                float.Parse(textBoxPosZ.Text, ni));
                rw.SetMouseAnchorVisibility(false);
            } catch (Exception) {
                MessageBox.Show("Error adding awning");
            }
        }

        // rimozione di un oggetto
        private void buttonDeleteAwn_Click(object sender, EventArgs e) {
            if(comboBox1.Items.Count > 1) {
                rw.DeleteLastAwning();
                comboBox1.Items.RemoveAt(comboBox1.Items.Count - 1);
                /*
                rw.SetMouseAnchorVisibility(false);
                rw.DeleteSelectedAwning();
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
                comboBox1.SelectedIndex = 0;
                rw.SetSelected(comboBox1.SelectedIndex);
                 */
            }
        }
    }
}