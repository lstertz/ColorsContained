using Entities.Environment;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// Sets up the run-time environment and creates the initial Entities to start the game. 
/// </summary>
public class Bootstrap : MonoBehaviour
{
    public Material GridTileMaterial;

    private void Start()
    {
        SetupRuntimeEnvironment();

        GridTileMaterial.enableInstancing = true;
        Resources.GridTileMaterial = GridTileMaterial;
        
        PopulateBackground();
        
        Destroy(gameObject);
    }


    /// <summary>
    /// Sets up the run-time environment.
    /// </summary>
    private void SetupRuntimeEnvironment()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        Input.simulateMouseWithTouches = true;
    }

    private void PopulateBackground()
    {
        for (int x = -Resources.BackgroundHalfSize; x <= Resources.BackgroundHalfSize; x++)
            for (int z = -Resources.BackgroundHalfSize; z <= Resources.BackgroundHalfSize; z++)
                Tile.Create(new float3(x, 0, z));
    }
}
