using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public class NewTextBox : Control
    {
        private StringFormat SF = new StringFormat();
        public NewTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Size = new Size(150, 70);
            BackColor = Color.Black;
            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graph.Clear(Parent.BackColor);
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Rectangle fillRect = new Rectangle(2, 2, Width - 5, Height - 5);
            graph.DrawRectangle(new Pen(Color.Black, 4F), rect);
            graph.FillRectangle(new SolidBrush(BackColor), fillRect);
            graph.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
        }
    }
}
