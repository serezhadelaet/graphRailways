using UI;
using VContainer;

namespace Level
{
    public class ResourcesCounter
    {
        private ResourcesTotalView _resourcesTotalView;
        
        [Inject]
        private void Construct(ResourcesTotalView resourcesTotalView)
        {
            _resourcesTotalView = resourcesTotalView;
        }
        
        private int _amount;
        
        public void Add(int amount)
        {
            _amount += amount;
            _resourcesTotalView.Set(_amount);
        }
    }
}