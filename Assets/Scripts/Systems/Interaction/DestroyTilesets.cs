using Components.Activity;
using Components.Attributes;
using Components.Supports;
using Systems.Progression;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the DestroyTilesets System, which handles the 
    /// destruction of Tilesets and the resetting of their Tiles.
	/// </summary>
	[UpdateAfter(typeof(Draw))]
    [UpdateAfter(typeof(Input))]
    [UpdateAfter(typeof(Reset))]
    [UpdateBefore(typeof(DetermineFulfillment))]
    public class DestroyTilesets : JobComponentSystem
    {
        [RequireComponentTag(typeof(Destroy))]
        private struct DestroyTilesetsJob : IJobProcessComponentDataWithEntity<TilesetProperties>
        {
            [WriteOnly]
            public EntityCommandBuffer.Concurrent Buffer;

            public void Execute(Entity entity, int index,
                [ReadOnly] ref TilesetProperties properties)
            {
                Buffer.DestroyEntity(index, entity);
            }
        }

        private struct ResetTilesetTilesJob : IJobProcessComponentDataWithEntity<
            TileProperties>
        {
            [ReadOnly]
            public ComponentDataFromEntity<Destroy> MarkedTilesets;

            public void Execute(Entity entity, int index, ref TileProperties properties)
            {
                if (MarkedTilesets.Exists(properties.Tileset))
                {
                    properties.HasTileToForward = false;
                    properties.HasTileBehind = false;
                    properties.HasTileToLeft = false;
                    properties.HasTileToRight = false;

                    properties.Tileset = Entity.Null;
                }
            }
        }

        private EntityCommandBufferSystem destroyTilesetsBufferSystem;

        protected override void OnCreateManager()
        {
            destroyTilesetsBufferSystem =
                World.GetOrCreateManager<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            ComponentDataFromEntity<Destroy> tilesetsToDestroy =
                GetComponentDataFromEntity<Destroy>();

            JobHandle resetTilesetTilesJob = new ResetTilesetTilesJob()
            {
                MarkedTilesets = tilesetsToDestroy
            }.Schedule(this, inputDependencies);

            EntityCommandBuffer.Concurrent destroyTilsetsBuffer =
                destroyTilesetsBufferSystem.CreateCommandBuffer().ToConcurrent();

            JobHandle destroyTilesetsJob = new DestroyTilesetsJob()
            {
                Buffer = destroyTilsetsBuffer
            }.Schedule(this, resetTilesetTilesJob);
            destroyTilesetsBufferSystem.AddJobHandleForProducer(destroyTilesetsJob);

            return destroyTilesetsJob;
        }
    }
}
