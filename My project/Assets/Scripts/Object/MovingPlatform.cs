using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed;
    public float waitTime;


    private int curWayPointIndex;
    private Transform player;

    private void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            StartCoroutine(MovePlatform());
        }

    }
    
    private IEnumerator MovePlatform()
    {
        if (waypoints.Length == 0) yield break;

        while (true)
        {
            Transform targetWaypoint = waypoints[curWayPointIndex];
            float distance = Vector3.Distance(transform.position, targetWaypoint.position);

            if (distance <= 0.01f)
            {
                yield return new WaitForSeconds(waitTime);

                curWayPointIndex = (curWayPointIndex + 1) % waypoints.Length;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            player.SetParent(transform); 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.SetParent(null); 
            player = null;
        }
    }

}
