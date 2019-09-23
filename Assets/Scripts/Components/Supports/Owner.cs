using Unity.Entities;


namespace Components.Supports
{
	/// <summary>
	/// Defines the Owner Component, which details an Entity 
	/// to which this Component's Entity belongs.
	/// </summary>
	public struct Owner : IComponentData
	{
		public Entity Entity { get; set; }
	}
}
