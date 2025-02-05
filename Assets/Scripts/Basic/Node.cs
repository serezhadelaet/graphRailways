using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Basic
{
    public class Node : MonoBehaviour
    {
        [field: SerializeField] public List<Node> Neighbours { get; private set; }
        
        private IGraph _graph;
        private Vector3 _prevPos;
        
        [Inject]
        private void Construct(IGraph graph)
        {
            _graph = graph;
        }

        private void Awake()
        {
            _prevPos = transform.position;
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (_prevPos == transform.position)
                return;
            _graph.OnPositionChanged(this);
            _prevPos = transform.position;
        }
#endif
    }
}