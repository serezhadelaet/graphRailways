using System.Collections.Generic;
using Level;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Basic
{
    public class Graph : MonoBehaviour, IGraph
    {
        [SerializeField] private Node _nodePrefab;
        [SerializeField] private Line _linePrefab;
        [SerializeField] private List<Node> _nodes;
        [SerializeField] private List<Line> _lines;

        private Dictionary<(Node, Node), Line> _nodesPairToLines = new Dictionary<(Node, Node), Line>();

        public IReadOnlyList<Node> Nodes => _nodes;

        public void AddNeighbour(Node from, Node to)
        {
            var line = GetLine(from, to);
            if (line != null)
                return;
            if (from.Neighbours.Contains(to))
                return;
            if (to.Neighbours.Contains(from))
                return;
            from.Neighbours.Add(to);
            to.Neighbours.Add(from);
            GenerateLine(from, to);
        }

        public void OnPositionChanged(Node node)
        {
            foreach (var neighbour in node.Neighbours)
            {
                var line = GetLine(node, neighbour);
                if (line == null)
                    continue;
                SetLinePosition(line);
            }
        }

        public Line GetLine(Node node1, Node node2)
        {
            if (_nodesPairToLines.TryGetValue((node1, node2), out var line) ||
                _nodesPairToLines.TryGetValue((node2, node1), out line))
                return line;
            return null;
        }

        public bool GetPath(Node from, Node to, out List<Node> path)
        {
            CalculatePath(from, out _, out var previousNodes);
            path = CreatePath(previousNodes, from, to);
            return path != null;
        }
        
        public bool GetPathToMine(Node from, IReadOnlyList<Mine> targets, out List<Node> path, float speed,
            float mineDuration)
        {
            CalculatePath(from, out var distances, out var previousNodes);
            
            Node closestNode = null;
            var totalMineDuration = float.MaxValue;
            foreach (var target in targets)
            {
                if (!distances.TryGetValue(target, out var distance))
                    continue;

                var duration = (mineDuration * target.MineDurationMod) / (speed / distance);
                if (duration < totalMineDuration)
                {
                    totalMineDuration = duration;
                    closestNode = target;
                }
            }

            path = CreatePath(previousNodes, from, closestNode);
            return path != null;
        }
        
        [Button]
        public bool GetPathToBase(Node from, IReadOnlyList<Base> targets, out List<Node> path, float speed)
        {
            CalculatePath(from, out var distances, out var previousNodes);

            Node closestNode = null;
            var maxOutput = float.MinValue;
            foreach (var target in targets)
            {
                if (!distances.TryGetValue(target, out var distance))
                    continue;

                var totalOutput = target.InputResourcesMod / (distance / speed);
                if (totalOutput > maxOutput)
                {
                    maxOutput = totalOutput;
                    closestNode = target;
                }
            }

            path = CreatePath(previousNodes, from, closestNode);
            return path != null;
        }

        private void CalculatePath(Node from, out Dictionary<Node, float> distances,
            out Dictionary<Node, Node> previousNodes)
        {
            distances = new Dictionary<Node, float>();
            previousNodes = new Dictionary<Node, Node>();
            var unvisited = new List<Node>(_nodes);

            foreach (var node in _nodes)
                distances[node] = float.MaxValue;
            distances[from] = 0;

            while (unvisited.Count > 0)
            {
                Node current = GetNodeWithMinDistance(unvisited, distances);
                if (current == null || distances[current] == float.MaxValue)
                    break;

                unvisited.Remove(current);

                foreach (var neighbor in current.Neighbours)
                {
                    var length = GetLength(current, neighbor);
                    if (length == float.MaxValue)
                        continue;

                    var newDistance = distances[current] + length;
                    if (newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        previousNodes[neighbor] = current;
                    }
                }
            }
        }

        private void Awake()
        {
            InitializeLines();
        }

        private void InitializeLines()
        {
            foreach (var l in _lines)
                _nodesPairToLines[(l.From, l.To)] = l;
        }

        private void GenerateLine(Node from, Node to)
        {
            var line = PrefabUtility.InstantiatePrefab(_linePrefab) as Line;
            line.From = from;
            line.To = to;
            line.name = $"Line [{from.name}-{to.name}]";
            line.transform.SetParent(transform);
            SetLinePosition(line);
            _nodesPairToLines[(from, to)] = line;
            _lines.Add(line);
        }

        private void SetLinePosition(Line line)
        {
            var angle = Mathf.Atan2(line.To.transform.position.y - line.From.transform.position.y,
                line.To.transform.position.x - line.From.transform.position.x) * Mathf.Rad2Deg - 90;
            line.Visual.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            var pos = (line.From.transform.position + line.To.transform.position) / 2;
            line.Visual.position = pos;
            var length = (line.From.transform.position - line.To.transform.position).magnitude;
            line.Visual.localScale = new Vector3(line.Visual.localScale.x, length, line.Visual.localScale.z);
        }

        private Node GetNodeWithMinDistance(List<Node> nodes, Dictionary<Node, float> distances)
        {
            Node minNode = null;
            var minDistance = float.MaxValue;
            foreach (var node in nodes)
            {
                if (distances[node] < minDistance)
                {
                    minDistance = distances[node];
                    minNode = node;
                }
            }
            return minNode;
        }

        private float GetLength(Node from, Node to)
        {
            var line = GetLine(from, to);
            if (line != null)
                return line.Length;
            return float.MaxValue;
        }

        private List<Node> CreatePath(Dictionary<Node, Node> previousNodes, Node from, Node target)
        {
            if (target == null)
                return null;

            var path = new List<Node>();
            var current = target;

            if (!previousNodes.ContainsKey(current) && current != from)
                return null;

            while (current != null && previousNodes.TryGetValue(current, out var prev))
            {
                path.Add(current);
                current = prev;
            }

            if (current == from)
            {
                path.Add(from);
                path.Reverse();
                return path;
            }

            return null;
        }
    }
}