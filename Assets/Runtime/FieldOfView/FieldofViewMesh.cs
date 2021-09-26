using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldofViewMesh : MonoBehaviour
{
	#region private-method
	[SerializeField]
	private MeshFilter _meshFilter;
	[SerializeField]
	private MouseDirectionHandler _mouseDirectionHandler;
	[SerializeField]
	private ViewMeshCreater _viewMeshCreater = new ViewMeshCreater();
	#endregion private-method

	#region MonoBehaviour-method
	private void Start()
	{
		SetUpMesh();
	}

	private void Update()
	{
		UpdateMesh();
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void SetUpMesh()
	{
		if (_meshFilter == null)
		{
			return;
		}
		_meshFilter.mesh = _viewMeshCreater.Mesh;
	}

	private void UpdateMesh()
	{
		_viewMeshCreater.Origin = transform.position;
		if (_mouseDirectionHandler != null)
		{
			_viewMeshCreater.Direction = _mouseDirectionHandler.GetDirection();
		}
		_viewMeshCreater.UpdateMesh();
	}
	#endregion private-method
}
