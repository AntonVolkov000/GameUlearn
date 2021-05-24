using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoMenu : MonoBehaviour
{
    public MainMenu load;
        
    private void startMenu()
    {
        load.PlayGame(0);
    }
}
