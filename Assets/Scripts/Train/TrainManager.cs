using System.Collections.Generic;
using Basic;
using Level;
using UnityEngine;
using VContainer;

namespace Train
{
    public class TrainManager : MonoBehaviour, ITrainManager
    {
        [SerializeField] private bool _onlyFreeMines;
        private List<Base> _bases = new();
        private List<Mine> _mines = new();
        
        private IGraph _graph;
        
        [Inject]
        private void Construct(IGraph graph)
        {
            _graph = graph;
            foreach (var node in _graph.Nodes)
            {
                if (node is Base baseNode)
                    _bases.Add(baseNode);
                if (node is Mine mine)
                    _mines.Add(mine);
            }
        }
        
        public List<Node> GetPath(Node from, Node to)
        {
            if (_graph.GetPath(from, to, out var path))
                return path;
            return null;
        }

        public Line GetLine(Node node1, Node node2) => _graph.GetLine(node1, node2);

        public Node GetSpawnNode() => _bases[Random.Range(0, _bases.Count)];

        public List<Node> GetPathToMine(Node from, float speed, float mineDuration)
        {
            var free = new List<Mine>();
            foreach (var m in _mines)
                if (!_onlyFreeMines || m.IsFree)
                    free.Add(m);

            if (_graph.GetPathToMine(from, free, out var path, speed, mineDuration))
                return path;
            
            return null;
        }

        public List<Node> GetPathToBase(Node from, float speed)
        {
            if (_graph.GetPathToBase(from, _bases, out var path, speed))
                return path;
            return null;
        }
    }
}