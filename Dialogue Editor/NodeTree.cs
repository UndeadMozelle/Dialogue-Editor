using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

// TODO Captioned
namespace Dialogue_Editor
{
    class NodeTree
    {
        // List of all exchange widgets belonging to this tree.
        private List<MovableNode> mNodeList;

        // The number of exchange nodes which has been created.
        private int mNumExchanges = 0;

        // List of all notes belonging to this tree.
        private List<NoteWidget> mNotesList;
	
	    // Our parent tab.
	    public Tab mParentTab;
	
	    // The farthest left exchange widget.
	    protected MovableNode mLeft = null;

        // The farthest up exchange widget.
        protected MovableNode mTop = null;

        // The farthest down exchange widget.
        protected MovableNode mBottom = null;

        // The farthest right exchange widget.
        protected MovableNode mRight = null;


        // CONSTRUCTOR.
        public NodeTree(Tab parentTab)
        {
            mParentTab = parentTab;
            //mEditorSheet = mParentTab.getEditorSheet();
            mNumExchanges = 0;
            mNodeList = new List<MovableNode>();
            mNotesList = new List<NoteWidget>();
        }

        // Dispose of all widgets.
        public void close()
        {
            for (int i = 0; i < mNodeList.Count(); ++i)
                mNodeList.ElementAt(i).close(false);
            for (int i = 0; i < mNotesList.Count(); ++i)
                mNotesList.ElementAt(i).close(false);
        }

        // Automatically generates a generic name for a new node based on the number of nodes in this tree.
        public String getAutoName(MovableNode.NodeType type)
        {
            String name;
            if (type == MovableNode.NodeType.Type_Exchange)
                name = "Exchange_";
            else if (type == MovableNode.NodeType.Type_Actions)
                name = "Actions_";
            else
                name = "Node_";

            // Find a suffix we can use for the name.
            int i = mNumExchanges;
            while (!verifyName(name+i.ToString(), false, true))
                ++i;

            return name+i.ToString();
        }

        // Create a new, empty node at the given coordinate with the given name.
        public ExchangeWidget addExchange(Point coord, String name, Boolean offset, Boolean suppressWarning, TabPage parent)
        {
            if (name.Equals(""))
                name = getAutoName(MovableNode.NodeType.Type_Exchange);
            else if (!verifyName(name, true, true))
                return null;

            // Editor Sheet.
            ExchangeWidget newWidget = new ExchangeWidget(coord, name, offset, parent, this);
            mNodeList.Add(newWidget);
            nodeMoved(newWidget);
            //if (name.Equals("ROOT")) TODO this
            //    mEditorSheet.setFocus(mNodesList.get(mNumExchanges));

            // Misc.
            Tab currentTab = EditorTabs.getSingleton().getCurrentTab();
            if (currentTab != null)
                currentTab.setUnsavedChanges(true);
            ++mNumExchanges;
            return newWidget;
        }

        // Create a new, empty action node at the given coordinate with the given name.
        public ActionsWidget addActions(Point coord, String name, Boolean offset, Boolean suppressWarning, TabPage parent)
        {
            if (name.Equals(""))
                name = getAutoName(MovableNode.NodeType.Type_Actions);
            else if (!verifyName(name, true, true))
                return null;
            
            // Editor Sheet.
            ActionsWidget newWidget = new ActionsWidget(coord, name, offset, parent, this);
            mNodeList.Add(newWidget);
            nodeMoved(newWidget);
            nodeMoved(newWidget);
            //if (name.equals("ROOT")) TODO
            //    mEditorSheet.setFocus(mNodesList.get(mNumExchanges));

            // Misc.
            Tab currentTab = EditorTabs.getSingleton().getCurrentTab();
            if (currentTab != null)
                currentTab.setUnsavedChanges(true);
            ++mNumExchanges;
            return newWidget;
        }

        // Add a note widget at the given point.
        public NoteWidget addNote(Point location)
        {
            Tab currentTab = EditorTabs.getSingleton().getCurrentTab();
            if (currentTab != null)
                currentTab.setUnsavedChanges(true);
            mNotesList.Add(new NoteWidget(/*mParentTab.getActiveSheet().mCanvas,*/ location));
            return mNotesList.Last();
        }

        // Return the exchage which contains the given widget.
        public MovableNode getNodeByWidget(Control widget)
        {
            foreach (MovableNode node in mNodeList)
                if (node.hasWidget(widget))
                    return node;
            foreach (MovableNode note in mNotesList)
                if (note.hasWidget(widget))
                    return note;
            return null;
        }

        // Remove the given exchange from the list.
        public void removeWidget(MovableNode e)
        {
            mNodeList.Remove(e);
            mNumExchanges--;
            nodeRemoved(e);
        }

