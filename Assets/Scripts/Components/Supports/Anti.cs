using Unity.Entities;


namespace Components.Supports
{
    /// <summary>
    /// Defines the Anti-Component Component, which specifies through its generic typing 
    /// any Component whose existence should be ignored by any systems that are 
    /// designed to ignore based on the Anti-Component Component.
    /// </summary>
    /// <typeparam name="T">The Component whose existence should be ignored.</typeparam>
    public struct Anti<T> : IComponentData where T : IComponentData { }
}
