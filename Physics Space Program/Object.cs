﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Physics_Space_Program
{
    internal class Object
    {
        public PointF pos, velocity;
        public float radius;
        readonly float mass;
        readonly double gravConstant;

        public Color objColor;


        public readonly List<PointF> listOfPos = new List<PointF>();
        int frame = 0;
        int myPixelsToUnits;
        float myTimeMultiplier;
        public bool doWeCare;

        public PointF minimumPoint = new PointF(0, 0);
        public float minimumDist = float.PositiveInfinity;
        public PointF maximumPoint = new PointF(0, 0);
        public float maximumDist = 0;

        public Object(PointF _pos, PointF _velocity, float _radius, float _mass, Color _objColor, bool _doWeCare)
        {
            pos = _pos;
            velocity = _velocity;
            radius = _radius;
            mass = _mass;
            objColor = _objColor;
            doWeCare = _doWeCare;

            gravConstant = 6.67 * Math.Pow(10, -11);
        }

        public void SetupObject(int _pxToUnits, float _myTimeMultiplier)
        {
            myPixelsToUnits = _pxToUnits;
            myTimeMultiplier = _myTimeMultiplier;
        }

        void AddForce(PointF _force)
        {
            velocity.X += _force.X * (1 / 60f) / mass;
            velocity.Y += _force.Y * (1 / 60f) / mass;
        }

        public float CalculateDistanceBetweenObjects(Object _obj1, Object _obj2)
        {
            return Convert.ToSingle(Math.Sqrt(Math.Pow(_obj2.pos.X - _obj1.pos.X, 2) + Math.Pow(_obj2.pos.Y - _obj1.pos.Y, 2)));
        }


        float CalculateDirectionBetweenObjects(Object _obj1, Object _obj2)
        {
            return Convert.ToSingle(Math.Atan2(_obj2.pos.Y - _obj1.pos.Y, _obj2.pos.X - _obj1.pos.X));
        }


        public void AddForcesBetweenObjects(List<Object> _objs, int currentID)
        {
            PointF totalForce = new PointF(0, 0);
            for(int j = 0;j < _objs.Count;j++)
            {
                if(currentID != j)
                {
                    totalForce.X += CalculateForcesBetweenObject(this, _objs[j]).X;
                    totalForce.Y += CalculateForcesBetweenObject(this, _objs[j]).Y;
                }
            }

            AddForce(totalForce);
        }

        public void MoveObject()
        {
            pos.X += (velocity.X / myPixelsToUnits);
            pos.Y += (velocity.Y / myPixelsToUnits);

            if(frame % (5 * myTimeMultiplier) == 0)
            {
                listOfPos.Add(pos);
                if(listOfPos.Count > 15)
                {
                    listOfPos.RemoveAt(0);
                }
            }
            frame++;
        }

        PointF CalculateForcesBetweenObject(Object _obj1, Object _obj2)
        {
            float theForce = Convert.ToSingle((gravConstant * _obj1.mass * _obj2.mass) / Math.Pow(CalculateDistanceBetweenObjects(_obj1, _obj2), 2) / myPixelsToUnits * Math.Pow(10, 14));
            float theDir = CalculateDirectionBetweenObjects(_obj1, _obj2);
            return new PointF(Convert.ToSingle(theForce * Math.Cos(theDir)), Convert.ToSingle(theForce * Math.Sin(theDir)));
        }

        public void UpdateMinMax(Object _theObj1)
        {
            if(CalculateDistanceBetweenObjects(this, _theObj1) > maximumDist)
            {
                maximumDist = CalculateDistanceBetweenObjects(this, _theObj1);
                maximumPoint = new PointF(pos.X, pos.Y);
            }
            if (CalculateDistanceBetweenObjects(this, _theObj1) < minimumDist)
            {
                minimumDist = CalculateDistanceBetweenObjects(this, _theObj1);
                minimumPoint = new PointF(pos.X, pos.Y);
            }
        }
    }
}
