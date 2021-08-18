using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour
{
	#region private-field
	[SerializeField]
	List<Vector3> _vertex;
	#endregion private-field

	#region MonoBehaviour-method
	[NaughtyAttributes.Button("Test")]
	public void Test()
	{
		var vertex = new List<Vertex>();
		foreach (var v in _vertex) 
		{
			vertex.Add(new Vertex(v));
		}
		Delaunay2D.Triangulate(vertex);
	}
	#endregion MonoBehaviour-method
}
