﻿using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WorldHardestGame.Core
{
    public readonly struct Position : IXmlReaderSerializable, IEquatable<Position>
    {
        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public readonly float X;
        public readonly float Y;

        unsafe void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.AttributeCount >= 2 && GetFloatAttribute("x", out var w) && GetFloatAttribute("y", out var h))
            {
                fixed (float* x = &X)
                    *x = w;
                fixed (float* y = &Y)
                    *y = h;
            }

            bool GetFloatAttribute(string name, out float value)
                => reader.GetFloatAttribute(name, out value);
        }

        public float DistanceWith(Position position)
            => MathF.Sqrt(DistanceWithSquared(position));

        public float DistanceWithSquared(Position position)
            => (position.X - X) * (position.X - X) + (position.Y - Y) * (position.Y - Y);

        public override int GetHashCode()
            => HashCode.Combine(X, Y);

        public override bool Equals(object? obj)
            => obj is Position position && Equals(position);

        public bool Equals(Position other)
            => X == other.X && Y == other.Y;

        public static bool operator ==(in Position left, in Position right)
            => left.Equals(right);

        public static bool operator !=(in Position left, in Position right)
            => !left.Equals(right);

        public static Position operator +(in Position left, in Position right)
            => new Position(left.X + right.X, left.Y + right.Y);

        public static Position operator -(in Position left, in Position right)
            => new Position(left.X - right.X, left.Y - right.Y);
    }
}