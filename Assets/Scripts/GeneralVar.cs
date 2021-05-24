using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralVar : MonoBehaviour
{
    public int countHeath;
    public int countShards;

    void Awake() {
        DontDestroyOnLoad(transform.gameObject);        
    }
}