using CCTU.GameDevTools.MonoSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Display
{
	public class DebugDrawer : MonoSingleton<DebugDrawer>
	{
		#region private-field
		private HashSet<IDrawable> _drawables = new HashSet<IDrawable>();
		#endregion private-field

		#region public-method
		public void AddDrawable(IDrawable drawable)
		{
			_drawables.Add(drawable);
		}

		public void RemoveDrawable(IDrawable drawable)
		{
			_drawables.Remove(drawable);
		}
		#endregion public-method

		#region MonoBehaviour-method
		private void OnDrawGizmos()
		{
			foreach (var d in _drawables) 
			{
				d.Draw();
			}
		}
		#endregion MonoBehaviour-method
	}
}