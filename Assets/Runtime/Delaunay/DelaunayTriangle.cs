using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Structure
{
	public class DelaunayTriangle
	{
		#region private-field
		private List<Vertex> _vertices;
		private List<Triangle> _triangles;
		private List<Edge> _edges;
		#endregion private-field

		#region public-property
		public IReadOnlyList<Edge> Edges 
		{
			get 
			{
				return _edges;
			}
		}
		#endregion public-property

		#region public-method
		public static DelaunayTriangle Triangulate(IEnumerable<Vertex> vertices)
		{
			var delaunay = new DelaunayTriangle();
			delaunay._vertices.Clear();
			delaunay._vertices.AddRange(vertices);
			delaunay.Triangulate();

			return delaunay;
		}

        public void Rebuild(IEnumerable<Vertex> vertices)
        {
            _triangles.Clear();
            _edges.Clear();
            _vertices.Clear();
            _vertices.AddRange(vertices);
            Triangulate();
        }
		#endregion public-method

		#region private-method
		private DelaunayTriangle()
		{
			_edges = new List<Edge>();
			_triangles = new List<Triangle>();
			_vertices = new List<Vertex>();
        }

        private void Triangulate()
        {
            var minX = _vertices.Min(v => v.Position.x);
            var maxX = _vertices.Max(v => v.Position.x);
            var minY = _vertices.Min(v => v.Position.z);
            var maxY = _vertices.Max(v => v.Position.z);

            var dx = maxX - minX;
            var dy = maxY - minY;
            var deltaMax = Mathf.Max(dx, dy) * 2;

            var p1 = new Vertex(new Vector3(minX - 1, 0, minY - 1));
            var p2 = new Vertex(new Vector3(minX - 1, 0, maxY + deltaMax));
            var p3 = new Vertex(new Vector3(maxX + deltaMax, 0, minY - 1));

            _triangles.Add(new Triangle(p1, p2, p3));

            var polygon = new List<Edge>();
            var removeSet = new HashSet<Edge>();
            foreach (var vertex in _vertices)
            {
                polygon.Clear();
                removeSet.Clear();
                foreach (var t in _triangles)
                {
                    if (t.CircumCircleContains(vertex.Position))
                    {
                        t.IsBad = true;
                        polygon.Add(new Edge(t.A, t.B));
                        polygon.Add(new Edge(t.B, t.C));
                        polygon.Add(new Edge(t.C, t.A));
                    }
                }

                _triangles.RemoveAll(t => t.IsBad);

                for (var i = 0; i < polygon.Count; i++)
                {
                    for (var j = i + 1; j < polygon.Count; j++)
                    {
                        if (Edge.AlmostEqual(polygon[i], polygon[j]))
                        {
                            removeSet.Add(polygon[i]);
                            removeSet.Add(polygon[j]);
                        }
                    }
                }

                polygon.RemoveAll(e => removeSet.Contains(e));

                foreach (var edge in polygon)
                {
                    _triangles.Add(new Triangle(edge.U, edge.V, vertex));
                }
            }


            var edgeSet = new HashSet<Edge>();

            foreach (var t in _triangles)
            {
                var ab = new Edge(t.A, t.B);
                var bc = new Edge(t.B, t.C);
                var ca = new Edge(t.C, t.A);

                if (edgeSet.Add(ab) && !(Edge.HasVertex(ab, p1) || Edge.HasVertex(ab, p2) || Edge.HasVertex(ab, p3)))
                {
                    _edges.Add(ab);
                }

                if (edgeSet.Add(bc) && !(Edge.HasVertex(bc, p1) || Edge.HasVertex(bc, p2) || Edge.HasVertex(bc, p3)))
                {
                    _edges.Add(bc);
                }

                if (edgeSet.Add(ca) && !(Edge.HasVertex(ca, p1) || Edge.HasVertex(ca, p2) || Edge.HasVertex(ca, p3)))
                {
                    _edges.Add(ca);
                }
            }
        }
        #endregion private-method
    }
}