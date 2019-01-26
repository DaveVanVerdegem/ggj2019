using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public Monster monster;
    public IMinigame[] minigames;

    private int currentMinigameIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteCurrentMinigame()
    {
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
