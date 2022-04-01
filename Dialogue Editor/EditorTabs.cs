using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Dialogue_Editor
{
    class EditorTabs
    {
        // Singleton reference.
        private static EditorTabs sEditorTabs = null;

        // Get singleton reference.
        public static EditorTabs getSingleton()
        {
            if (sEditorTabs == null)
                sEditorTabs = new EditorTabs();
            return sEditorTabs;
        }

        // CONSTRUCTOR
        private EditorTabs()
        {
            mTabsList = new List<Tab>();

            // Create context menu.
            mContextMenu = new ContextMenuStrip();
            ToolStripMenuItem mnuAddExchange = new ToolStripMenuItem("Add Exchange");
            ToolStripMenuItem mnuAddAction = new ToolStripMenuItem("Add Actions");
            ToolStripMenuItem mnuAddNote = new ToolStripMenuItem("Add Note");

            mnuAddExchange.Click += new EventHandler(addExchangeClicked);
            mnuAddAction.Click += new EventHandler(addActionsClicked);
            mnuAddNote.Click += new EventHandler(addNoteClicked);

            mContextMenu.Items.AddRange(new ToolStripItem[] { mnuAddExchange, mnuAddAction, mnuAddNote });
        }

        // Context menu to apply to all tabs.
        private ContextMenuStrip mContextMenu = null;

        // Forms TabControl
        private TabControl mTabControl = null;

        // Set reference to tab control.
        public void initialise(TabControl tc)
        {
            mTabControl = tc;
            createTabPage();
        }

        // Create a new empty tab.
        public Tab createTabPage()
        {
            TabPage newTabPage = new TabPage("New Tab");
            newTabPage.ContextMenuStrip = mContextMenu;
            newTabPage.BackColor = Color.White;

            // Scroll bars
            newTabPage.VerticalScroll.Enabled = true;
            newTabPage.VerticalScroll.Visible = true;
            newTabPage.HorizontalScroll.Enabled = true;
            newTabPage.HorizontalScroll.Visible = true;
            
            mTabControl.TabPages.Add(newTabPage);
            mTabControl.SelectedTab = newTabPage;

            mTabsList.Add(new Tab(newTabPage));
            return mTabsList.Last();
        }

        // The list of current tabs.
        private List<Tab> mTabsList;

        // Removes the given tab from our list of active tabs.
        public void removeTab(Tab toRemove)
        {
            if (mTabsList.Remove(toRemove))
                mTabControl.TabPages.Remove(toRemove.mTabPage);
        }

        // Return the tab which currently has focus.
        public Tab getCurrentTab()
        {
            foreach (Tab t in mTabsList)
                if (mTabControl.SelectedTab == t.mTabPage)
                    return t;
            return null;
        }



        // Called when context menu clicked.
        private void addExchangeClicked(object sender, EventArgs e)
        {
            String name = "";
            if (InputBox("Node Name", "Enter a name for the new exchange node, or leave blank to have a name automatically generated.", ref name) == DialogResult.OK)
                getCurrentTab().addExchangeClicked(sender, e, mContextMenu.Bounds.Location, name);
        }

        // Called when context menu clicked.
        private void addActionsClicked(object sender, EventArgs e)
        {
            String name = "";
            if (InputBox("Node Name", "Enter a name for the new actions node, or leave blank to have a name automatically generated.", ref name) == DialogResult.OK)
                getCurrentTab().addActionsClicked(sender, e, mContextMenu.Bounds.Location, name);
        }

        // Called when context menu clicked.
        private void addNoteClicked(object sender, EventArgs e)
        {            
            getCurrentTab().addNoteClicked(sender, e, mContextMenu.Bounds.Location);
        }

        // Open an input box with the given text.
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}
