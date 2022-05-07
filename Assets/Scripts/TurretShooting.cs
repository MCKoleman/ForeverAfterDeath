using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootIntervalMin;
    [SerializeField] private float shootIntervalMax;
    public bool isShootingActive = true;

    void Start()
    {
        StartCoroutine(Shoot());        
    } 

    IEnumerator Shoot()
    {
        while(isShootingActive)
        {
            yield return new WaitUntil(() => (GameManager.Instance.IsGameActive && !UIManager.Instance.IsPaused()));
            yield return new WaitForSeconds(Random.Range(shootIntervalMin, shootIntervalMax));
            Instantiate(bullet, shootPoint.position, shootPoint.rotation, PrefabManager.Instance.projectileHolder);
        }
    }
}
