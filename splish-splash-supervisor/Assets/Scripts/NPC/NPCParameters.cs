using UnityEngine;

[CreateAssetMenu(fileName = "CommonNames", menuName = "NPC/Common Names", order = 1)]
public class NPCParameters : ScriptableObject
{
    public string[] names;
    public int maxHealth;
    public int minHealth;
    public int maxSatisfaction;
    public int minSatisfaction;
    public float swimVelocity;
    public float changeDirectionIntervalMax;
    public float changeDirectionIntervalMin;
    public float hottubVelocity;
    public float moveSpeed;
}
