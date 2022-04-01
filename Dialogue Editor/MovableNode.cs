using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;


namespace Dialogue_Editor
{
    abstract class MovableNode : IDisposable
    {
        // The name of this node.
        protected String mName;

        public enum NodeType
        {
            Type_Exchange,
            Type_Actions,
            Type_Note,
            Type_Sheet
        };

        // The type of node this represents.
        public NodeType mNodeType;
	
	// CONSTRUCTOR.
	public MovableNode(NodeType type, String name)
        {
            mNodeType = type;
            mName = name;
        }

        public abstract void Dispose();

        // Change the name of this node.
        public void setName(String newName)
        {
            mName = newName;
        }

        // Set the location of this node relative to it's parent.
        public abstract void setLocation(Point p);

        // Set the location of this node relative to it's parent.
        public abstract void setLocationDrag(Point p);

        // Return this node's location relative to it's parent.
        public abstract Point getLocation();

        // Return the coordinate of the cursor within this node.
        public abstract Point getMousePosOn();

        // Add a listener to this node.
        //public abstract void addListener(int eventType, MouseListener listener);

        // Remove listener from this node.
        //public abstract void removeListener(int eventType, MouseListener listener);

        // Return whether this node contains the given widget - used to calculate drag events.
        public abstract Boolean hasWidget(Control widget);

        // Test whether this node covers the given point on the canvas.
        public abstract Boolean covers(Point p);

        // Get this node's parent.
        //public abstract Composite getParent();

        // Return the base widget of this node.
        //public abstract Composite getBaseWidget();

        // Return the name of this node.
        public String getName() { return mName; }

        // Close this node, choose whether to update any goto lines.
        public void close(Boolean updateLines) { }

        // Load this node's data from XML.
        public Boolean load(XmlElement element) { return true; }

        // Save this Node's data to XML file.
        public void save(XmlElement rootElement) { }

        // Export this Node to XML file.
        public Boolean export(XmlElement rootElement) { return true; }

        // Return the Point coordinate goto lines should lead to.
        public Point getGoToPoint() { return new Point(0, 0); }

        // Clear the GoTo with the given Line ID.
        public void clearGoTo(int lineID, Boolean notifySender) { }

        // Add a GoTo with the given data.
        //public void addGoTo(int lineID, GoToInterface sender) { }

        // Called to notify the tab of unsaved changes made to this element.
        public void changesMade()
        {
            //EditorTabs.getSingleton().getCurrentTab().setUnsavedChanges(true);
        }

        protected void _close()
        {

        }
    }
}
