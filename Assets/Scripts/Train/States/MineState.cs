using Cysharp.Threading.Tasks;
using Level;

namespace Train.States
{
    public class MineState : BaseState
    {
        public override void Enter()
        {
            AsyncWorkTask().Forget();
        }

        private async UniTask AsyncWorkTask()
        {
            if (Machine.Train.CurrentNode is Mine mine)
            {
                await Machine.Train.AsyncMine(mine);
                mine.CompleteMining();
                Machine.SetState<MoveToBaseState>();
            }
        }
    }
}