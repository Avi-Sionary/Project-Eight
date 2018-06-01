using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform playerTrans;

    new Camera camera;
    float height;
    float width;
	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
        height = 2f * camera.orthographicSize;
        width = height * camera.aspect;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 screenPos = camera.WorldToScreenPoint(playerTrans.position);
        //Debug.Log("target is " + screenPos.x + " pixels from the left");
        if(screenPos.x < 0)
        {
            transform.position -= new Vector3(width, 0, 0);
        }
        else if(screenPos.x > camera.pixelWidth)
        {
            transform.position += new Vector3(width, 0, 0);
        }
        if(screenPos.y < 0)
        {
            transform.position -= new Vector3(0, height, 0);
        }
        else if(screenPos.y > camera.pixelHeight)
        {
            transform.position += new Vector3(0, height, 0);
        }
	}
}
