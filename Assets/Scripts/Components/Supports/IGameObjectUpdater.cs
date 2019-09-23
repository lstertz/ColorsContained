namespace Components.Supports
{
	/// <summary>
	/// Defines the interface of a GameObject updater.
	/// </summary>
	public interface IGameObjectUpdater
	{
		/// <summary>
		/// Updates the GameObject for the specifics of the implementor.
		/// </summary>
		/// <param name="gameObject">The GameObject to be updated.</param>
		void Update(UnityEngine.GameObject gameObject);
	}
}
