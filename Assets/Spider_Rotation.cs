using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Rotation : MonoBehaviour
{
    // Start is called before the first frame update
	public float speed=100;
	public float amazeSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,speed*Time.deltaTime));
		transform.GetChild(0).transform.Rotate(new Vector3(0,0,(-speed+amazeSpeed)*Time.deltaTime));
    }
}
