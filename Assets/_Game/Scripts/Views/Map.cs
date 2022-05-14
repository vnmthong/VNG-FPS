using PYDFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private void Awake()
    {
        Singleton<Map>.Set(this);
    }

    private void OnDestroy()
    {
        Singleton<Map>.Unset(this);
    }

    public List<Transform> foodSlots;
    public List<Transform> enemySlots;

    public Transform parentObject;
    public Transform GetSlot(int index, TeamType teamType)
    {
        var isHeroSlot = teamType == TeamType.Hero;
        var slots = isHeroSlot ? foodSlots : enemySlots;
        index = isHeroSlot ? index : index - 1;
        if (index < 0 || index >= slots.Count)
            return null;

        return slots[index];
    }
}
