using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace TMain
{
    public partial class MainForm : Form
    {

        private ToolForm tf;
        private Globals g;
        private RenderWrapper rw;
        private AboutForm af;
        private Timer timer;

        public MainForm() {

            rw = RenderWrapper.Instance;
            //rw = new RenderWrapper();
            tf = new ToolForm();
            tf.Show();
            g = Globals.Instance;
            g.getOptions();
            
          
            InitializeComponent();
            this.simpleOpenGlControl1.InitializeContexts();
            // timer control
            timer = new Timer();
            timer.Tick += new EventHandler(OnTimer);
            timer.Enabled = true;
            timer.Interval = g.clock;
        }

        // eventi principali di rendering //////////////////////////////////////////////
        private void simpleOpenGlControl1_Load(object sender, EventArgs e) {
            rw.Init(this.simpleOpenGlControl1.Width, this.simpleOpenGlControl1.Height);
            rw.ResetTrackMouse(0.5f, 0.5f); 
            rw.Render();
        }

        private void simpleOpenGlControl1_Resize(object sender, EventArgs e) {
            Control control = (Control)sender;
            control.Size = new Size(control.Size.Width, control.Size.Height);
            rw.Reshape(control.Size.Width, control.Size.Height);
            rw.Render();
        }

        // callback eventi mouse //////////////////////////////////////////////////////
        private void simpleOpenGlControl1_MouseMove(object sender, MouseEventArgs e) {
            Control control = (Control)sender;
            float dx = e.X / (float)control.Size.Height;
            float dy = 1.0f - e.Y / (float)control.Size.Height;
            
            if(g.ui_mode == 0) { // modalità trackball
                if(e.Button == MouseButtons.Left) {
                    rw.TrackView(dx, dy);   
                } else if(e.Button == MouseButtons.Right) {
                } else if(e.Button == MouseButtons.Middle) {
                    rw.StrafeView(dx, dy);
                }
            } else if(g.ui_mode == 1) {// modalità drappeggio
                if(e.Button == MouseButtons.Left) {
                    rw.MoveMouseAnchor(((float)e.X) / ((float)control.Size.Width), 1.0f - ((float)e.Y / (float)control.Size.Height));
                }
            } else if(g.ui_mode == 2) { // modalità rimozione ancora
                if(e.Button == MouseButtons.Left) {
                    rw.RemoveAnchor(((float)e.X) / ((float)control.Size.Width), 1.0f - ((float)e.Y / (float)control.Size.Height));
                }
            } else if(g.ui_mode == 3) { // modalità movimento luce
                if(e.Button == MouseButtons.Left) {
                    rw.TrackLight(dx, dy);
                }
            }
            simpleOpenGlControl1.Refresh();
            rw.ResetTrackMouse(dx, dy);
        }

        private void simpleOpenGlControl1_MouseDown(object sender, MouseEventArgs e) {
            Control control = (Control)sender;
            float dx = e.X / (float)control.Size.Height;
            float dy = 1.0f - e.Y / (float)control.Size.Height;
              if(g.ui_mode == 1) {
                rw.FindNearestAnchor((float)e.X/(float)control.Size.Width, 1.0f-((float)e.Y/(float)control.Size.Height) );
                rw.SetMouseAnchorVisibility(true);
            } else { // if(g.ui_mode == 0) {
                rw.SetMouseAnchorVisibility(false);
                rw.ResetTrackMouse(dx, dy);
            }
        }

        private void simpleOpenGlControl1_MouseUp(object sender, MouseEventArgs e) {
        }

        private void simpleOpenGlControl1_MouseWheel(object sender, MouseEventArgs e) {
            float dy = (float)e.Delta / 200.0f;
            if(g.ui_mode == 0) { // modalità trackball
                rw.StrafeView(0.0f, dy);
            }
        }

        // evento timer: step simulazione //////////////////////////////////////////////
        private void OnTimer(object sender, EventArgs e) {
            rw.OnTimer();
            simpleOpenGlControl1.Refresh();
            //simpleOpenGlControl1.Focus();
            //simpleOpenGlControl1.Invalidate();

        }

        // evento tastiera /////////////////////////////////////////////////////////////
        private void simpleOpenGlControl1_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == 'w') {
                // SwitchRenderingMode();
                rw.SwitchRenderingMode();          
            }
        }

        // callback eventi menu strip ////////////////////////////////////////////////
        private void resetToolStripMenuItem_Click(object sender, EventArgs e) {
            rw.ResetSimulation();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            af = new AboutForm();
            af.Show();
        }

        private void giornostandardToolStripMenuItem_Click(object sender, EventArgs e) {
            rw.SetAmbientType(1);
        }

        private void notteToolStripMenuItem_Click(object sender, EventArgs e) {
            rw.SetAmbientType(0);
        }

        // esportazione immagine
        private void salvaToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = ".\\";
            sfd.Filter = "Bitmap|*.bmp|All|*.*";
            sfd.ShowDialog();
            try {
                Bitmap bmp = new Bitmap(simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
                // se salvo tutto il form vedo solo i controlli!!
                // this.DrawToBitmap(bmp, Rectangle.FromLTRB(0, 0, this.Width, this.Hright));
                // la drawtobitmap non va con il simpleopengl...
                this.simpleOpenGlControl1.DrawToBitmap(bmp, Rectangle.FromLTRB(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height));
                bmp.Save(sfd.FileName, ImageFormat.Bmp);
            } catch(Exception) {
                MessageBox.Show("Error saving file");
            }
            
        }

        // gestione toolbar
        private void toolStripButton_newSimulation_Click(object sender, EventArgs e) {
            // reset simulazione
        }

        // eventi ancora da riempire ///////////////////////////////////////////////////
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void MainForm_Load(object sender, EventArgs e) {
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

    }

}