using Components.Activity;
using Components.Goal;
using Systems.Activity;
using Unity.Burst;
using Unity.Entities;

namespace Systems.Progression
{
	/// <summary>
	/// Defines the Determine Fulfillment System, which calculates the 
    /// how much each requirement has been fulfilled.
	/// </summary>
	[UpdateAfter(typeof(DefineGridDimensions))]
	public class DetermineFulfillment : ComponentSystem
	{
        [BurstCompile]
        protected override void OnUpdate()
        {
            bool hasUnfulfilledRequirement = false;

            int totalActiveTileCount = 0;
            Entities.ForEach((ref TilesetProperties properties) =>
            {
                totalActiveTileCount += properties.TileCount;
            });

            Entities.ForEach((ref Fulfillment fulfillment, 
                ref Requirement requirement) =>
            {
                fulfillment.Value = (int)DefineGridDimensions.Area - totalActiveTileCount;

                if (fulfillment.Value != requirement.Value)
                    hasUnfulfilledRequirement = true;
            });

            LevelManager.CanProgress = !hasUnfulfilledRequirement;
        }
    }
}
