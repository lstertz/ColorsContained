using Unity.Entities;


namespace Components.Supports
{
	/// <summary>
	/// Defines a Mirror Component, which is a Component that represents a reference 
	/// to an Entity whose Components are to be referenced dynamically, 
	/// as needed by Systems that accommodate Mirror functionality.
	/// </summary>
	public struct Mirror : IComponentData
	{
		/// <summary>
		/// The Entity to be mirrored.
		/// </summary>
		public Entity Entity { get; set; }
	}
}
