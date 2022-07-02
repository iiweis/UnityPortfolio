using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public int Level { get; set; } = 1;

    public int ResultCount { get; private set; }


    private GameManager() { }

    public static GameManager Instance { get; } = new GameManager();

    public void Resut()
    {
        Level = 1;
        ResultCount = 0;
    }
}
