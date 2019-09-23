using Components.Activity;
using Components.Attributes;
using Entities.Environment;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the Populate Grid System, which changes Tiles 
    /// to Grid Tiles, for those Tiles that are within the updated Grid Dimensions.
	/// </summary>
	[UpdateAfter(typeof(DefineGridDimensions))]
	public class PopulateGrid : JobComponentSystem
	{
        [ExcludeComponent(typeof(GridElement))]
        private struct PopulateJob : IJobProcessComponentDataWithEntity<Position, TileProperties>
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
                [ReadOnly] ref TileProperties properties)
            {
                if (PositiveX > position.Value.x &&
                    PositiveY > position.Value.y &&
                    PositiveZ > position.Value.z &&
                    NegativeX <= position.Value.x &&
                    NegativeY <= position.Value.y &&
                    NegativeZ <= position.Value.z)
                {
                    Buffer.AddComponent(index, entity, new GridElement());
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
            JobHandle job = new PopulateJob()
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
