using System.Collections.Generic;
using Level;

namespace Basic
{
    public interface IGraph
    {
        public IReadOnlyCollection<Node> Nodes { get; }
        void OnPositionChanged(Node node);
        Line GetLine(Node node1, Node node2);
        bool GetPathToMine(Node from, IReadOnlyList<Mine> targets, out List<Node> path, float speed,
            float mineDuration);
        bool GetPathToBase(Node from, IReadOnlyList<Base> targets, out List<Node> path, float speed);
        bool GetPath(Node from, Node to, out List<Node> path);
    }
}