using Level;
using VContainer;

namespace Train.States
{
    public class UnloadToBaseState : BaseState
    {
        private ResourcesCounter _resourcesCounter;
        private const int MineAmount = 1;
        
        [Inject]
        private void Construct(ResourcesCounter resourcesCounter)
        {
            _resourcesCounter = resourcesCounter;
        }
        
        public override void Enter() 
        {
            if (Machine.Train.CurrentNode is Base baseNode)
            {
                Machine.SetState<MoveToMineState>();
                _resourcesCounter.Add(baseNode.InputResourcesMod * MineAmount);
            }
        }
    }
}