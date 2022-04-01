using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Dialogue_Editor
{
    class Tab
    {
        // The page Forms page belonging to this tab.
        public readonly TabPage mTabPage = null;

        // Whether this tab has unsaved changes.
        private bool mUnsavedChanges = false, mModified = false;

        // The name of the dialogue open on this tab.
        private String mDialogueName;

        // The tree of exchange and action nodes on this tab.
        private NodeTree mNodeTree;
        
        // CONSTRUCTOR
        public Tab(TabPage tabPage)
        {
            mTabPage = tabPage;
            mDialogueName = mTabPage.Text;
            mNodeTree = new NodeTree(this);
        }

        // Set whether or not this tab has unsaved changes.
        public void setUnsavedChanges(bool hasChanged)
        {
            if (hasChanged != mUnsavedChanges)
            {
                mModified = true;
                mUnsavedChanges = hasChanged;
                changeName(mDialogueName);
            }
        }

        // Change the name of this tab.
        public void changeName(String name)
        {
            if (name.Count() > 4 && (name.Substring(name.Count() - 4, 4).Equals(".xml", StringComparison.OrdinalIgnoreCase) || name.Substring(name.Count() - 4, 4).Equals(".dlg", StringComparison.OrdinalIgnoreCase)))
                name = name.Substring(0, name.Count() - 4);
            mDialogueName = name;

            if (mUnsavedChanges)
                mTabPage.Text = mDialogueName + '*';
            else
                mTabPage.Text = mDialogueName;
        }

        // Called when context menu clicked.
        public void addExchangeClicked(object sender, EventArgs e, Point point, String name)
        {
            mNodeTree.addExchange(mTabPage.PointToClient(point), name, false, false, mTabPage);
        }

        // Called when context menu clicked.
        public void addActionsClicked(object sender, EventArgs e, Point point, String name)
        {
            mNodeTree.addActions(mTabPage.PointToClient(point), name, false, false, mTabPage);
        }

        // Called when context menu clicked.
        public void addNoteClicked(object sender, EventArgs e, Point point)
        {

        }

        public Point screenToControl(Point point)
        {
            Form form = mTabPage.FindForm();
            if (form != null)
            {
                new Point(point.X - form.Location.X, point.Y - form.Location.Y);
            }
            return new Point();
        }
    }
}
