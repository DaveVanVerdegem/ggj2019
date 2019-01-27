using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public Monster monster;
    public AbstractMinigame[] minigames;

    private int currentMinigameIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        monster.SetCurrentActionProperties(minigames[currentMinigameIndex].GetActionQueueProperties());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteCurrentMinigame()
    {
        Debug.Log(string.Format("Minigame {0} completed!", currentMinigameIndex), this);
        currentMinigameIndex++;
        if(currentMinigameIndex < minigames.Length)
        {
            Debug.Log(string.Format("preparing next minigame {0}!", currentMinigameIndex), this);
            monster.SetCurrentActionProperties(minigames[currentMinigameIndex].GetActionQueueProperties());
        }
        else
        {
            //win the game!
            Debug.Log("completed all minigames!", this);
        }

    }
}
