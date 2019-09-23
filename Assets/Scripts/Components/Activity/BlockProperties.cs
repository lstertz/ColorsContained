using Unity.Entities;
using UnityEngine;

namespace Components.Activity
{
    /// <summary>
    /// Defines the BlockProperties Component, which details the properties 
    /// specific to a Block.
    /// </summary>
    public struct BlockProperties : IComponentData
    {
        public Color Color;
    }
}
