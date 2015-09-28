using UnityEngine;
using System.Collections;

public class Hunter : MonoBehaviour {
    public int health = 100;
    public GameObject[] vampires;

    // Usepublic  this for initialization
    void Start () {
       vampires = GameObject.FindGameObjectsWithTag("Vampire");
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < vampires.Length; i++)
        {
            if (Vector3.Distance(vampires[i].transform.position, this.transform.position) <= 10.0f)
            {
                health -= 1;
                if(health <= 0)
                {
                    Destroy(gameObject);
                    Application.Quit();
                }
            }
        }
    }
}
