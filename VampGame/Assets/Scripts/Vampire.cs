using UnityEngine;
using System.Collections;

public class Vampire : FSM {


public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
        Hide
    }

    //Current state that the NPC is reaching
    public FSMState curState;

    //Speed of the tank
    private float curSpeed;

    //Tank Rotation Speed
    private float curRotSpeed;

    //Bullet
    public GameObject Bullet = null;

    //Whether the NPC is destroyed or not
    private bool bDead;
    private int health;


    //Initialize the Finite state machine for the NPC tank
    protected override void Initialize()
    {
        curState = FSMState.Patrol;
        curSpeed = 2;
        curRotSpeed = 2.0f;
        bDead = false;
        elapsedTime = 0.0f;
        health = 100;

        //Get the list of points
        pointList = GameObject.FindGameObjectsWithTag("WandarPoint");

        //Set Random destination point first
        FindNextPoint();

        //Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");
    }

    //Update each frame
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
            case FSMState.Hide: UpdateHideState(); break;
        }

        //Update the time
        elapsedTime += Time.deltaTime;

        //Go to dead state is no health left
        if (health <= 0)
            curState = FSMState.Dead;
    }

    /// <summary>
    /// Patrol state
    /// </summary>
    protected void UpdatePatrolState()
    {
        //Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= 1)
        {
            print("Reached to the destination point\ncalculating the next point");
            FindNextPoint();
        }
        //Check the distance with player tank
        //When the distance is near, transition to chase state
        /*        else if (Vector3.Distance(transform.position, playerTransform.position) <= 300.0f)
        {
            print("Switch to Chase Position");
            curState = FSMState.Chase;
        }
        */
        //Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
        if (Input.GetKey(KeyCode.E))
        {
            curState = FSMState.Hide;
        }
    }

    /// <summary>
    /// Chase state
    /// </summary>
    protected void UpdateChaseState()
    {
        //Set the target position as the player position
        destPos = playerTransform.position;

        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= 200.0f)
        {
            curState = FSMState.Attack;
        }
        //Go back to patrol is it become too far
        else if (dist >= 300.0f)
        {
            curState = FSMState.Patrol;
        }

        //Go Forward
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    /// <summary>
    /// Attack state
    /// </summary>
    protected void UpdateAttackState()
    {
        //Set the target position as the player position
        destPos = playerTransform.position;

        //Check the distance with the player
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist >= 200.0f && dist < 300.0f)
        {
            //Rotate to the target point
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //Go Forward
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);

            curState = FSMState.Attack;
        }
        //Transition to patrol is the player become too far
        else if (dist >= 300.0f)
        {
            curState = FSMState.Patrol;
        }
    }

    /// <summary>
    /// Dead state
    /// </summary>
    protected void UpdateDeadState()
    {
        
        if (!bDead)
        {
            bDead = true;
        
        }
    }
    /// <summary>
    /// Check the collision with the bullet
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
            health -= 10;
    }

    /// <summary>
    /// Find the next semi-random patrol point
    /// </summary>
    protected void FindNextPoint()
    {
        print("Finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        float rndRadius = 10.0f;

        Vector3 rndPosition = Vector3.zero;
        destPos = pointList[rndIndex].transform.position + rndPosition;

        //Check Range
        //Prevent to decide the random point as the same as before
        if (IsInCurrentRange(destPos))
        {
            rndPosition = new Vector3(Random.Range(-rndRadius, rndRadius), 0.0f, Random.Range(-rndRadius, rndRadius));
            destPos = pointList[rndIndex].transform.position + rndPosition;
        }
    }

    /// <summary>
    /// Check whether the next random position is the same as current tank position
    /// </summary>
    /// <param name="pos">position to check</param>
    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);

        if (xPos <= 50 && zPos <= 50)
            return true;

        return false;
    }

    protected void UpdateHideState()
    {
        GameObject hidePoint = GameObject.FindGameObjectWithTag("Cave");
            destPos = hidePoint.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //Go Forward
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
        if (Input.GetKey(KeyCode.E))
        {
            curState = FSMState.Patrol;
        }
    }

}
