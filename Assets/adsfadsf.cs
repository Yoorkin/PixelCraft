using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adsfadsf : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public void addddddd()
    {
        GameObject.Find("Map").GetComponent<Map>().LoadChuck(new Vector2(4,4));
        GameObject.Find("Map").GetComponent<Map>().LoadChuck(new Vector2(4, 5));
        GameObject.Find("Map").GetComponent<Map>().LoadChuck(new Vector2(4, 6));
        GameObject.Find("Map").GetComponent<Map>().LoadChuck(new Vector2(4, 7));
        GameObject.Find("Map").GetComponent<Map>().LoadChuck(new Vector2(4, 8));
        GameObject.Find("Map").GetComponent<Map>().ChuckStream[new Vector2(4, 4)].Present();
        GameObject.Find("Map").GetComponent<Map>().ChuckStream[new Vector2(4, 5)].Present();
        GameObject.Find("Map").GetComponent<Map>().ChuckStream[new Vector2(4, 6)].Present();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
