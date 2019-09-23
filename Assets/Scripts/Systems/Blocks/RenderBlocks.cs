using Components.Activity;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Systems.Activity
{
	/// <summary>
	/// Defines the Render Grid System, which renders the grid as a collection of 
    /// grid tiles defined by Grid Tile Properties and a Position.
	/// </summary>
    [UpdateAfter(typeof(TrimGrid))]
    //[AlwaysUpdateSystem]
    public class RenderBlocks : ComponentSystem
	{
        #region Mesh
        private static readonly Quaternion identity = Quaternion.identity;
        private const int tileSize = 1;


        private static readonly Vector3[] topVertices = new Vector3[] {
            new Vector3(tileSize, 0, tileSize),
            new Vector3(tileSize, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, tileSize)
        };

        private static readonly Vector3[] bottomVertices = new Vector3[] {
            new Vector3(tileSize, -tileSize, tileSize),
            new Vector3(tileSize, -tileSize, 0),
            new Vector3(0, -tileSize, 0),
            new Vector3(0, -tileSize, tileSize)
        };

        private static readonly Vector3[] forwardVertices = new Vector3[] {
            new Vector3(tileSize, -tileSize, tileSize),
            new Vector3(tileSize, 0, tileSize),
            new Vector3(0, 0, tileSize),
            new Vector3(0, -tileSize, tileSize)
        };

        private static readonly Vector3[] backwardVertices = new Vector3[] {
            new Vector3(tileSize, -tileSize, 0),
            new Vector3(tileSize, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, -tileSize, 0)
        };

        private static readonly Vector3[] leftVertices = new Vector3[] {
            new Vector3(0, -tileSize, tileSize),
            new Vector3(0, -tileSize, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, tileSize)
        };

        private static readonly Vector3[] rightVertices = new Vector3[] {
            new Vector3(tileSize, -tileSize, tileSize),
            new Vector3(tileSize, -tileSize, 0),
            new Vector3(tileSize, 0, 0),
            new Vector3(tileSize, 0, tileSize)
        };

        private static readonly int[] flippedTriangles = new int[] {
            0, 1, 2,
            2, 3, 0
        };

        private static readonly int[] triangles = new int[] {
            2, 1, 0,
            0, 3, 2
        };

        private static readonly Vector2[] uv = new Vector2[] {
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(0, 1)
        };

        private static readonly Mesh bottomMesh = new Mesh
        {
            vertices = bottomVertices,
            triangles = triangles,
            uv = uv
        };

        private static readonly Mesh topMesh = new Mesh
        {
            vertices = topVertices,
            triangles = flippedTriangles,
            uv = uv
        };

        private static readonly Mesh forwardMesh = new Mesh
        {
            vertices = forwardVertices,
            triangles = flippedTriangles,
            uv = uv
        };

        private static readonly Mesh backwardMesh = new Mesh
        {
            vertices = backwardVertices,
            triangles = triangles,
            uv = uv
        };
        private static readonly Mesh leftMesh = new Mesh
        {
            vertices = leftVertices,
            triangles = triangles,
            uv = uv
        };

        private static readonly Mesh rightMesh = new Mesh
        {
            vertices = rightVertices,
            triangles = flippedTriangles,
            uv = uv
        };

        private static readonly MaterialPropertyBlock propertyBlock =
            new MaterialPropertyBlock();
        #endregion


        protected override void OnCreateManager()
        {
            base.OnCreateManager();
        }

        [BurstCompile]
        protected override void OnUpdate()
		{
            Camera camera = Camera.main;

            Entities.ForEach((ref BlockProperties properties,  ref Position position) =>
            {
                float3 p = position.Value;
                propertyBlock.SetColor("_Color", properties.Color);

                Graphics.DrawMesh(topMesh, p, identity,
                    Resources.BlockMaterial, 0, camera, 0, propertyBlock);
                Graphics.DrawMesh(bottomMesh, p, identity,
                    Resources.ShadedBlockMaterial, 0, camera, 0, propertyBlock);

                Graphics.DrawMesh(forwardMesh, p, identity,
                    Resources.ShadedBlockMaterial, 0, camera, 0, propertyBlock);
                Graphics.DrawMesh(backwardMesh, p, identity,
                    Resources.BlockMaterial, 0, camera, 0, propertyBlock);

                Graphics.DrawMesh(leftMesh, p, identity,
                    Resources.ShadedBlockMaterial, 0, camera, 0, propertyBlock);
                Graphics.DrawMesh(rightMesh, p, identity,
                    Resources.BlockMaterial, 0, camera, 0, propertyBlock);
            });
        }
    }
}
