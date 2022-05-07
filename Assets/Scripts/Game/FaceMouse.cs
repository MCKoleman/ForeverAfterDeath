using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FaceMouse : MonoBehaviour
{
    private bool rotatingActive = true;
    [SerializeField] private GameObject objectToRotate;
    [Range(0f, 100f)]
    [SerializeField] private float rotateSpeed;


    void Update()
    {
        if (rotatingActive)
        {
            RotateToFaceMouse();
        }
    }
    void RotateToFaceMouse()
    {
        Vector2 direction;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        direction.x = mousePos.x - objectToRotate.transform.position.x;
        direction.y = mousePos.y - objectToRotate.transform.position.y;
        Vector3 lerpVar = Vector3.Lerp(transform.up, direction, Time.deltaTime * rotateSpeed);
        objectToRotate.transform.up = lerpVar;
    }

    #region Toggling Mouse Facing
    public void TurnOffMouseFacing()
    {
        rotatingActive = false;
    }

    public void TurnOffMouseFacing(float time)
    {
        StartCoroutine(TurnOffMouseFacingTimer(time));
    }

    public void TurnOnMouseFacing()
    {
        rotatingActive = true;
    }

    IEnumerator TurnOffMouseFacingTimer(float time)
    {
        rotatingActive = false;
        yield return new WaitForSeconds(time);
        TurnOnMouseFacing();
    }
    #endregion

}