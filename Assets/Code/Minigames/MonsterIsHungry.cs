using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The monster is hungry. Feed him food!
/// </summary>
public class MonsterIsHungry : AbstractMinigame
{

    public GameObject monster;

    public ActionQueueProperties actionQueueProperties;

    /// <summary>
    /// Happy audio clips
    /// </summary>
    [Tooltip("Eating audio clips")]
    [SerializeField]
    private RandomAudioClip eatingSounds = null;

    private MinigameManager minigameManager; // ugly but at least this one should work

    private Collider2D mouthCollider;
    private InputTrigger inputTrigger;
    private Monster monsterScript;
    private int foodCount;
    private int currentFoodCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.AddComponent<Collider2D>(hotspot.GetComponent<Collider2D>());
        mouthCollider = GetComponent<Collider2D>();
        monsterScript = monster.GetComponent<Monster>();
        minigameManager = GetComponentInParent<MinigameManager>();
        foodCount = actionQueueProperties.Actions.Count;
    }

    // Update is called once per frame
    void Update()
    {
        //drag items tagged as food on the monsters mouth

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.FOOD))
        {
            if(currentFoodCount < foodCount)
            {
                Debug.Log("The monster ate food.", this);
                eatingSounds.PlayRandomAudioClip();
                Destroy(other.gameObject);
                monsterScript.RegisterAction(ActionType.DragAndDrop, HotSpotLocation.Teeth);
            }
            if(currentFoodCount >= foodCount)
            {
                minigameManager.CompleteCurrentMinigame();
            }
            currentFoodCount++;
        }

    }

    #region IMinigame methods
    public override ActionQueueProperties GetActionQueueProperties()
    {
        return actionQueueProperties;
    }
    #endregion
}
