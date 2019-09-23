using Unity.Entities;


namespace Components.Activity
{
	/// <summary>
	/// Defines an Action Component, which details the time until an effect may be triggered.
	/// </summary>
	public struct Action : IComponentData
	{
		public float TimeUntilEffect { get; set; }
	}
}
