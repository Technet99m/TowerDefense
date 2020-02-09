﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAim : MonoBehaviour
{
    public Targeting aimType;
    TowerDataHolder dataHolder;
    [SerializeField] Transform target;
    [SerializeField] GameObject bull;
    public bool smart;
    private void Start()
    {
        dataHolder = GetComponent<TowerDataHolder>();
        InvokeRepeating(nameof(CheckForTarget), 1, 1 / 15f);
        StartCoroutine(Reload());
    }
    private void FixedUpdate()
    {
        LookAtTarget();
        
    }
    void LookAtTarget()
    {
        if (target)
        {
                transform.right = (target.position + target.GetComponent<EnemyMovementComponent>().direction * target.GetComponent<EnemyDataHolder>().data.Speed *
                    Vector3.Distance(target.position, transform.GetChild(0).position) *(smart? 1f : 0.5f) / dataHolder.data.BulletSpeed) - transform.position;
        }
    }
    void CheckForTarget()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, dataHolder.data.Range);
        List<Collider2D> list = new List<Collider2D>();
        foreach (Collider2D col in colls)
        {
            if(col.CompareTag("Enemy"))
                list.Add(col);
        }
        if (list.Count == 0)
        {
            target = null;
            return;
        }
        switch (aimType)
        {
            case Targeting.Weakest:
                list.Sort((x, y) => x.GetComponent<EnemyHealth>().health.CompareTo(y.GetComponent<EnemyHealth>().health));
                target = list[0].transform;
                break;
            case Targeting.Strongest:
                list.Sort((x, y) => y.GetComponent<EnemyHealth>().health.CompareTo(x.GetComponent<EnemyHealth>().health));
                target = list[0].transform;
                break;
            case Targeting.Nearest:
                list.Sort((x, y) => y.GetComponent<EnemyMovementComponent>().distance.CompareTo(x.GetComponent<EnemyMovementComponent>().distance));
                target = list[0].transform;
                break;
        }
        LookAtTarget();
    }

    IEnumerator Reload()
    {
        while(true)
        {
            if (target != null)
            {
                Shoot();
                yield return new WaitForSeconds(dataHolder.data.Reload);
            }
            else
                yield return null;
        }
    }
    void Shoot()
    {
        var bullet = Instantiate(bull,transform.GetChild(0).position,Quaternion.identity, transform).GetComponent<BulletController>();
        bullet.Initialize(dataHolder.data.Damage, dataHolder.data.BulletSpeed, dataHolder.data.Shape,dataHolder.data.Color);
    }
}

public enum Targeting
{
    Weakest,
    Strongest,
    Nearest
}
