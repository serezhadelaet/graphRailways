using System.Collections.Generic;
using Basic;
using Cysharp.Threading.Tasks;
using Level;
using UnityEngine;
using VContainer;

namespace Train.States
{
    public class MoveToMineState : BaseState
    {
        private ITrainManager _trainManager;

        [Inject]
        private void Construct(ITrainManager trainManager)
        {
            _trainManager = trainManager;
        }

        public override void Enter()
        {
            AsyncWorkTask().Forget();
        }

        private async UniTask AsyncWorkTask()
        {
            var train = Machine.Train;
            List<Node> path = null;
            while (path == null)
            {
                await UniTask.Yield(cancellationToken: transform.GetCancellationTokenOnDestroy());
                path = _trainManager.GetPathToMine(train.CurrentNode, train.Speed, train.MiningDuration);
                if (path != null && path[^1] is Mine mine)
                    mine.Reserve();
            }

            await Machine.Train.AsyncMovePath(path);
            Machine.SetState<MineState>();
        }
    }
}