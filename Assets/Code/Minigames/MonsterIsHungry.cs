using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The monster is hungry. Feed him food!
/// </summary>
public class MonsterIsHungry : MonoBehaviour
{

    public GameObject hotspot;

    public ActionQueueProperties actionQueueProperties;

    private Collider2D mouthCollider;
    private InputTrigger inputTrigger;

    // Start is called before the first frame update
    void Start()
    {
        mouthCollider = GetComponent<Collider2D>();
        inputTrigger = hotspot.GetComponent<InputTrigger>();
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
        }
    }

    void CompleteMinigame()
    {
        Debug.Log("The monster is fed and happy!", this);
    }
}
