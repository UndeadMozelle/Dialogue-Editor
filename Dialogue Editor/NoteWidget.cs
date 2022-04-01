using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Dialogue_Editor
{
    class NoteWidget : MovableNode
    {
        public NoteWidget(Point location) : base(NodeType.Type_Note, "")
        { }

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

        public override void Dispose()
        {

        }

        public override bool hasWidget(Control widget)
        {
            return false;
        }
    }
}
