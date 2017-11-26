using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapResource : MonoBehaviour {


	public StageData.resourceType currentType;
	public int unitFromPlayer;
    //0 = resource, 1 = player1, 2 = player2

    public Unidad TEST;

	// Use this for initialization
	void Start () {
        TEST = (Unidad)this.gameObject.GetComponent<Explorador>();

    }
	
	// Update is called once per frame
	void Update () {

        /*
        if (Input.GetKeyDown(KeyCode.D)) {

            StageData.currentInstance.GetNodeFromPosition(transform.position).setInfluence(StageData.resourceType.Resource);


        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            StageData.currentInstance.GetNodeFromPosition(transform.position).setInfluence(StageData.resourceType.Army);


        }
        */
       /* if (Input.GetKeyDown(KeyCode.S))
        {
            print(transform.position + StageData.currentInstance.grafoTotal[0, 0].position);
            StageData.currentInstance.GetPathToTarget(transform.position, StageData.currentInstance.grafoTotal[0, 0].position, this.gameObject.GetComponent<Explorador>());
			this.gameObject.GetComponent<Explorador> ().AccionMover ();
        }*/



    }
}
