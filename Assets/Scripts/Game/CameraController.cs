using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    protected Transform cameraPivot;
    [SerializeField]
    protected CameraShake shake;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private float lockedFollowSpeed = 5.0f;
    [SerializeField]
    private LevelRoom room;
    [SerializeField]
    private Vector3 roomPos;
    [SerializeField]
    private Vector2 roomSize;
    [SerializeField]
    private Vector2 defaultRoomSize;

    private void Start()
    {
        shake = Camera.main.GetComponentInParent<CameraShake>();
    }

    private void Update()
    {
        // Follow with camera to destination
        if (MathUtils.AlmostZero(this.transform.position - destination, 2))
            return;

        Vector3 tempVec = Vector3.Lerp(this.transform.position, destination, lockedFollowSpeed * Time.deltaTime);
        this.transform.position = new Vector3(tempVec.x, tempVec.y, transform.position.z);
    }

    // Set the room to follow with the camera
    public void SetRoom(LevelRoom newRoom)
    {
        // Don't set the room to follow to null
        if (newRoom == null)
            return;

        room = newRoom;
        roomSize = room.GetSize();
        roomPos = room.GetPosition();
        //offset.z = OFFSET_X_MOD * roomSize.x + OFFSET_B_MOD;

        destination = new Vector3(roomPos.x + offset.x, roomPos.y + offset.y, offset.z);
    }

    // Returns the destination clamped to the bounds that the camera can move to
    private Vector3 ClampDestinationToRoomBounds(Vector3 destination)
    {
        return new Vector3(
            Mathf.Clamp(destination.x,
                roomPos.x - roomSize.x * 0.5f - defaultRoomSize.x * 0.5f + offset.x,
                roomPos.x + roomSize.x * 0.5f + defaultRoomSize.x * 0.5f + offset.x),
            Mathf.Clamp(destination.y,
                roomPos.y - roomSize.y * 0.5f - defaultRoomSize.y * 0.5f,
                roomPos.y + roomSize.y * 0.5f + defaultRoomSize.y * 0.5f),
            offset.z);
    }

    // Shakes the camera
    public void Shake(float duration = 0.5f, float magnitude = 0.7f, float damping = 1.0f)
    {
        shake.TriggerShake(duration, magnitude, damping);
    }
}
