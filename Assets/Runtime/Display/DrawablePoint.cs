using Display;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Display
{
	public class DrawablePoint : IPoint, IDrawable
	{
		#region public-property
		public Vector3 Position 
		{
			get;
			set;
		}
		#endregion public-property

		#region public-method
		public void Draw()
		{
			var oldColor = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(Position, 0.1F);
			Gizmos.color = oldColor;
		}
		#endregion public-method
	}
}
