using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurSingletonFriend : MonoBehaviour
{
    private static OurSingletonFriend instance = null;
    private bool currentDifficulty = true; //TODO: REMOVE TRUE WHEN ASSIGNED TO MENU.

    private OurSingletonFriend() { } //Defeats Instantiation

    public static OurSingletonFriend getInstance()
    {
        if (instance == null)
        {
            instance = new OurSingletonFriend();
        }
        return instance;
    }
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void SetDifficultyHard()
    {
        currentDifficulty = true;
    }
    public void SetDifficultyNormal()
    {
        currentDifficulty = false;
    }
    public bool GetDifficulty()
    {
        return currentDifficulty;
    }
}