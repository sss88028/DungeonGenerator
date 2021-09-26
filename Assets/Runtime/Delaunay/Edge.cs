using Display;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Structure
{
    public class Edge : IEquatable<Edge>
	{
		#region public-property
		public Vertex U 
		{ 
			get;
			set; 
		}

		public Vertex V 
		{ 
			get; 
			set; 
		}
		#endregion public-property

		#region public-method
		public Edge()
		{
		}

		public Edge(Vertex u, Vertex v)
		{
			U = u;
			V = v;
		}

		public static bool operator ==(Edge left, Edge right)
		{
			var isLeftNull = ReferenceEquals(left, null);
			var isRightNull = ReferenceEquals(right, null);
			if (isLeftNull || isRightNull) 
			{
				return isLeftNull != isRightNull;
			}
			return (left.U == right.U || left.U == right.V)
				&& (left.V == right.U || left.V == right.V);
		}

		public static bool operator !=(Edge left, Edge right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			if (obj is Edge e)
			{
				return this == e;
			}

			return false;
		}

		public bool Equals(Edge e)
		{
			return this == e;
		}

		public override int GetHashCode()
		{
			return U.GetHashCode() ^ V.GetHashCode();
		}

		public static bool AlmostEqual(Edge left, Edge right)
		{
			return Vertex.AlmostEqual(left.U, right.U) && Vertex.AlmostEqual(left.V, right.V)
				|| Vertex.AlmostEqual(left.U, right.V) && Vertex.AlmostEqual(left.V, right.U);
		}

		public static bool HasVertex(Edge e, Vertex v)
		{
			return e.U.Equals(v) || e.V.Equals(v);
		}
		#endregion public-method
	}
}