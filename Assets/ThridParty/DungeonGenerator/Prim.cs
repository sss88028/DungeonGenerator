﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Graphs;

public static class Prim 
{
    public class Edge : Graphs.Edge 
    {
        public float Distance { get; private set; }

        public Edge(Vertex u, Vertex v) : base(u, v) 
        {
            Distance = Vector3.Distance(u.Position, v.Position);
        }

        public static bool operator ==(Edge left, Edge right) 
        {
            var isLeftNull = ReferenceEquals(left, null);
            var isRightNull = ReferenceEquals(right, null);
            if (isLeftNull || isRightNull)
            {
                if (!isLeftNull || !isRightNull) 
                {
                    return false;
                }
                return true;
            }
            return (left.U == right.U && left.V == right.V)
                || (left.U == right.V && left.V == right.U);
        }

        public static bool operator !=(Edge left, Edge right) {
            return !(left == right);
        }

        public override bool Equals(object obj) {
            if (obj is Edge e) {
                return this == e;
            }

            return false;
        }

        public bool Equals(Edge e) {
            return this == e;
        }

        public override int GetHashCode() {
            return U.GetHashCode() ^ V.GetHashCode();
        }
    }

    public static List<Edge> MinimumSpanningTree(List<Edge> edges, Vertex start) 
    {
        var openSet = new HashSet<Vertex>();
        var closedSet = new HashSet<Vertex>();

        foreach (var edge in edges) 
        {
            openSet.Add(edge.U);
            openSet.Add(edge.V);
        }

        closedSet.Add(start);

        var results = new List<Edge>();

        while (openSet.Count > 0) 
        {
            Edge chosenEdge = null;
            var minWeight = float.PositiveInfinity;

            foreach (var edge in edges) 
            {
                var closedVertices = 0;

                if (!closedSet.Contains(edge.U))
                {
                    closedVertices++;
                } 
                if (!closedSet.Contains(edge.V))
                {
                    closedVertices++;
                }

                if (closedVertices != 1)
                {
                    continue;
                }

                if (edge.Distance < minWeight) 
                {
                    chosenEdge = edge;
                    minWeight = edge.Distance;
                }
            }

            if (chosenEdge == null)
            {
                break;
            }

            results.Add(chosenEdge);
            openSet.Remove(chosenEdge.U);
            openSet.Remove(chosenEdge.V);
            closedSet.Add(chosenEdge.U);
            closedSet.Add(chosenEdge.V);
        }

        return results;
    }
}
