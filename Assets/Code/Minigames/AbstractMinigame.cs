using UnityEngine;
using System.Collections;
/// <summary>
/// parent of minigames. Needed for showing up in the UnityEditor because IMinigame is not a GameObject.
/// </summary>
public abstract class AbstractMinigame : MonoBehaviour, IMinigame
{
    public abstract ActionQueueProperties GetActionQueueProperties();
}
