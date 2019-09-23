using Entities;
using Unity.Entities;


namespace Components.Supports
{
	/// <summary>
	/// Defines the interface of a construct with a GameObject.
	/// </summary>
	public interface IGameObject
	{
		/// <summary>
		/// Provides the implementor's GameObject.
		/// </summary>
		UnityEngine.GameObject Value { get; }
	}

	/// <summary>
	/// Defines the GameObject Component, which specifies that this Component's Entity, 
	/// the Entity provided, is associated with a GameObject.
	/// </summary>
	public struct GameObject : IComponentData, IGameObject
	{
		/// <summary>
		/// The Entity associated with the GameObject.
		/// </summary>
		public Entity Entity { get; set; }

		/// <summary>
		/// Provides the GameObject.
		/// </summary>
		public UnityEngine.GameObject Value
		{
			get
			{
				return this.GetGameObject(Entity);
			}
		}
	}
}
