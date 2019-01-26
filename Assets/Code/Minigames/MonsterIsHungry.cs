using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The monster is hungry. Feed him food!
/// </summary>
public class MonsterIsHungry : MonoBehaviour
{

    public GameObject monster;

    public ActionQueueProperties actionQueueProperties;

    private Collider2D mouthCollider;
    private InputTrigger inputTrigger;
    private Monster monsterScript;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.AddComponent<Collider2D>(hotspot.GetComponent<Collider2D>());
        mouthCollider = GetComponent<Collider2D>();
        monsterScript = monster.GetComponent<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        //drag items tagged as food on the monsters mouth

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("in collider", this);

        if (other.gameObject.CompareTag(Tags.FOOD))
        {
            Debug.Log("Food in mouth!", this);
            Destroy(other.gameObject);
            monsterScript.RegisterAction(ActionType.DragAndDrop, HotSpotLocation.Teeth);
        }
    }

    //maybe this is not needed
    //void CompleteMinigame()
    //{
    //    Debug.Log("The monster is fed and happy!", this);
    //}
}
