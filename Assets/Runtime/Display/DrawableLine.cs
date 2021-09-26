using Display;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Display
{
	public class DrawableLine : IDrawable, ILine
	{
		#region public-property
		public IPoint U 
		{
			get;
			set;
		}

		public IPoint V
		{
			get;
			set;
		}
		#endregion public-property

		#region public-method
		public void Draw()
		{
			var oldColor = Gizmos.color;
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(U.Position, V.Position);
			Gizmos.color = oldColor;
		}
		#endregion public-method
	}
}