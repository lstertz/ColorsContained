using Unity.Entities;


namespace Utilities.Wrappers
{
	/// <summary>
	/// Defines a wrapper for performing Entity actions without being concerned with 
	/// what performs the actions.
	/// </summary>
	public partial struct EntityWrapper
	{
		/// <summary>
		/// Defines the EntityWrapper ForSharedComponent Interface, 
		/// for specifying SharedComponent-typed methods for interacting with the EntityWrapper.
		/// </summary>
		public interface IForSharedComponent
		{
			void AddSharedComponent(ISharedComponentData component);
			void RemoveComponent();
			void SetSharedComponent(ISharedComponentData component);
		}

		/// <summary>
		/// Defines the EntityWrapper ForSharedComponent, for providing SharedComponent-typed 
		/// methods for interacting with the EntityWrapper.
		/// </summary>
		public struct ForSharedComponent<T> : IForSharedComponent 
			where T : struct, ISharedComponentData
		{
			/// <summary>
			/// The EntityWrapper wrapped by this wrapper.
			/// </summary>
			public EntityWrapper Wrapper { private get; set; }


			/// <summary>
			/// Adds the provided shared Component to the current Entity.
			/// </summary>
			/// <param name="component">The shared Component being added.</param>
			public void AddSharedComponent(ISharedComponentData component)
			{
				Wrapper.AddSharedComponent((T)component);
			}

			/// <summary>
			/// Removes the Component of the specified Type.
			/// Does not remove a Component if the action is for a CommandBuffer when 
			/// the Entity has not been specified.
			/// </summary>
			public void RemoveComponent()
			{
				Wrapper.RemoveComponent<T>();
			}

			/// <summary>
			/// Sets the provided shared Component for the current Entity.
			/// </summary>
			/// <param name="component">The shared Component being set.</param>
			public void SetSharedComponent(ISharedComponentData component)
			{
				Wrapper.SetSharedComponent((T)component);
			}
		}


		/// <summary>
		/// Provides a SharedComponent-typed wrapper for the EntityWrapper, enabling the 
		/// generic methods of the EntityWrapper to be called from a source that is 
		/// unaware of the typing.
		/// </summary>
		/// <typeparam name="T">The Type of the SharedComponent being bound.</typeparam>
		/// <returns>The SharedComponent-typed wrapper.</returns>
		public ForSharedComponent<T> GetTypedShared<T>() 
			where T : struct, ISharedComponentData
		{
			return new ForSharedComponent<T> { Wrapper = this };
		}
	}
}
