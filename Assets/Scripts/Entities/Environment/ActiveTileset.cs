using Components.Activity;
using Components.Attributes;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Wrappers;


namespace Entities.Environment
{
    /// <summary>
    /// Defines the Active Tileset Entity Setup.
    /// </summary>
    public class ActiveTileset : Entity<ActiveTileset>
    {
        /// <summary>
        /// Creates, with a Concurrent Command Buffer, a new Entity whose setup 
        /// is defined by this class.
        /// </summary>
        /// <param name="buffer">The Concurrent Command Buffer that will be used 
        /// to create the Entity.</param>
        /// <param name="jobIndex">The index of the current concurrent job.</param>
        public new static void BufferedCreate(EntityCommandBuffer.Concurrent buffer, int jobIndex)
        {
            Entity<ActiveTileset>.BufferedCreate(buffer, jobIndex);
        }

        /// <summary>
        /// Creates a new Entity whose setup is defined by this class.
        /// </summary>
        /// <param name="tilePositions">The positions of the Tiles contained 
        /// within this Tileset.</param>
        /// <param name="indestructible">Specifies whether this Tileset 
        /// can be destroyed through user interaction.</param>
        public static Entity Create(float3[] tilePositions, bool indestructible = false)
        {
            for (int c = 0, count = tilePositions.Length; c < count; c++)
            {
                float3 tilePosition = tilePositions[c];

                if (tilePosition.x + 1 > Setup.positiveX)
                    Setup.positiveX = tilePosition.x + 1;
                if (tilePosition.x < Setup.negativeX)
                    Setup.negativeX = tilePosition.x;

                if (tilePosition.y + 1 > Setup.positiveY)
                    Setup.positiveY = tilePosition.y + 1;
                if (tilePosition.y < Setup.negativeY)
                    Setup.negativeY = tilePosition.y;

                if (tilePosition.z + 1 > Setup.positiveZ)
                    Setup.positiveZ = tilePosition.z + 1;
                if (tilePosition.z < Setup.negativeZ)
                    Setup.negativeZ = tilePosition.z;
            }

            Setup.indestructible = indestructible;
            Setup.tileCount = tilePositions.Length;
            Entity e = Create();

            Setup.negativeX = Mathf.Infinity;
            Setup.negativeY = Mathf.Infinity;
            Setup.negativeZ = Mathf.Infinity;
            Setup.positiveX = -Mathf.Infinity;
            Setup.positiveY = -Mathf.Infinity;
            Setup.positiveZ = -Mathf.Infinity;

            return e;
        }


        private float negativeX = Mathf.Infinity;
        private float negativeY = Mathf.Infinity;
        private float negativeZ = Mathf.Infinity;

        private float positiveX = -Mathf.Infinity;
        private float positiveY = -Mathf.Infinity;
        private float positiveZ = -Mathf.Infinity;

        private int tileCount = 0;
        private bool indestructible = false;


        /// <summary>
        /// Sets the Components of this Entity.
        /// </summary>
        /// <param name="wrapper">The EntityWrapper that allows the Components 
        /// to be set, agnostic to how the setting is performed.</param>
        protected override void SetComponents(EntityWrapper wrapper)
        {
            wrapper.SetComponent(new TilesetProperties
            {
                TileCount = tileCount
            });

            wrapper.SetComponent(new Dimensions
            {
                NegativeX = negativeX,
                NegativeY = negativeY,
                NegativeZ = negativeZ,
                PositiveX = positiveX,
                PositiveY = positiveY,
                PositiveZ = positiveZ
            });

            if (indestructible)
                wrapper.AddComponent(new Indestructible());
        }

        /// <summary>
        /// Provides the ComponentTypes of this Entity.
        /// </summary>
        /// <returns>An array of this Entities ComponentTypes.</returns>
        protected override ComponentType[] GetComponentTypes()
        {
            return new ComponentType[]
            {
                typeof(Dimensions),
                typeof(TilesetProperties)
            };
        }
    }
}
