using System.Collections.Generic;
using Basic;
using Cysharp.Threading.Tasks;

namespace Train
{
    public interface ITrain
    {
        float Speed { get; }
        float MiningDuration { get; }
        Node CurrentNode { get; }
        UniTask AsyncMine(Node node);
        UniTask AsyncMovePath(List<Node> node);
    }
}