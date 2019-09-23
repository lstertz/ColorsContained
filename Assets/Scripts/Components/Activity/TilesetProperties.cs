using Unity.Entities;
using UnityEngine;

namespace Components.Activity
{
    /// <summary>
    /// Defines the TilesetProperties Component, which details the properties 
    /// specific to a set of tiles.
    /// </summary>
    public struct TilesetProperties : IComponentData
    {
        public int TileCount;
    }
}
