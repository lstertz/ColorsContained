using Unity.Entities;
using Utilities.Wrappers;


namespace Components
{
	/// <summary>
	/// Defines the IComponent Interface, for specifying functionality 
	/// of all non-shared Components.
	/// </summary>
	public interface IComponent : IComponentData
	{
		EntityWrapper.IForComponent GetTypedWrapper(EntityWrapper wrapper);
	}
	/// <summary>
	/// Defines the ISharedComponent Interface, for specifying functionality 
	/// of all shared Components.
	/// </summary>
	public interface ISharedComponent : ISharedComponentData
	{
		EntityWrapper.IForSharedComponent GetTypedWrapper(EntityWrapper wrapper);
	}
}
