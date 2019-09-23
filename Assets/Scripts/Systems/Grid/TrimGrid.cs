using Components.Activity;
using Components.Attributes;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the Trim Grid System, with initiates the destruction 
    /// of Grid Tiles that are not within the defined Grid dimensions.
	/// </summary>
	[UpdateAfter(typeof(PopulateGrid))]
	public class TrimGrid : JobComponentSystem
	{
        [RequireComponentTag(typeof(GridElement))]
        private struct TrimJob : IJobProcessComponentDataWithEntity<Position, TileProperties>
        {
            [ReadOnly]
            public float PositiveX;
            [ReadOnly]
            public float PositiveY;
            [ReadOnly]
            public float PositiveZ;
            [ReadOnly] 
            public float NegativeX;
            [ReadOnly]
            public float NegativeY;
            [ReadOnly]
            public float NegativeZ;

            [WriteOnly]
            public EntityCommandBuffer.Concurrent Buffer;

            public void Execute(Entity entity, int index, 
                [ReadOnly] ref Position position,
                ref TileProperties properties)
            {
                if (PositiveX <= position.Value.x ||
                    PositiveY <= position.Value.y ||
                    PositiveZ <= position.Value.z ||
                    NegativeX > position.Value.x ||
                    NegativeY > position.Value.y ||
                    NegativeZ > position.Value.z)
                {
                    Buffer.RemoveComponent<GridElement>(index, entity);
                }
            }
        }


        private EntityCommandBufferSystem barrier;

        protected override void OnCreateManager()
        {
            barrier = World.GetOrCreateManager<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            EntityCommandBuffer.Concurrent buffer = barrier.CreateCommandBuffer().ToConcurrent();
            JobHandle job = new TrimJob()
            {
                Buffer = buffer, 
                PositiveX = DefineGridDimensions.PositiveX,
                PositiveY = DefineGridDimensions.PositiveY,
                PositiveZ = DefineGridDimensions.PositiveZ,
                NegativeX = DefineGridDimensions.NegativeX,
                NegativeY = DefineGridDimensions.NegativeY,
                NegativeZ = DefineGridDimensions.NegativeZ
            }.Schedule(this, inputDependencies);

            barrier.AddJobHandleForProducer(job);

            return job;
        }
    }
}
