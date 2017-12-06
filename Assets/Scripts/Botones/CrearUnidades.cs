using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearUnidades : MonoBehaviour {

    public GameObject Unidad;

    public void CreateUnity() {

        Vector3 pos = StageData.currentInstance.LastClickedNode.position;
        Instantiate(Unidad,pos,Quaternion.identity);


    }


}
