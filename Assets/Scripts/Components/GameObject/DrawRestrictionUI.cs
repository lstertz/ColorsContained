using UnityEngine;
using UnityEngine.UI;

public class DrawRestrictionUI : MonoBehaviour
{
    private static Text restriction;

    private static int currentMet = 0;
    private static int currentTotal = 0;

    public Text Restriction;

    
    public static void SetVisibility(bool isVisible)
    {
        restriction.enabled = isVisible;
    }
    
    public static void SetRequirementMet(int met)
    {
        if (currentMet != met)
            UpdateRequirement(met, currentTotal);
    }
    
    public static void SetTotalRequired(int total)
    {
        if (currentTotal != total)
            UpdateRequirement(currentMet, total);
    }

    private static void UpdateRequirement(int met, int total)
    {
        currentMet = met;
        currentTotal = total;

        restriction.text = met.ToString() + '\\' + total.ToString();

        if (met == total)
            restriction.color = Color.white;
        else
            restriction.color = new Color(1.0f, 0.4915f, 0.2878f, 1.0f);
    }

    private void Awake()
    {
        restriction = Restriction;
        restriction.enabled = false;
    }
}
