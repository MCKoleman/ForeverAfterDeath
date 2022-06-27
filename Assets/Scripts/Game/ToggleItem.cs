using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleItem : MonoBehaviour
{
    public GameObject objectToToggle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoToggle()
    {
        if(objectToToggle.activeSelf)
        {
            objectToToggle.SetActive(false);
        }
        else
        {
            objectToToggle.SetActive(true);
        }
       
    }
}
