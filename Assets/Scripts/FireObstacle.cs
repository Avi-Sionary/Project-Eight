using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObstacle : MonoBehaviour {

    public GameObject fire;

	// Use this for initialization
	void Start () {

	}

    IEnumerator Fire() {
        // just a "stopwatch" for debugging purposes.
        print((int)Time.time);
        yield return new WaitForSeconds(5);

        //for every other 5 seconds, clone the FireObstacle
        if ((int)Time.time % 5 == 0 && (int)Time.time % 2 != 0) 
        {
            if (GameObject.FindGameObjectsWithTag("Fire").Length < 10)
            {
                GameObject.Instantiate(fire, transform.position, transform.rotation); 
            }
        }
        //for every other other 5 seconds(idfk the proper term for it; shut up), destroy the clones
        else if ((int)Time.time % 5 == 0 && (int)Time.time % 2 == 0)
        {
            GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire");
            foreach (GameObject i in fires)
            {
                if (i.name == "FireObstacle(Clone)")
                {
                    GameObject.Destroy(i);
                }
            }
        } 
    }

    //if the player hits the fire, he/she is destroyed. I'm assuming the Destroy method will be replaced by whatever's written in the health script.
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Player")
        {
            Destroy(col.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        StartCoroutine(Fire());
    }
}
