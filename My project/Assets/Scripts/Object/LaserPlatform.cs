using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPlatform : MonoBehaviour
{
    public Transform laserShoot;
    public Transform laserTarget;
    public float laserCheckRate;
    public LayerMask playerLayer;
    public GameObject trap;
    public float laserDistance;
    public float gravitymultiplier;

    private Rigidbody trapRb;
    private float lastLaserCheckTime;
    private List<GameObject> hitplayers = new List<GameObject>();

    private void Awake()
    {
        trapRb = trap.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Time.time - lastLaserCheckTime > laserCheckRate)
        {
            lastLaserCheckTime = Time.time;
            CheckPlayerCollision();
        }
    }

    private void CheckPlayerCollision()
    {
        Vector3 LaserDirection = (laserTarget.position - laserShoot.position).normalized;
        laserDistance = Vector3.Distance(laserTarget.position, laserShoot.position);

        RaycastHit hit;
        if (Physics.Raycast(laserShoot.position, LaserDirection, out hit, laserDistance, playerLayer))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                if (trap != null)
                {
                    DropTrap();
                }
            }
        }
        else
        {
            hitplayers.Clear();
        }
    }

    public void DropTrap()
    {
        if (trapRb != null)
        {
            trapRb.isKinematic = false;
        }
    }
}
