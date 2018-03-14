﻿using SimpleCAD.Geometry;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace SimpleCAD.Drawables
{
    public class Circle : Drawable
    {
        private Point2D center;
        private float radius;

        public Point2D Center { get => center; set { center = value; NotifyPropertyChanged(); } }
        public float Radius { get => radius; set { radius = value; NotifyPropertyChanged(); } }

        [Browsable(false)]
        public float X { get { return Center.X; } }
        [Browsable(false)]
        public float Y { get { return Center.Y; } }

        public Circle(Point2D center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public Circle(float x, float y, float radius)
            : this(new Point2D(x, y), radius)
        {
            ;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawCircle(Style, Center, Radius);
        }

        public override Extents2D GetExtents()
        {
            Extents2D extents = new Extents2D();
            extents.Add(X - Radius, Y - Radius);
            extents.Add(X + Radius, Y + Radius);
            return extents;
        }

        public override void TransformBy(Matrix2D transformation)
        {
            Center = Center.Transform(transformation);
            Radius = (Vector2D.XAxis * Radius).Transform(transformation).Length;
        }

        public override bool Contains(Point2D pt, float pickBoxSize)
        {
            float dist = (pt - Center).Length;
            return dist <= Radius + pickBoxSize / 2 && dist >= Radius - pickBoxSize / 2;
        }

        public override ControlPoint[] GetControlPoints()
        {
            return new[]
            {
                new ControlPoint("Center"),
                new ControlPoint("Radius", ControlPoint.ControlPointType.Distance, Center, Center + Radius * Vector2D.XAxis),
            };
        }

        public Circle(BinaryReader reader) : base(reader)
        {
            Center = new Point2D(reader);
            Radius = reader.ReadSingle();
        }

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            Center.Save(writer);
            writer.Write(Radius);
        }
    }
}
