using System.Collections.Generic;
using Basic;
using Cysharp.Threading.Tasks;
using Level;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Train
{
    public class Train : MonoBehaviour, ITrain
    {
        [field: SerializeField] public float Speed { get; private set; } = 1;
        [field: SerializeField] public float MiningDuration { get; private set; } = 2;

        private ITrainManager _trainManager;
        private List<Node> _gizmosPath = new();

        public Node CurrentNode { get; private set; }

        [Inject]
        private void Construct(ITrainManager trainManager)
        {
            _trainManager = trainManager;
        }

        private void Awake()
        {
            RandomSpawn();
        }

        private void RandomSpawn()
        {
            CurrentNode = _trainManager.GetSpawnNode();
            transform.position = CurrentNode.transform.position;
        }

        public async UniTask AsyncMine(Node node)
        {
            if (node is not Mine mine)
                return;
            var duration = mine.MineDurationMod * MiningDuration;
            await UniTask.WaitForSeconds(duration, cancellationToken: transform.GetCancellationTokenOnDestroy());
        }

        public async UniTask AsyncMovePath(List<Node> path)
        {
            if (path == null)
                return;
            for (int i = 1; i < path.Count; i++)
                _gizmosPath.Add(path[i]);
            for (int i = 1; i < path.Count; i++)
            {
                var target = path[i];
                var startPos = transform.position;
                var targetPos = target.transform.position;
                var line = _trainManager.GetLine(CurrentNode, target);
                var length = line == null ? 1 : line.Length;
                var t = 0f;
                
                var rot = Quaternion.LookRotation(targetPos - startPos);
                rot.x = transform.rotation.x;
                rot.y = transform.rotation.y;
                transform.rotation = rot;

                while (t < 1)
                {
                    t += Time.deltaTime * (Speed / length);
                    transform.position = Vector3.Lerp(startPos, targetPos, t);
                    await UniTask.Yield(cancellationToken: transform.GetCancellationTokenOnDestroy());
                }

                CurrentNode = target;
                _gizmosPath.Remove(target);
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_gizmosPath.Count == 0)
                return;
            for (int i = 0; i < _gizmosPath.Count - 1; i++)
                Handles.DrawDottedLine(_gizmosPath[i].transform.position, _gizmosPath[i + 1].transform.position, 10);
            Handles.DrawDottedLine(transform.position, _gizmosPath[0].transform.position, 10);
        }
#endif
    }
}