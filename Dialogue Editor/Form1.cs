using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Dialogue_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            EditorTabs.getSingleton().initialise(EditorTabsControl);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorTabs.getSingleton().createTabPage();            
        }
    }
}
