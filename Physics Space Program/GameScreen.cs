using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physics_Space_Program
{
    public partial class GameScreen : UserControl
    {

        readonly List<Object> objs = new List<Object> ();

        public GameScreen()
        {
            InitializeComponent();

            objs.Add(new Object(new PointF(-4150, 0), new PointF(0, 0.000000000001f), 10, 15f / 81f));
            objs.Add(new Object(new PointF(-4000, 0), new PointF(0, 0.00000000005f), 20, 15f));
            objs.Add(new Object(new PointF(0, 0), new PointF(0, 0), 80, 100f));
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            e.Graphics.ScaleTransform(1 /8f, 1 / 8f);
            e.Graphics.TranslateTransform(-objs[1].pos.X, -objs[1].pos.Y);
            foreach (Object obj in objs)
            {
                e.Graphics.DrawEllipse(new Pen(Color.White, 2), obj.pos.X - obj.radius, obj.pos.Y - obj.radius, obj.radius * 2, obj.radius * 2);
                foreach(PointF point in obj.listOfPos)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Gray, 2), point.X - 1, point.Y - 1, 2, 2);
                }
            }
        }

        private void FrameTick_Tick(object sender, EventArgs e)
        {
            for (int i = 0;i < objs.Count;i++)
            {
                objs[i].AddForcesBetweenObjects(objs, i);
                objs[i].MoveObject();
            }

            this.Refresh();
        }
    }
}
