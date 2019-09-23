using UnityEngine;
using UnityEngine.UI;

public class GoalUI : MonoBehaviour
{
    public Image Background;
    public RawImage BlockType = null;
    public RawImage TileType = null;
    public Text Requirement;

    private int currentMet = 0;
    private int total = 0;
    

    public void SetBackgroundColor(Color color)
    {
        if (Background.color != color)
            Background.color = color;
    }

    public void SetRequirementMet(int currentMet)
    {
        if (this.currentMet != currentMet)
            UpdateRequirement(currentMet, total);
    }

    public void SetType(Color color)
    {
        // TODO :: Possibly consider Shades and other requirements.

        if (TileType != null)
        {
            if (TileType.color != color)
                TileType.color = color;
        }
        else if (BlockType != null)
        {
            if (BlockType.color != color)
                BlockType.color = color;
        }
    }

    public void SetTotalRequired(int total)
    {
        if (this.total != total)
            UpdateRequirement(currentMet, total);
    }

    private void UpdateRequirement(int currentMet, int total)
    {
        this.currentMet = currentMet;
        this.total = total;

        Requirement.text = currentMet.ToString() + '\\' + total.ToString();

        if (currentMet == total)
            Requirement.color = Color.white;
        else
            Requirement.color = new Color(1.0f, 0.4915f, 0.2878f, 1.0f);
    }
}
