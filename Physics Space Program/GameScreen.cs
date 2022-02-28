﻿using System;
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
        /** GLOBAL VALUES **/
        readonly int pixelToUnits = 1000000000; // What is 1 pixel in real life? (1 million kilometres)
        readonly float timeMultiplier = 15; // 600 million times faster


        readonly List<Object> objs = new List<Object> ();

        public GameScreen()
        {
            InitializeComponent();

            objs.Add(new Object(new PointF(0, 0), new PointF(0, 0), 10, Convert.ToInt64(1.989f * Math.Pow(10, 16)))); // Sun
            objs.Add(new Object(new PointF(69.806f, 0), new PointF(0, 0.16f * pixelToUnits), 2, Convert.ToInt64(3.285f * Math.Pow(10, 9)))); // Mercury
            objs.Add(new Object(new PointF(108.2f, 0), new PointF(0, 0.14325f * pixelToUnits), 5, Convert.ToInt64(4.867f * Math.Pow(10, 10)))); // Venus
            objs.Add(new Object(new PointF(152, 0), new PointF(0, 0.1195f * pixelToUnits), 5, Convert.ToInt64(5.9722f * Math.Pow(10, 10)))); // Earth
            objs.Add(new Object(new PointF(249.23f, 0), new PointF(0, 0.0895f * pixelToUnits), 3, Convert.ToInt64(6.39f * Math.Pow(10, 9)))); // Mars?

            foreach (Object obj in objs)
            {
                obj.SetupObject(pixelToUnits, timeMultiplier);
            }
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            //e.Graphics.ScaleTransform(1 /8f, 1 / 8f);
            e.Graphics.TranslateTransform(-objs[3].pos.X, -objs[3].pos.Y);
            int i = 0;
            foreach (Object obj in objs)
            {
                foreach (PointF point in obj.listOfPos)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(36, 36, 36), 2), point.X - 1, point.Y - 1, 2, 2);
                }
                e.Graphics.DrawEllipse(new Pen(Color.White, 2), obj.pos.X - obj.radius, obj.pos.Y - obj.radius, obj.radius * 2, obj.radius * 2);

                if(i != 3)
                {
                    e.Graphics.DrawLine(new Pen(Color.Blue), obj.pos.X, obj.pos.Y, objs[3].pos.X, objs[3].pos.Y);
                    e.Graphics.DrawString($"{obj.CalculateDistanceBetweenObjects(obj, objs[3])}", DefaultFont, new SolidBrush(Color.White), new PointF(obj.pos.X, obj.pos.Y - 20));
                }
                i++;
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
