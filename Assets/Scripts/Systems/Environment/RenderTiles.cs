using Components.Activity;
using Components.Attributes;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the Render Tiles System, which renders all tiles as 
    /// defined by Entities with Tile Properties and a Position.
	/// </summary>
    [UpdateAfter(typeof(TrimGrid))]
    [UpdateAfter(typeof(Reset))]
    public class RenderTiles : ComponentSystem
	{
        #region Mesh
        private static readonly Quaternion identity = Quaternion.identity;
        private const int tileSize = 1;

        private static readonly Vector3[] vertices = new Vector3[] {
            new Vector3(tileSize, 0, tileSize),
            new Vector3(tileSize, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, tileSize)
        };

        private static readonly int[] triangles = new int[] {
            0, 1, 2,
            2, 3, 0
        };

        private static readonly Vector2[] uv = new Vector2[] {
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(0, 1)
        };

        private static readonly Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            uv = uv
        };

        private static readonly MaterialPropertyBlock propertyBlock =
            new MaterialPropertyBlock();
        #endregion

        
        [BurstCompile]
		protected override void OnUpdate()
		{
            Camera camera = Camera.main;

            Color noAccentColor = new Color(1, 1, 1, 0);

            ComponentDataFromEntity<GridElement> gridElements =
                GetComponentDataFromEntity<GridElement>(true);
            ComponentDataFromEntity<Indestructible> indestructibles =
                GetComponentDataFromEntity<Indestructible>(true);

            Entities.ForEach((Entity entity,
                ref TileProperties properties, ref Position position) =>
            {
                Color primary = new Color(0, 0, 0, 0);
                Color secondary = new Color(0, 0, 0, 0);
                Color tertiary = new Color(0, 0, 0, 0);
                Color color = properties.UnactivatedColor;
                Color accentColor = new Color(0, 0, 0, 0);
                if (gridElements.Exists(entity))
                {
                    accentColor = new Color(1, 1, 1, 1);
                    secondary = new Color(1, 1, 1, 1);
                    color = new Color(1,1,1,0);
                }

                if (Input.DrawLocations.Contains(position.Value))
                {
                    color = new Color(1, 1, 1, 1);
                    primary = new Color(0.8745f, 0.5843f, 0.2784f, 1.0f);
                    secondary = new Color(1, 1, 1, 1);
                    tertiary = new Color(0.8745f, 0.5843f, 0.2784f, 1.0f);
                    //color = new Color(color.r, color.g, color.b, 1.0f); // Use Property's Color.
                    accentColor = color;// new Color(1, 1, 1, 1);

                    if (properties.IsInvalidDraw)
                    {
                        primary = properties.InvalidColor;
                        tertiary = properties.InvalidColor;
                    }
                }
                else if (EntityManager.Exists(properties.Tileset))
                {
                    primary = properties.ActivatedColor;
                    tertiary = properties.ActivatedColor;
                    secondary = new Color(1, 1, 1, 1);
                    color = new Color(1, 1, 1, 1);
                    accentColor = new Color(1, 1, 1, 1);
                }

                if (indestructibles.Exists(properties.Tileset))
                {
                    tertiary = new Color(1, 1, 1, 1);
                }

                propertyBlock.SetColor("_MainColor", color);
                propertyBlock.SetColor("_Primary", primary);
                propertyBlock.SetColor("_Secondary", secondary);
                propertyBlock.SetColor("_Tertiary", tertiary);

                if (!(properties.IsBeingDrawn && properties.HasDrawnTileToLeft) && 
                    !(!properties.IsBeingDrawn && properties.HasTileToLeft))
                    propertyBlock.SetColor("_Accent1", accentColor);
                else
                    propertyBlock.SetColor("_Accent1", primary);

                if (!(properties.IsBeingDrawn && properties.HasDrawnTileBehind) &&
                    !(!properties.IsBeingDrawn && properties.HasTileBehind))
                    propertyBlock.SetColor("_Accent2", accentColor);
                else
                    propertyBlock.SetColor("_Accent2", primary);

                if (!(properties.IsBeingDrawn && properties.HasDrawnTileToRight) &&
                    !(!properties.IsBeingDrawn && properties.HasTileToRight))
                    propertyBlock.SetColor("_Accent3", accentColor);
                else
                    propertyBlock.SetColor("_Accent3", primary);

                if (!(properties.IsBeingDrawn && properties.HasDrawnTileToForward) &&
                    !(!properties.IsBeingDrawn && properties.HasTileToForward))
                    propertyBlock.SetColor("_Accent4", accentColor);
                else
                    propertyBlock.SetColor("_Accent4", primary);

                // TODO :: Filter by current Z Axis for better 3D interaction later.
                //              Perhaps handle in a Job with an added Tag?

                Graphics.DrawMesh(mesh, position.Value, identity,
                    Resources.GridTileMaterial, 0, camera, 0, propertyBlock);
            });
        }
    }
}
