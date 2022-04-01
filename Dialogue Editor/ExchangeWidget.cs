using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Dialogue_Editor
{
    class ExchangeWidget : MovableNode
    {
        // The right-click menu shared by all ExhangeWidget instances.
        static ContextMenuStrip smMenuStrip = null;

        // Reference to the currently active (clicked-on) exchange.
        static ExchangeWidget smActiveExchange = null;

        // The box widget containing all other elements of this node.
        private GroupBox mOuterGroup;

        // Reference to this node's parent Node Tree.
        protected NodeTree mParentTree;

        // CONSTRUCTOR
        public ExchangeWidget(Point coord, String name, bool offset, Control tabPage, NodeTree nodeTree) : base(NodeType.Type_Exchange, name)
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
                    ToolStripMenuItem mnuAddSetState = new ToolStripMenuItem("Set State");
                    ToolStripMenuItem mnuAddBody = new ToolStripMenuItem("Body");
                    ToolStripMenuItem mnuAddReply = new ToolStripMenuItem("Reply");
                    ToolStripMenuItem mnuAddGoTo = new ToolStripMenuItem("Go to");
                ToolStripMenuItem mnuRename = new ToolStripMenuItem("Rename");
                ToolStripMenuItem mnuRemove = new ToolStripMenuItem("Remove");

                mnuAddElement.DropDown.Items.AddRange(new ToolStripItem[] { mnuAddSetState, mnuAddBody, mnuAddReply, mnuAddGoTo });
                smMenuStrip.Items.AddRange(new ToolStripItem[] { mnuAddElement, mnuRename, mnuRemove });

                mnuRename.Click += new EventHandler(_renameClicked);
                mnuRemove.Click += new EventHandler(_removeClicked);
            }

            mOuterGroup.ContextMenuStrip = smMenuStrip;
            mOuterGroup.MouseEnter += new EventHandler(setFocus);
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

        public override void setLocation(Point p)
        {

        }

        public override void setLocationDrag(Point p)
        {

        }

        public override bool hasWidget(Control widget)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            if (smActiveExchange == this)
                smActiveExchange = null;
            mOuterGroup.Dispose();
            mOuterGroup = null;
        }

        // Called when the mouse cursor enters this widget's outer box.
        public void setFocus(Object obj, EventArgs e)
        {
            smActiveExchange = this;
        }

        // Called when the user clicks 'Set State' in our context menu.
        private void addSetState()
        {

        }

        // Called when the user clicks 'Body' in our context menu.
        private void addBody()
        {

        }

        // Called when the user clicks 'Reply' in our context menu.
        private void addReply()
        {

        }

        // Called when the user clicks 'Go To' in our context menu.
        private void addGoTo()
        {

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

        static void _addSetState(Object obj, EventArgs e)
        {
            if (smActiveExchange != null)
                smActiveExchange.addSetState();
        }

        static void _addBody(Object obj, EventArgs e)
        {
            if (smActiveExchange != null)
                smActiveExchange.addBody();
        }

        static void _addReply(Object obj, EventArgs e)
        {
            if (smActiveExchange != null)
                smActiveExchange.addReply();
        }

        static void _addGoTo(Object obj, EventArgs e)
        {
            if (smActiveExchange != null)
                smActiveExchange.addGoTo();
        }

        static void _renameClicked(Object obj, EventArgs e)
        {
            if (smActiveExchange != null)
                smActiveExchange.rename();
        }

        static void _removeClicked(Object obj, EventArgs e)
        {
            if (smActiveExchange != null)
            {
                smActiveExchange.remove();
                smActiveExchange.Dispose();
            }
        }
}
}
