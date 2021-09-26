using CCTU.GameDevTools.MonoSingleton;
using Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Display
{
	public class PointObject : MonoBehaviour
	{
		private class PointObjectManager
		{
			#region private-field
			private static PointObjectManager _instance;
			private Dictionary<IPoint, Vertex> _vertexDict = new Dictionary<IPoint, Vertex>();
			private DelaunayTriangle _delaunay;
			#endregion private-field

			#region public-property
			public static PointObjectManager Instance 
			{
				get 
				{
					if (_instance == null) 
					{
						_instance = new PointObjectManager();
					}
					return _instance;
				}
			}
			#endregion public-property

			#region public-method
			public void AddPoint(IPoint point) 
			{
				var vertex = new Vertex(point.Position);
				_vertexDict[point] = vertex;

				GameSystem.Instance.OnUpdateEvent -= OnUpdate;
				GameSystem.Instance.OnUpdateEvent += OnUpdate;
			}

			public void RemovePoint(IPoint point)
			{
				if (_vertexDict.TryGetValue(point, out var vertex)) 
				{
					_vertexDict.Remove(point);
				}

				GameSystem.Instance.OnUpdateEvent -= OnUpdate;
				GameSystem.Instance.OnUpdateEvent += OnUpdate;
			}

			public void UpdatePoint(IPoint point)
			{
				if (_vertexDict.TryGetValue(point, out var vertex))
				{
					vertex.Position = point.Position;
				}

				GameSystem.Instance.OnUpdateEvent -= OnUpdate;
				GameSystem.Instance.OnUpdateEvent += OnUpdate;
			}
			#endregion public-method

			#region private-method
			private void OnUpdate(float deltaTime)
			{
				GameSystem.Instance.OnUpdateEvent -= OnUpdate;
				if (_delaunay == null)
				{
					_delaunay = DelaunayTriangle.Triangulate(_vertexDict.Values);
				}
				else 
				{
					_delaunay.Rebuild(_vertexDict.Values);
				}

			}
			#endregion private-method
		}

		#region private-field
		private DrawablePoint _drawablePoint;
		#endregion private-field

		#region private-method
		private void Awake()
		{
			_drawablePoint = new DrawablePoint();
			_drawablePoint.Position = transform.position;

			DebugDrawer.Instance.AddDrawable(_drawablePoint);
			PointObjectManager.Instance.AddPoint(_drawablePoint);

			GameSystem.Instance.OnUpdateEvent += OnUpdate;
		}

		private void OnDestroy()
		{
			GameSystem.Instance.OnUpdateEvent -= OnUpdate;

			PointObjectManager.Instance.RemovePoint(_drawablePoint);
			DebugDrawer.Instance.RemoveDrawable(_drawablePoint);
			_drawablePoint = null;
		}

		private void OnDrawGizmosSelected()
		{
			var oldColor = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 0.5F);
			Gizmos.color = oldColor;
		}

		private void OnUpdate(float deltaTime)
		{
			if (transform.hasChanged)
			{
				_drawablePoint.Position = transform.position;
				PointObjectManager.Instance.UpdatePoint(_drawablePoint);
				transform.hasChanged = false;
			}
		}
		#endregion private-method
	}
}