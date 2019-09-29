using Entities.Environment;
using System.Collections;
using Systems.Activity;
using Systems.Progression;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the state of the game (existing Entities) for the current level.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private const float highestPermittedBuiltY = -1.0f;

    public static bool CanProgress
    {
        set
        {
            instance.Next.interactable = value;
        }
    }

    private int currentLevel = 0;
    private static LevelManager instance;

    private static int[][] drawRestrictions = new int[][]
    {
        new int[] { 4 },
        new int[] { 6 },
        new int[] { 4 },
        new int[] { 5 },
        new int[] { 4 },
        new int[] { 5 },
        new int[] { 4 }
    };
    private static int[] goalRequirements = new int[]
    {
        8, 
        10,
        6, 
        13, 
        11,
        8,
        12
    };


    public void Progress()
    {
        currentLevel++;
        Restart.interactable = true;

        DestroyGoals destroyGoals = World.Active.GetOrCreateManager<DestroyGoals>();
        destroyGoals.Enabled = true;
        {
            destroyGoals.Update();
        }
        destroyGoals.Enabled = false;

        if (currentLevel < goalRequirements.Length)
        {
            CountActivations count = World.Active.GetOrCreateManager<CountActivations>();
            count.Enabled = true;
            {
                count.Update();
            }
            count.Enabled = false;

            Flip flip = World.Active.GetOrCreateManager<Flip>();
            flip.Enabled = true;
            {
                flip.Update();
            }
            flip.Enabled = false;

            Systems.Activity.Input.SetDrawRestrictions(drawRestrictions[currentLevel]);
            Goal.Create(goalRequirements[currentLevel],
                new Color(1, 1, 1, 1), new Color(0.345f, 0.6666f, 0.4509f, 1));
        }
        else
        {
            CountActivations count = World.Active.GetOrCreateManager<CountActivations>();
            count.Enabled = true;
            {
                count.Update();
            }
            count.Enabled = false;

            Reset reset = World.Active.GetOrCreateManager<Reset>();
            reset.Enabled = true;
            {
                reset.Update(false);
            }
            reset.Enabled = false;

            Next.gameObject.SetActive(false);
            Systems.Activity.Input.SetDrawRestrictions(null);
            //World.Active.GetOrCreateManager<RenderTiles>().Enabled = false;
        }
    }

    public void JumpToFirstLevel()
    {
        Reset reset = World.Active.GetOrCreateManager<Reset>();
        reset.Enabled = true;
        {
            reset.Update(true);
        }
        reset.Enabled = false;

        currentLevel = 0;

        DestroyGoals destroyGoals = World.Active.GetOrCreateManager<DestroyGoals>();
        destroyGoals.Enabled = true;
        {
            destroyGoals.Update();
        }
        destroyGoals.Enabled = false;

        StartCoroutine(PostJump());

        Systems.Activity.Input.SetDrawRestrictions(drawRestrictions[currentLevel]);
        Goal.Create(goalRequirements[currentLevel],
            new Color(1, 1, 1, 1), new Color(0.345f, 0.6666f, 0.4509f, 1));
    }


    public Button Next;
    public Button Restart;

    private void Start()
    {
        instance = this;

        // Temporarily define the first level on Awake.
        Systems.Activity.Input.SetDrawRestrictions(drawRestrictions[0]);
        Goal.Create(goalRequirements[0], 
            new Color(1, 1, 1, 1), new Color(0.345f, 0.6666f, 0.4509f, 1));

        Restart.interactable = false;
    }

    private IEnumerator PostJump()
    {
        yield return new WaitForEndOfFrame();

        Restart.interactable = false;
        Next.gameObject.SetActive(true);
        World.Active.GetOrCreateManager<RenderTiles>().Enabled = true;
    }
}
