using Components.Goal;
using Components.Supports;
using Systems.Activity;
using Unity.Burst;
using Unity.Entities;

namespace Systems.Progression
{
	/// <summary>
	/// Defines the Update Goal UI System, which updates the 
    /// fulfillment and requirements of associated Goal UI.
	/// </summary>
	[UpdateAfter(typeof(DetermineFulfillment))]
	public class UpdateGoalUI : ComponentSystem
	{
        [BurstCompile]
        protected override void OnUpdate()
        {
            Entities.ForEach((ref GameObject gameObject, 
                ref Fulfillment fulfillment, 
                ref Requirement requirement) =>
            {
                fulfillment.Update(gameObject.Value);
                requirement.Update(gameObject.Value);
            });
        }
    }
}
