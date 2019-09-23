using UnityEngine;


/// <summary>
/// Maintains static global reference resources.
/// </summary>
public static class Resources
{
    public static readonly Color Darkener = new Color(0.1f, 0.1f, 0.1f, 1.0f);
    public static readonly Color Lightener = new Color(0.9f, 0.9f, 0.9f, 1.0f);

    public static readonly Color BackgroundColor = new Color(0.345f, 0.6666f, 0.4509f, 1.0f);
    public const int BackgroundHalfSize = 5;
    public static readonly Color BackgroundTileColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);
    public const float BackgroundTileEmission = 0.25f;

    public static readonly Color DefaultGridTileColor = new Color(0.8745f, 0.5843f, 0.2784f, 1.0f);

    public static Material BlockMaterial;
    public static Material ShadedBlockMaterial;

    public static Material GridTileMaterial;
}
