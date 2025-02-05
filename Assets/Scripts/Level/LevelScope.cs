using Basic;
using Train;
using UI;
using VContainer;
using VContainer.Unity;

namespace Level
{
    public class LevelScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<IGraph>();
            builder.RegisterComponentInHierarchy<ITrainManager>();
            builder.RegisterComponentInHierarchy<ResourcesTotalView>();
            builder.Register<ResourcesCounter>(Lifetime.Singleton);
        }
    }
}