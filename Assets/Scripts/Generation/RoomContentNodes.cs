using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoomContentNodes
{
    public List<ContentNode> wallContentNodes;
    public List<ContentNode> cornerContentNodes;
    public List<ContentNode> centerContentNodes;
    public List<ContentNode> edgeContentNodes;
    public List<ContentNode> contentNodes;

    public void AddContentNode(ContentNode node)
    {
        switch(node.nodePlace)
        {
            case GlobalVars.NodePlace.WALL:
                wallContentNodes.Add(node);
                break;
            case GlobalVars.NodePlace.CENTER:
                centerContentNodes.Add(node);
                break;
            case GlobalVars.NodePlace.CORNER:
                cornerContentNodes.Add(node);
                break;
            case GlobalVars.NodePlace.EDGE:
                edgeContentNodes.Add(node);
                break;
            case GlobalVars.NodePlace.NORMAL:
                contentNodes.Add(node);
                break;
        }
    }

    public RoomContentNodes(ContentNode node)
    {
        wallContentNodes = new List<ContentNode>();
        cornerContentNodes = new List<ContentNode>();
        centerContentNodes = new List<ContentNode>();
        edgeContentNodes = new List<ContentNode>();
        contentNodes = new List<ContentNode>();

        AddContentNode(node);
    }
}
