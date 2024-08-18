using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VimPig
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
            panel1.Paint += (sender, e) =>
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    // Çàäàíèå ðàäèóñà çàêðóãëåíèÿ
                    int radius = 20;

                    // Ñîçäàíèå ïðÿìîóãîëüíèêà ñ çàêðóãëåííûìè óãëàìè
                    path.StartFigure();
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(panel1.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(panel1.Width - radius, panel1.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, panel1.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    // Ïðèìåíåíèå îáðåçêè
                    panel1.Region = new Region(path);
                }
            };
        }
        private void Scintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.E)
            {
                this.Close();
                UpdateStattodef();
                e.SuppressKeyPress = true;
                return;
            }
        }
        public void UpdateStattodef()
        {
            if (Form1.isCommandMode)
            {
                RPC.SetState("Writing Commands..", true);
            } else
            {
                RPC.SetState("Editing..", true);
            }
        }
    }
}
