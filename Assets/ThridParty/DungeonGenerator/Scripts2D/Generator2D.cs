using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using System.Threading.Tasks;
using System.Threading;

public class Generator2D : MonoBehaviour {
    enum CellType {
        None,
        Room,
        Hallway
    }

    class Room 
    {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size) 
        {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b) 
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }
    }

    [SerializeField]
    Vector2Int size;
    [SerializeField]
    int roomCount;
    [SerializeField]
    RectInt roomMaxSize;
    [SerializeField]
    GameObject cubePrefab;
    [SerializeField]
    Material redMaterial;
    [SerializeField]
    Material blueMaterial;

    Random _random;
    Grid2D<CellType> _grid;
    List<Room> _rooms;
    Delaunay2D _delaunay;
    HashSet<Prim.Edge> _selectedEdges;

    private void Start() 
    {
        Generate();
    }

    private void Generate() 
    {
        var seed = (int)DateTime.Now.Ticks;
        _random = new Random(0);
        //_random = new Random(seed);
        _grid = new Grid2D<CellType>(size, Vector2Int.zero);
        _rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
    }

    private void PlaceRooms() 
    {
        var maxRoomCount = roomCount;
        var maxRetry = maxRoomCount * 10;
        while ((maxRoomCount > 0 || _rooms.Count < 2) && maxRetry > 0)
        {
            Vector2Int location = new Vector2Int(
                _random.Next(0, size.x),
                _random.Next(0, size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                _random.Next(roomMaxSize.x, roomMaxSize.width + 1),
                _random.Next(roomMaxSize.y, roomMaxSize.height + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in _rooms)
            {
                if (Room.Intersect(room, buffer))
                {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x ||
                newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y)
            {
                add = false;
            }

            if (add)
            {
                _rooms.Add(newRoom);
                PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);

                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    _grid[pos] = CellType.Room;
                }
                maxRoomCount--;
            }
            maxRetry--;
        }
    }

    private void Triangulate() 
    {
        var vertices = new List<Vertex>();

        foreach (var room in _rooms)
        {
            vertices.Add(new Vertex<Room>(room.bounds.center, room));
        }

        _delaunay = Delaunay2D.Triangulate(vertices);
    }

    private void CreateHallways() 
    {
        var edges = new List<Prim.Edge>();

        foreach (var edge in _delaunay.Edges) 
        {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        var mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        _selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(_selectedEdges);

        foreach (var edge in remainingEdges) 
        {
            if (_random.NextDouble() < 0.125) 
            {
                _selectedEdges.Add(edge);
            }
        }
    }

    private void PathfindHallways() 
    {
        var aStar = new DungeonPathfinder2D(size);

        foreach (var edge in _selectedEdges) 
        {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => 
            {
                var pathCost = new DungeonPathfinder2D.PathCost();
                
                pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

                if (_grid[b.Position] == CellType.Room) 
                {
                    pathCost.cost += 10;
                } 
                else if (_grid[b.Position] == CellType.None) 
                {
                    pathCost.cost += 5;
                } 
                else if (_grid[b.Position] == CellType.Hallway) 
                {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null) 
            {
                for (int i = 0; i < path.Count; i++) 
                {
                    var current = path[i];

                    if (_grid[current] == CellType.None) 
                    {
                        _grid[current] = CellType.Hallway;
                    }

                    if (i > 0) 
                    {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }

                foreach (var pos in path) 
                {
                    if (_grid[pos] == CellType.Hallway) 
                    {
                        PlaceHallway(pos);
                    }
                }
            }
        }
    }

    private void PlaceCube(Vector2Int location, Vector2Int size, Material material) 
    {
        GameObject go = Instantiate(cubePrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);
        go.GetComponent<Transform>().localScale = new Vector3(size.x, 1, size.y);
        go.GetComponent<MeshRenderer>().material = material;
    }

    private void PlaceRoom(Vector2Int location, Vector2Int size) 
    {
        PlaceCube(location, size, redMaterial);
    }

    private void PlaceHallway(Vector2Int location) 
    {
        PlaceCube(location, new Vector2Int(1, 1), blueMaterial);
    }
}
