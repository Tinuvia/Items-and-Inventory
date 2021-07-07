using UnityEngine;
using Tinuvia.CharacterStats;

public class StatPanel : MonoBehaviour
{
    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;

    private CharacterStat[] stats;

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<StatDisplay>();
        UpdateStatNames();
    }

    public void SetStats(params CharacterStat[] charStats) 
        // params allows for variable number of arguments
    {
        stats = charStats;

        if (stats.Length > statDisplays.Length)
            // if we have more stats than display, throw error
        {
            Debug.LogError("Not enough Stat Displays!");
            return;
        }

        for (int i = 0; i < statDisplays.Length; i++)
            // if we have more stat displays than stats, disable the extras
        {
            statDisplays[i].gameObject.SetActive(i < stats.Length);
        }
    }

    public void UpdateStatValues()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            statDisplays[i].ValueText.text = stats[i].Value.ToString();
        }
    }

    public void UpdateStatNames()
    {
        for (int i = 0; i < statNames.Length; i++)
        {
            statDisplays[i].NameText.text = statNames[i];
        }
    }


}
