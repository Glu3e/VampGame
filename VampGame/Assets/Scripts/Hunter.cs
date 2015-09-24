using UnityEngine;
using System.Collections;

public class Hunter : MonoBehaviour {
    public int health = 100;
    GameObject[] object1;

    // Usepublic  this for initialization
    void Start () {
       object1 = GameObject.FindGameObjectsWithTag("Vampire");
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < object1.Length; i++)
        {
            if (Vector3.Distance(object1[i].transform.position, this.transform.position) <= 3)
            {
                health -= 10;
            }
        }
    }
}
