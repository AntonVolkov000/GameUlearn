using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllShards : MonoBehaviour
{
    public GameObject allShardsWisest;
    public GameObject notAllShardsWisest;
    public GameObject allShardsObelisk;
    public GameObject notAllShardsObelisk;
    private void FixedUpdate()
    {
        var generalVariable = GameObject.FindGameObjectWithTag("GeneralVar").GetComponent<GeneralVar>();
        var countShards = generalVariable.countShards;
        if (countShards == 6)
        {
            allShardsWisest.SetActive(true);
            allShardsObelisk.SetActive(true);
            notAllShardsWisest.SetActive(false);
            notAllShardsObelisk.SetActive(false);
        }
        else
        {
            allShardsWisest.SetActive(false);
            allShardsObelisk.SetActive(false);
            notAllShardsWisest.SetActive(true);
            notAllShardsObelisk.SetActive(true);
        }
    }
}
