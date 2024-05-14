using UnityEngine;

[CreateAssetMenu(fileName = "CommonNames", menuName = "NPC/Common Names", order = 1)]
public class NPCParameters : ScriptableObject
{
    public string[] names;
    public int maxHealth;
    public int minHealth;
    public int maxSatisfaction;
    public int minSatisfaction;
}
