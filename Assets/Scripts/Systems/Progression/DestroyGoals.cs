using Components.Goal;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Systems.Progression
{
	/// <summary>
	/// Defines the Destroy Goals System, which destroys all existing Goals.
	/// </summary>
    [AlwaysUpdateSystem]
    public class DestroyGoals : ComponentSystem
    {
        protected override void OnCreateManager()
        {
            Enabled = false;
        }

        [BurstCompile]
		protected override void OnUpdate()
		{
            Entities.ForEach((Entity entity, ref Components.Supports.GameObject gameObject, 
                ref Fulfillment fulfillment, ref Requirement requirement) =>
            {
                GameObject.Destroy(gameObject.Value);
                PostUpdateCommands.DestroyEntity(entity);
            });
        }
    }
}
