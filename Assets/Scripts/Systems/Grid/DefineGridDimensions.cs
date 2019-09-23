using Components.Activity;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the Define Grid Dimensions System, which evaluates all Dimensions to 
    /// define the dimensions of the Grid.
	/// </summary>
    [AlwaysUpdateSystem]
    [UpdateAfter(typeof(DestroyTilesets))]
	public class DefineGridDimensions : ComponentSystem
	{
        public static float Area
        {
            get
            {
                float area = Mathf.Abs(
                    (NegativeX - PositiveX) *
                    (NegativeY - PositiveY) *
                    (NegativeZ - PositiveZ));

                if (area == Mathf.Infinity)
                    return 0.0f;
                return area;
            }
        }

        public static float PositiveX { get; private set;}
        public static float PositiveY { get; private set;}
        public static float PositiveZ { get; private set;}

        public static float NegativeX { get; private set;}
        public static float NegativeY { get; private set;}
        public static float NegativeZ { get; private set;}


        [BurstCompile]
        protected override void OnUpdate()
        {
            float newPositiveX = -Mathf.Infinity;
            float newNegativeX = Mathf.Infinity;
            float newPositiveY = -Mathf.Infinity;
            float newNegativeY = Mathf.Infinity;
            float newPositiveZ = -Mathf.Infinity;
            float newNegativeZ = Mathf.Infinity;
            
            Entities.ForEach((Entity entity, ref Dimensions dimensions) =>
            {
                if (dimensions.PositiveX > newPositiveX)
                    newPositiveX = dimensions.PositiveX;
                if (dimensions.NegativeX < newNegativeX)
                    newNegativeX = dimensions.NegativeX;

                if (dimensions.PositiveY > newPositiveY)
                    newPositiveY = dimensions.PositiveY;
                if (dimensions.NegativeY < newNegativeY)
                    newNegativeY = dimensions.NegativeY;

                if (dimensions.PositiveZ > newPositiveZ)
                    newPositiveZ = dimensions.PositiveZ;
                if (dimensions.NegativeZ < newNegativeZ)
                    newNegativeZ = dimensions.NegativeZ;
            });

            PositiveX = newPositiveX;
            NegativeX = newNegativeX;

            PositiveY = newPositiveY;
            NegativeY = newNegativeY;

            PositiveZ = newPositiveZ;
            NegativeZ = newNegativeZ;
        }
    }
}
