using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentNode : MonoBehaviour
{
    public Vector2Int pos;

    private void Start()
    {
        pos = new Vector2Int(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
        GenManager.Instance.AddContentNode(this);
    }
}
