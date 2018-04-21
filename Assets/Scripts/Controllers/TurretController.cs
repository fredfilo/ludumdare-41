﻿using Controllers;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    // Properties
    // ------------------------------------------------------

    [SerializeField] private bool isActive;
    [SerializeField] private GameObject bulletStart;
    [SerializeField] private GameObject bulletModel;
    [SerializeField] private float fireInterval = 1.0f;
    [SerializeField] private float direction = 1.0f;

    private float lastFire;
    
    // Private methods
    // ------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (Time.time - lastFire > fireInterval)
        {
            lastFire = Time.time;
            Fire();
        }
    }

    private void Fire()
    {
        if (bulletModel == null || bulletStart == null)
        {
            return;
        }

        GameObject bullet = Instantiate(bulletModel, transform);
        bullet.transform.position = bulletStart.transform.position;

        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.direction = new Vector2(direction, 0);
    }
}