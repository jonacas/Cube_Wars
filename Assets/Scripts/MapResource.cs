using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapResource : MonoBehaviour {


	public StageData.resourceType currentType;
	public int unitFromPlayer; 
	//0 = resource, 1 = player1, 2 = player2


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.D)) {

            StageData.currentInstance.GetNodeFromPosition(transform.position).setInfluence(StageData.resourceType.Resource);


        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            StageData.currentInstance.GetNodeFromPosition(transform.position).setInfluence(StageData.resourceType.Army);


        }

    }
}
