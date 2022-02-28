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
        readonly float timeMultiplier = 5; // 600 million times faster
        Random randGen = new Random();

        readonly List<PointF> pastPoints = new List<PointF>();
        int frame = 0;
        readonly int selectedObject = 0;

        readonly List<Object> objs = new List<Object> ();

        public GameScreen()
        {
            InitializeComponent();

            objs.Add(new Object(new PointF(0, 0), new PointF(0, 0), 10, Convert.ToInt64(1.989f * Math.Pow(10, 16)), Color.Yellow, true)); // Sun
            //objs.Add(new Object(new PointF(600, 0), new PointF(0, -0.025f * pixelToUnits), 10, Convert.ToInt64(1.989f * Math.Pow(10, 16)), Color.Yellow), true); // Sun
            //objs.Add(new Object(new PointF(69.806f, 0), new PointF(0, 0.16f * pixelToUnits), 2, Convert.ToInt64(3.285f * Math.Pow(10, 9)), Color.Pink, true)); // Mercury
            //objs.Add(new Object(new PointF(108.2f, 0), new PointF(0, 0.14325f * pixelToUnits), 5, Convert.ToInt64(4.867f * Math.Pow(10, 10)), Color.Orange, true)); // Venus
            objs.Add(new Object(new PointF(152, 0), new PointF(0, 0.1195f * pixelToUnits), 5, Convert.ToInt64(5.9722f * Math.Pow(10, 10)), Color.Blue, true)); // Earth
            objs.Add(new Object(new PointF(249.23f, 0), new PointF(0, 0.0895f * pixelToUnits), 3, Convert.ToInt64(6.39f * Math.Pow(10, 9)), Color.Red, true)); // Mars?

            /*
            for(int i = 0;i < 30;i++)
            {
                int _direction = i * 12;
                int _distance = randGen.Next(255, 350);
                float rngAdd = 0.99f + Convert.ToSingle(randGen.NextDouble() / 50);
                objs.Add(new Object(
                    new PointF(Convert.ToSingle(Math.Cos(_direction / (180 / Math.PI))) * _distance, Convert.ToSingle(Math.Sin(_direction / (180 / Math.PI)) * _distance)), 
                    new PointF(Convert.ToSingle(0.04895f * (_distance / 255) * pixelToUnits * Math.Cos((_direction + 90) / (180 / Math.PI))) * rngAdd, Convert.ToSingle(0.04895f * (_distance / 255) * pixelToUnits * Math.Sin((_direction + 90) / (180 / Math.PI))) * rngAdd), 
                    1, 
                    Convert.ToInt64(6.39f * Math.Pow(10, 6)), 
                    Color.Gray,
                    false
                ));
            }*/


            foreach (Object obj in objs)
            {
                obj.SetupObject(pixelToUnits, timeMultiplier);
            }
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            foreach (PointF point in pastPoints)
            {
                //e.Graphics.DrawRectangle(new Pen(Color.FromArgb(36, 36, 36), 2), point.X - 1, point.Y - 1, 2, 2);
            }
            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
            //e.Graphics.ScaleTransform(1 /8f, 1 / 8f);
            e.Graphics.TranslateTransform(-objs[selectedObject].pos.X, -objs[selectedObject].pos.Y);
            int i = 0;
            foreach (Object obj in objs)
            {
                if (obj.doWeCare) { 
                    foreach (PointF point in obj.listOfPos)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(36, 36, 36), 2), point.X - 1, point.Y - 1, 2, 2);
                    }
                }
                e.Graphics.DrawEllipse(new Pen(obj.objColor, 3), obj.pos.X - obj.radius, obj.pos.Y - obj.radius, obj.radius * 2, obj.radius * 2);
                if(i != selectedObject)
                {
                    if (frame % 5 == 0)
                    {
                        pastPoints.Add(new PointF(obj.pos.X - objs[selectedObject].pos.X, obj.pos.Y - objs[selectedObject].pos.Y));
                        if (pastPoints.Count > 200)
                        {
                            pastPoints.RemoveAt(0);
                        }
                    }
                    //e.Graphics.FillEllipse(new SolidBrush(Color.Red), obj.minimumPoint.X - 3, obj.minimumPoint.Y - 3, 6, 6);
                    //e.Graphics.FillEllipse(new SolidBrush(Color.Blue), obj.maximumPoint.X - 3, obj.maximumPoint.Y - 3, 6, 6);
                    //e.Graphics.DrawLine(new Pen(Color.Blue), obj.pos.X, obj.pos.Y, objs[selectedObject].pos.X, objs[selectedObject].pos.Y);
                }
                if (obj.doWeCare)
                {
                    e.Graphics.DrawString($"{obj.CalculateDistanceBetweenObjects(obj, objs[selectedObject])}\n{(Math.Sqrt(Math.Pow(obj.velocity.X, 2) + Math.Pow(obj.velocity.Y, 2)) / pixelToUnits).ToString("0.000")}", DefaultFont, new SolidBrush(Color.White), new PointF(obj.pos.X, obj.pos.Y - 20 - obj.radius));
                }
                i++;
            }
            frame++;
        }

        private void FrameTick_Tick(object sender, EventArgs e)
        {
            for (int i = 0;i < objs.Count;i++)
            {
                for (int j = 0; j < timeMultiplier; j++)
                {
                    objs[i].AddForcesBetweenObjects(objs, i);
                    objs[i].MoveObject();
                    if(i != selectedObject)
                    {
                        //objs[i].UpdateMinMax(objs[selectedObject]);
                    }
                }
            }

            this.Refresh();
        }
    }
}
