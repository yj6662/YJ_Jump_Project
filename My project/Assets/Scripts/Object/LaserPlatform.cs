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
    public LineRenderer laserLine;

    private Rigidbody trapRb;
    private float lastLaserCheckTime;
    private List<GameObject> hitplayers = new List<GameObject>();

    private void Awake()
    {
        trapRb = trap.GetComponent<Rigidbody>();
        laserLine = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        laserLine.startWidth = 0.01f;
        laserLine.endWidth = 0.01f;
        laserLine.startColor = Color.red;
        laserLine.endColor = Color.red;
    }

    private void Update()
    {
        if (Time.time - lastLaserCheckTime > laserCheckRate)
        {
            lastLaserCheckTime = Time.time;
            CheckPlayerCollision();
        }
        laserLine.SetPosition(0, laserShoot.position);
        laserLine.SetPosition(1, laserTarget.position);
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
