using Unity.Entities;


namespace Components.Interaction
{
	/// <summary>
	/// Defines the Directive Component, which details an Action or Movement Direction 
	/// to be undertaken by its Entity.
	/// </summary>
	public struct Directive : IComponentData
	{
		public enum ActiveAction
		{
			None,
			StandardAttack
		}
		public ActiveAction Action;

		public enum MovementDirection
		{
			None,
			Forward,
			Back,
			Left,
			Right
		}
		public MovementDirection Movement;
	}
}
