using Unity.Entities;


namespace Components.Supports
{
    /// <summary>
    /// Defines the Represents Component, which details an Entity 
    /// of which this Component's Entity represents.
    /// </summary>
    public struct Represents : IComponentData
	{
		public Entity Entity { get; set; }
	}
}
