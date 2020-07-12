using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int SoulFragments;
    public int BoneShards;
    public int BrownCaps;


    public Inventory(){
        this.SoulFragments = 1;
        this.BoneShards = 1;
        this.BrownCaps = 1;
    }
}
