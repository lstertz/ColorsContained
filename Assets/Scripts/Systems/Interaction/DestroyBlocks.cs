using Components.Activity;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the DestroyBlocks System, which handles the destruction of all Blocks.
	/// </summary>
	[AlwaysUpdateSystem]
    public class DestroyBlocks : JobComponentSystem
    {
        private struct DestroyBlocksJob : IJobProcessComponentDataWithEntity<BlockProperties>
        {
            [WriteOnly]
            public EntityCommandBuffer.Concurrent Buffer;

            public void Execute(Entity entity, int index,
                [ReadOnly] ref BlockProperties properties)
            {
                Buffer.DestroyEntity(index, entity);
            }
        }

        private EntityCommandBufferSystem destroyBlocksBarrier;

        protected override void OnCreateManager()
        {
            Enabled = false;
            destroyBlocksBarrier =
                World.GetOrCreateManager<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            EntityCommandBuffer.Concurrent destroyBlocksBuffer =
                destroyBlocksBarrier.CreateCommandBuffer().ToConcurrent();

            JobHandle destroyBlocksJob = new DestroyBlocksJob()
            {
                Buffer = destroyBlocksBuffer
            }.Schedule(this, inputDependencies);
            destroyBlocksBarrier.AddJobHandleForProducer(destroyBlocksJob);

            return destroyBlocksJob;
        }
    }
}