        // Remove the given note from the list.
        public void removeNote(NoteWidget n)
        {
            mNotesList.Remove(n);
            nodeRemoved(n);
        }

        // Called AFTER the given node is removed from our nodes list.
        private void nodeRemoved(MovableNode node)
        {
            Tab currentTab = EditorTabs.getSingleton().getCurrentTab();
            if (currentTab != null)
                currentTab.setUnsavedChanges(true);
            if (node == mLeft || node == mRight || node == mTop || node == mBottom)
            {
                mLeft = mRight = mTop = mBottom = null;
                foreach (MovableNode n in mNodeList)
                    nodeMoved(n);
            }
        }

        // Return the exchange at the given point.
        public MovableNode getNodeAt(Point p)
        {
            foreach (MovableNode e in mNodeList)
                if (e.covers(p))
                    return e;
            foreach (MovableNode e in mNotesList)
                if (e.covers(p))
                    return e;
            return null;
        }

        // Verify the given name for a new exchange. Return true if the name is valid.
        public Boolean verifyName(String _string, Boolean showError, Boolean testRoot)
        {
            if (testRoot && _string.Equals("ROOT") && mNodeList.Count() != 0)
            {
                if (showError)
                {
                    MessageBox.Show("Name \"ROOT\" is reserved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }
            if (_string.Equals("_END_DLG"))
            {
                if (showError)
                {
                    MessageBox.Show("Exchange name \"_END_DLG\" is reserved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }
            foreach (MovableNode w in mNodeList)
            {
                if (_string.Equals(w.getName()))
                {
                    if (showError)
                    {
                        MessageBox.Show("Tried to create exchange with already existing name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return false;
                }
            }
            return true;
        }

        // Called whenever a node in this tree is moved.
        public void nodeMoved(MovableNode node)
        {
            if (mLeft == null || node.getLocation().X < mLeft.getLocation().X)
                mLeft = node;
            if (mRight == null || node.getLocation().X > mRight.getLocation().X)
                mRight = node;
            if (mTop == null || node.getLocation().Y < mTop.getLocation().Y)
                mTop = node;
            if (mBottom == null || node.getLocation().Y > mBottom.getLocation().Y)
                mBottom = node;
        }

        // Return a rectangle describing the area currently containing nodes.
        public void getActiveRegion(Rectangle r)
        {
            r.X = mLeft.getLocation().X;
            r.Y = mTop.getLocation().Y;
            r.Width = mRight.getLocation().X - mLeft.getLocation().X;
            r.Height = mBottom.getLocation().Y - mTop.getLocation().Y;
        }

        // Prompts the user to rename the given node.
        public void renameNode(MovableNode node)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = "Rename node";
            label.Text = "Enter a new name for this node: " + node.getName();
            textBox.Text = "";

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
            if (dialogResult == DialogResult.OK && verifyName(textBox.Text, true, true))
            {
                if (textBox.Text == "")
                    node.setName(getAutoName(node.mNodeType));
                else
                    node.setName(textBox.Text);
            }
        }

        // Get an Node by name.
        public MovableNode getNode(String name)
        {
            foreach (MovableNode e in mNodeList)
                if (e.getName().Equals(name))
                    return e;
            return null;
        }

        // Sort the exchange list alphabetically by name.
        public void sortList()
        {
            mNodeList.TrimExcess();
            mNodeList.Sort();
            //for (int i = mNodeList.Count - 1; i >= 0; --i)
            //    for (int j = 0; j < i; ++j)
            //    {
            //        // If the item at [j] belongs below [j-1].
            //        if (mNodeList.ElementAt(j).getName().c(mNodeList.ElementAt(j + 1).getName()) > 0)
            //        {
            //            MovableNode temp = mNodeList.ElementAt(j);
            //            mNodeList.set(j, mNodeList.ElementAt(j + 1));
            //            mNodeList.set(j + 1, temp);
            //        }
            //    }
        }

        //// Save the tree.
        //public void saveTree(Element rootElement)
        //{
        //    // Save each widget.
        //    for (MovableNode e : mNodesList)
        //        e.save(rootElement);

        //    // Save notes.
        //    for (NoteWidget n : mNotesList)
        //    {
        //        if (n.getParent() == mEditorSheet.mCanvas)
        //            n.save(rootElement, 'e');
        //        else
        //            n.save(rootElement, 'z');
        //    }
        //}

        //// Export the current tree.
        //public Boolean exportTree(Element rootElement)
        //{
        //    // Sort the list into alphabetical order.
        //    sortList();

        //    for (MovableNode e : mNodesList)
        //        if (!e.export(rootElement))
        //            return false;
        //    return true;
        //}

        //// Load the tree from XML.
        //public Boolean loadTree(Element rootElement)
        //{
        //    // Clear this tree ready for loading.
        //    emptyTree();

        //    // Load exchanges & notes.
        //    if (!loadExchanges(rootElement) || !loadNotes(rootElement))
        //        return false;

        //    mParentTab.setUnsavedChanges(false);
        //    return true;
        //}

        //// Load all exchanges from given root element.
        //private Boolean loadExchanges(Element rootElement)
        //{
        //    // List of XML elements for exchanges.
        //    ArrayList<Element> exchangeList = new ArrayList<Element>();
        //    ArrayList<Element> actionList = new ArrayList<Element>();
        //    try
        //    {
        //        NodeList nodesList = rootElement.getElementsByTagName("Exchange");
        //        for (int i = 0; i < nodesList.getLength(); ++i)
        //            if (nodesList.item(i).getNodeType() == Node.ELEMENT_NODE)
        //                exchangeList.add((Element)nodesList.item(i));

        //        NodeList aNodesList = rootElement.getElementsByTagName("Action");
        //        for (int i = 0; i < aNodesList.getLength(); ++i)
        //            if (aNodesList.item(i).getNodeType() == Node.ELEMENT_NODE)
        //                actionList.add((Element)aNodesList.item(i));
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }

        //    // ~~ Exchanges ~~ //

        //    // First create empty exchange widgets
        //    for (Element element : exchangeList)
        //    {
        //        if (!element.hasAttribute("name") || !element.hasAttribute("posX") || !element.hasAttribute("posY"))
        //            return false;

        //        String name = element.getAttribute("name");
        //        Point pos = new Point(Integer.parseInt(element.getAttribute("posX")), Integer.parseInt(element.getAttribute("posY")));
        //        addExchange(pos, name, false, true);
        //    }

        //    // ~~ Actions ~~ //
        //    int actionsStart = mNodesList.size();

        //    // First create empty action widgets
        //    for (Element element : actionList)
        //    {
        //        if (!element.hasAttribute("name") || !element.hasAttribute("posX") || !element.hasAttribute("posY"))
        //            return false;

        //        String name = element.getAttribute("name");
        //        Point pos = new Point(Integer.parseInt(element.getAttribute("posX")), Integer.parseInt(element.getAttribute("posY")));
        //        addActions(pos, name, false, true);
        //    }

        //    // Populate exchanges with data from file.
        //    for (int i = 0; i < actionsStart; ++i)
        //        if (!mNodesList.get(i).load(exchangeList.get(i)))
        //            return false;

        //    // Populate actions with data from file
        //    for (int i = actionsStart; i < mNodesList.size(); ++i)
        //        if (!mNodesList.get(i).load(actionList.get(i - actionsStart)))
        //            return false;

        //    // Success.
        //    return true;
        //}

        //// Load notes from file.
        //private Boolean loadNotes(Element rootElement)
        //{
        //    // Editor sheet notes.
        //    try
        //    {
        //        NodeList nodesList = rootElement.getElementsByTagName("eNote");
        //        for (int i = 0; i < nodesList.getLength(); ++i)
        //            if (nodesList.item(i).getNodeType() == Node.ELEMENT_NODE)
        //                loadNote((Element)nodesList.item(i), mEditorSheet);

        //        nodesList = rootElement.getElementsByTagName("zNote");
        //        for (int i = 0; i < nodesList.getLength(); ++i)
        //            if (nodesList.item(i).getNodeType() == Node.ELEMENT_NODE)
        //                loadNote((Element)nodesList.item(i), mZoomedSheet);
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //// Load a note on the given sheet.
        //private Boolean loadNote(Element noteElement, BaseSheet sheet)
        //{
        //    if (!noteElement.hasAttribute("posX") || !noteElement.hasAttribute("posY") || !noteElement.hasAttribute("text"))
        //        return false;

        //    mNotesList.add(new NoteWidget(sheet.mCanvas, new Point(Integer.parseInt(noteElement.getAttribute("posX")), Integer.parseInt(noteElement.getAttribute("posY")))));
        //    mNotesList.get(mNotesList.size() - 1).setText(noteElement.getAttribute("text"));

        //    return true;
        //}

        //// Remove all exchanges and notes from the tree.
        //private void emptyTree()
        //{
        //    for (MovableNode n : mNodesList)
        //        n.close();
        //    mNodesList.clear();

        //    for (NoteWidget n : mNotesList)
        //        n.close();
        //    mNotesList.clear();

        //    mLeft = mRight = mTop = mBottom = null;
        //    mNumExchanges = 0;
        //}
    }
}
