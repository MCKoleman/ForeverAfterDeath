using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float speed;
    [SerializeField] private float maxAttackRange = 15.0f;

    void Update()
    {
        if (InputManager.Instance.GetPlayer() != null)
            if (GameManager.Instance.IsGameActive && !UIManager.Instance.IsPaused()
                && Vector3.Distance(this.transform.position, InputManager.Instance.GetPlayer().transform.position) < maxAttackRange)
                MoveForward();
    }

    private void MoveForward()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }
}
