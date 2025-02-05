using System.Collections.Generic;
using Basic;

namespace Train
{
    public interface ITrainManager
    {
        Line GetLine(Node node1, Node node2);
        Node GetSpawnNode();
        List<Node> GetPathToMine(Node from, float speed, float minDuration);
        List<Node> GetPathToBase(Node from, float speed);
    }
}