using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObstacle : MonoBehaviour {

    public GameObject fire;
    public int height;
    public bool isOdd;


	// Use this for initialization
	void Start () {

	}

    IEnumerator Fire()
    {
        // just a "stopwatch" for debugging purposes.
        print((int)Time.time);
        yield return new WaitForSeconds(1);
        int count = 0;
        GameObject[] fires = GameObject.FindGameObjectsWithTag(fire.tag);
        foreach (GameObject i in fires)
        {
            if (i.name == fire.name + "(Clone)")
            {
                count++;
            }
        }
        if (fire.GetComponent<Renderer>().isVisible)
        {
            //depending on isOdd's value, clone the FireObstacle
            if ((isOdd == false && (int)Time.time % 2 == 0) || (isOdd == true && (int)Time.time % 2 != 0))
            {
                if (count < height)
                {
                    Vector3 y = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    for (int i = 1; i <= height; i++)
                    {
                        y.y = y.y + (float)0.1;
                        GameObject.Instantiate(fire, y, transform.rotation);
                        AudioSource flame = GetComponent<AudioSource>();
                        flame.Play();
                    }
                }
            }
            //destroy the clones if the parity of the time is contradictory to isOdd
            else
            {
                foreach (GameObject i in fires)
                {
                    if (i.name == fire.name + "(Clone)")
                    {
                        GameObject.Destroy(i);
                    }
                }
            }
        }
        else
        {
            foreach (GameObject i in fires)
            {
                if (i.name == fire.name + "(Clone)")
                {
                    GameObject.Destroy(i);
                }
            }
        }
    }

    //if the player hits the fire, he/she is destroyed. I'm assuming the Destroy method will be replaced by whatever's written in the health script.
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().playerHealth = 0;
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        StartCoroutine(Fire());
    }
}
