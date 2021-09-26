using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ViewMeshCreater
{
	#region public-method
	public Vector3 Origin;
	public Vector3 Direction;
	#endregion public-method

	#region private-field
	[SerializeField]
	private float _fov = 90;
	[SerializeField]
	private int _rayCount = 2;
	[SerializeField]
	private int _viewDistance = 50;
	[SerializeField]
	private LayerMask _layerMask;

	private Mesh _mesh;
	#endregion private-field

	#region public-property
	public Mesh Mesh
	{
		get
		{
			if (_mesh == null)
			{
				_mesh = new Mesh();
			}
			return _mesh;
		}
	}
	#endregion public-property

	#region public-method
	public void UpdateMesh() 
	{
		float angle = GetAngleFromVector3Float(Direction) + _fov / 2;
		var angleIncrease = _fov / _rayCount;

		var origin = Vector3.zero;

		var length = _rayCount + 1 + 1;
		var vertices = new Vector3[length];
		var uv = new Vector2[length];
		var triangles = new int[_rayCount * 3];

		vertices[0] = origin;
		var vertexIndex = 1;
		var triangleIndex = 0;
		for (var i = 0; i <= _rayCount; i++) 
		{
			var angleRed = angle * Mathf.Deg2Rad;
			var dir = new Vector3(Mathf.Cos(angleRed), 0, Mathf.Sin(angleRed));
			var end = dir * _viewDistance;
			var vertex = origin + end;

			var isHit = Physics.Raycast(Origin, dir, out var hit, _viewDistance, _layerMask.value);
			if (isHit) 
			{
				vertex = hit.point - Origin;
			}
			
			vertices[vertexIndex] = vertex;

			if (i > 0)
			{
				triangles[triangleIndex + 0] = 0;
				triangles[triangleIndex + 1] = vertexIndex - 1;
				triangles[triangleIndex + 2] = vertexIndex;

				triangleIndex += 3;
			}
			++vertexIndex;
			angle -= angleIncrease;
		}

		Mesh.vertices = vertices;
		Mesh.uv = uv;
		Mesh.triangles = triangles;
	}
	#endregion public-method

	#region private-method
	private float GetAngleFromVector3Float(Vector3 dir) 
	{
		dir = dir.normalized;
		var n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
		if (n < 0) 
		{
			n += 360;
		}
		return n;
	}
	#endregion private-method

}
