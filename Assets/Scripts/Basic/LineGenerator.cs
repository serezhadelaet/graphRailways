using UnityEditor;
using UnityEngine;

namespace Basic
{
    public class LineSpawner : MonoBehaviour
    {
        [SerializeField] private Line _linePrefab;

#if UNITY_EDITOR
        public Line SpawnLine(Node from, Node to)
        {
            var line = PrefabUtility.InstantiatePrefab(_linePrefab) as Line;
            line.From = from;
            line.To = to;
            line.name = $"Line [{from.name}-{to.name}]";
            line.transform.SetParent(transform);
            SetLinePosition(line);
            return line;
        }

        public void SetLinePosition(Line line)
        {
            var angle = Mathf.Atan2(line.To.transform.position.y - line.From.transform.position.y,
                line.To.transform.position.x - line.From.transform.position.x) * Mathf.Rad2Deg - 90;
            line.Visual.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            var pos = (line.From.transform.position + line.To.transform.position) / 2;
            line.Visual.position = pos;
            var length = (line.From.transform.position - line.To.transform.position).magnitude;
            line.Visual.localScale = new Vector3(line.Visual.localScale.x, length, line.Visual.localScale.z);
        }
#endif
    }
}