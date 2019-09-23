using Unity.Entities;


namespace Components.Activity
{
	/// <summary>
	/// Defines the Dimensions Component, which details the maximas in 3 dimensions for an Entity.
	/// </summary>
	public struct Dimensions : IComponentData
    {
        public float NegativeX;
        public float NegativeY;
        public float NegativeZ;

        public float PositiveX;
        public float PositiveY;
        public float PositiveZ;
	}
}
