using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test
{
	public class PathFindingBehaviour : MonoBehaviour
	{
		#region private-method
		[SerializeField]
		private Vector2Int _size;
		[SerializeField]
		private Vector2Int _start;
		[SerializeField]
		private Vector2Int _end;

		[SerializeField]
		private Vector2Int[] _walls;
		#endregion private-method

		#region public-method
		[NaughtyAttributes.Button("Test")]
		public void Test() 
		{
			var pf = new DungeonPathfinder2D(_size);

			pf.FindPath(_start, _end, (a, b) =>
			{
				var pathCost = new DungeonPathfinder2D.PathCost();
				pathCost.traversable = true;

				if (_walls.Contains(b.Position)) 
				{
					pathCost.cost += 10;
				}
				else
				{
					pathCost.cost += 1;
				}
				return pathCost;
			});
		}
		#endregion public-method
	}
}
