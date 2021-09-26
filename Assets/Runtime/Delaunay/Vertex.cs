using Display;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Structure
{
	public class Vertex : IEquatable<Vertex>
	{
		#region public-field
		public Vector3 Position;
		#endregion public-field

		#region public-method
		public Vertex() 
		{
		}

		public Vertex(Vector3 p)
		{
			Position = p;
		}
		public override bool Equals(object obj)
		{
			if (obj is Vertex v)
			{
				return Equals(v);
			}

			return false;
		}

		public bool Equals(Vertex other)
		{
			return Position == other.Position;
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode();
		}

		public override string ToString()
		{
			return Position.ToString();
		}

		public static bool AlmostEqual(Vertex left, Vertex right)
		{
			return AlmostEqual(left.Position.x, right.Position.x) && AlmostEqual(left.Position.y, right.Position.y) && AlmostEqual(left.Position.z, right.Position.z);
		}
		#endregion public-method

		#region private-method
		private static bool AlmostEqual(float x, float y)
		{
			return Mathf.Abs(x - y) <= float.Epsilon * Mathf.Abs(x + y) * 2
				|| Mathf.Abs(x - y) < float.MinValue;
		}
		#endregion private-method
	}

	public class Vertex<T> : Vertex
	{
		#region public-property
		public T Item 
		{ 
			get; 
			private set; 
		}
		#endregion public-property

		#region public-method
		public Vertex(T item)
		{
			Item = item;
		}

		public Vertex(Vector3 position, T item) : base(position)
		{
			Item = item;
		}
		#endregion public-method
	}
}
