using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCropper : MonoBehaviour
{
    private NPC npc;
    public List<Sprite> SwimmingSprite;

    void Start()
    {
        npc = GetComponent<NPC>();
    }

    void Update()
    {
        if (npc.GetStatus() == NPCStatus.Swimming || npc.GetStatus() == NPCStatus.Hottub)
        {
            Debug.Log("swim");
            this.gameObject.GetComponent<SpriteRenderer>().sprite = 
        }
    }
}