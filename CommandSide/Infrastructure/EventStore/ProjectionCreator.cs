using System.Threading.Tasks;
using Abstractions;
using EventStore.ClientAPI.Projections;

namespace Infrastructure.EventStore
{
    internal sealed class ProjectionCreator
    {
        private readonly ProjectionsManager _projectionsManager;

        public ProjectionCreator(ProjectionsManager projectionsManager)
        {
            _projectionsManager = projectionsManager;
        }
        
        public async Task CreateProjectionFrom(SubscriptionRequest subscriptionRequest)
        {
            var projectionState = await _projectionsManager.GetStateAsync(subscriptionRequest.ProjectionName);
        }
    }
}