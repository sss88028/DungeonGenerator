using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Structure
{
    public class Triangle : IEquatable<Triangle>
	{
		#region public-field
		public readonly Vertex A;

		public readonly Vertex B;

		public readonly Vertex C;

		internal bool IsBad;
		#endregion public-field

		#region private-field
		private Vector3? _circumCircleCenter;
		private float? _sqrCircumCircleRadius;
        #endregion private-field

        #region private-property
        private Vector3 CircumCircleCenter
        {
            get
            {
                if (_circumCircleCenter == null)
                {
                    var a = A.Position;
                    var b = B.Position;
                    var c = C.Position;

                    var ab = a.sqrMagnitude;
                    var cd = b.sqrMagnitude;
                    var ef = c.sqrMagnitude;

                    var circumX = (ab * (c.z - b.z) + cd * (a.z - c.z) + ef * (b.z - a.z)) / (a.x * (c.z - b.z) + b.x * (a.z - c.z) + c.x * (b.z - a.z));
                    var circumY = (ab * (c.x - b.x) + cd * (a.x - c.x) + ef * (b.x - a.x)) / (a.z * (c.x - b.x) + b.z * (a.x - c.x) + c.z * (b.x - a.x));

                    _circumCircleCenter = new Vector3(circumX / 2, 0, circumY / 2);
                }
                return _circumCircleCenter.Value;
            }
        }

        private float SqrCircumCircleRadius
        {
            get
            {
                if (_sqrCircumCircleRadius == null)
                {
                    var a = A.Position;

                    var circum = CircumCircleCenter;
                    _sqrCircumCircleRadius = Vector3.SqrMagnitude(a - circum);
                }
                return _sqrCircumCircleRadius.Value;
            }
        }
        #endregion private-property

        #region public-method
        public Triangle()
        {

        }

        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
        }

        public bool ContainsVertex(Vector3 v)
        {
            return Vector3.Distance(v, A.Position) < 0.01f
                || Vector3.Distance(v, B.Position) < 0.01f
                || Vector3.Distance(v, C.Position) < 0.01f;
        }

        public bool CircumCircleContains(Vector3 v)
        {
            float dist = Vector3.SqrMagnitude(v - CircumCircleCenter);
            return dist <= SqrCircumCircleRadius;
        }

        public static bool operator ==(Triangle left, Triangle right)
        {
            return (left.A == right.A || left.A == right.B || left.A == right.C)
                && (left.B == right.A || left.B == right.B || left.B == right.C)
                && (left.C == right.A || left.C == right.B || left.C == right.C);
        }

        public static bool operator !=(Triangle left, Triangle right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Triangle t)
            {
                return this == t;
            }

            return false;
        }

        public bool Equals(Triangle t)
        {
            return this == t;
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode();
        }
        #endregion public-method
    }
}