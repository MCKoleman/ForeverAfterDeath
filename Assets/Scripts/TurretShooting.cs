using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootIntervalMin;
    [SerializeField] private float shootIntervalMax;

    void Start()
    {
        StartCoroutine(Shoot());        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(shootIntervalMin, shootIntervalMax));
        Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        StartCoroutine(Shoot());
    }


}
