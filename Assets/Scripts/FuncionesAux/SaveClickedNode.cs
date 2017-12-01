using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveClickedNode : MonoBehaviour {

    StageData m_Instance;
    Camera c;

    private void Start()
    {
        m_Instance = StageData.currentInstance;
        c = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Instance.LastClickedNode = m_Instance.GetNodeFromPosition(c.ScreenToViewportPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane)));
        }
    }

}
