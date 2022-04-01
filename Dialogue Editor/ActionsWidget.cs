using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Dialogue_Editor
{
    class ActionsWidget : MovableNode
    {
        // The right-click menu shared by all ExhangeWidget instances.
        static ContextMenuStrip smMenuStrip = null;

        // Reference to the currently active (clicked-on) exchange.
        static ActionsWidget smActiveActions = null;

        // The box widget containing all other elements of this node.
        private GroupBox mOuterGroup;

        // Reference to this node's parent Node Tree.
        protected NodeTree mParentTree;

        // CONSTRUCTOR
        public ActionsWidget(Point coord, String name, bool offset, Control tabPage, NodeTree nodeTree) : base(NodeType.Type_Actions, name)
        {
            mOuterGroup = new GroupBox();
            mOuterGroup.Location = coord;
            mOuterGroup.Parent = tabPage;
            mOuterGroup.Text = name;

            mParentTree = nodeTree;

            // If our context menu strip hasn't been instantiated yet, do so.
            if (smMenuStrip == null)
            {
                smMenuStrip = new ContextMenuStrip();
                ToolStripMenuItem mnuAddElement = new ToolStripMenuItem("Add Element");
                ToolStripMenuItem mnuAddGoTo = new ToolStripMenuItem("Go to");

                ToolStripMenuItem mnuRename = new ToolStripMenuItem("Rename");
                ToolStripMenuItem mnuRemove = new ToolStripMenuItem("Remove");

                mnuAddElement.DropDown.Items.AddRange(new ToolStripItem[] { mnuAddGoTo });
                smMenuStrip.Items.AddRange(new ToolStripItem[] { mnuAddElement, mnuRename, mnuRemove });

                mnuRename.Click += new EventHandler(_renameClicked);
                mnuRemove.Click += new EventHandler(_removeClicked);
            }

            mOuterGroup.ContextMenuStrip = smMenuStrip;
            mOuterGroup.MouseEnter += new EventHandler(setFocus);
        }

        public override void Dispose()
        {
            if (smActiveActions == this)
                smActiveActions = null;
            mOuterGroup.Dispose();
            mOuterGroup = null;
        }

        public override bool covers(Point p)
        {
            return false;
        }

        public override Point getLocation()
        {
            return new Point();
        }

        public override Point getMousePosOn()
        {
            return new Point();
        }

        public override bool hasWidget(Control widget)
        {
            return false;
        }

        public override void setLocation(Point p)
        {

        }

        public override void setLocationDrag(Point p)
        {

        }

        // Called when the mouse cursor enters this widget's outer box.
        public void setFocus(Object obj, EventArgs e)
        {
            smActiveActions = this;
        }

        // Called when the user clicks 'rename' in our context menu.
        private void rename()
        {
            mParentTree.renameNode(this);
            mOuterGroup.Text = mName;
        }

        // Called when the user clicks 'remove' in our context menu. Returns whether or not removal was completed.
        private bool remove()
        {
            if (mName != "ROOT")
            {
                mParentTree.removeWidget(this);
                return true;
            }

            MessageBox.Show("Cannot remove root node.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        static void _renameClicked(Object obj, EventArgs e)
        {
            if (smActiveActions != null)
                smActiveActions.rename();
        }

        static void _removeClicked(Object obj, EventArgs e)
        {
            if (smActiveActions != null)
            {
                smActiveActions.remove();
                smActiveActions.Dispose();
            }
        }
    }
}
