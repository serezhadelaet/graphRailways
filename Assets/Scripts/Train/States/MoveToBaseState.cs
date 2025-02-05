using System.Collections.Generic;
using Basic;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Train.States
{
    public class MoveToBaseState : BaseState
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
                path = _trainManager.GetPathToBase(train.CurrentNode, train.Speed);
                await UniTask.Yield(cancellationToken: transform.GetCancellationTokenOnDestroy());
            }

            await Machine.Train.AsyncMovePath(path);
            Machine.SetState<UnloadToBaseState>();
        }
    }
}