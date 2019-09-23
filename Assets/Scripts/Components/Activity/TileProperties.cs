using Unity.Entities;
using UnityEngine;

namespace Components.Activity
{
    /// <summary>
    /// Defines the TileProperties Component, which details the properties 
    /// specific to a Tile.
    /// </summary>
    public struct TileProperties : IComponentData
    {
        public int ActivationCount;
        public Color ActivatedColor;
        public Color InvalidColor;
        public Color UnactivatedColor;

        public bool HasTileToForward;
        public bool HasTileBehind;
        public bool HasTileToLeft;
        public bool HasTileToRight;

        public bool IsBeingDrawn;
        public bool IsInvalidDraw;
        public bool HasDrawnTileToForward;
        public bool HasDrawnTileBehind;
        public bool HasDrawnTileToLeft;
        public bool HasDrawnTileToRight;

        public Entity Tileset;
    }
}
