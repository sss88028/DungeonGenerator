using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Structure
{
	public class Vertex : IEquatable<Vertex>
	{
		#region public-property
		public Vector3 Position 
		{
			get;
			private set;
		}

		#endregion public-property

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
		#endregion public-method
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
