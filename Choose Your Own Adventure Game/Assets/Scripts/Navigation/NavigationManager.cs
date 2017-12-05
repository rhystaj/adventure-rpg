using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Managers player movement by mouse input between points on the map.
 */ 
public class NavigationManager : MonoBehaviour {

    [SerializeField] Piece playerPiece;
    public MapNode startingLocation;

    private MapNode currentNode;

    private void Start()
    {
        currentNode = startingLocation;
    }

    private void OnEnable()
    {
        MapNode.NodeSelected += MovePlayerPieceToNode;
    }

    private void OnDisable()
    {
        MapNode.NodeSelected -= MovePlayerPieceToNode;
    }

    private void MovePlayerPieceToNode(MapNode node)
    {

        //If the piece is alreay moving, or the path to the given node isn't traversable, don't do anything.
        if (playerPiece.isMoving() || !currentNode.GetPathTo(node).traversable) return; 

        //Find the required node and move the piece along it, if it exists.
        if (node == currentNode) return;
        MovementPath path = currentNode.GetPathTo(node);
        if (path == null) return;
        StartCoroutine(playerPiece.MoveAlongPath(path));
        currentNode = node;

    }
}
