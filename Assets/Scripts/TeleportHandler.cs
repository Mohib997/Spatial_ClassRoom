using SpatialSys.UnitySDK;
using System;
using UnityEngine;

public class TeleportHandler : MonoBehaviour
{
    [Header("Join")]
    [SerializeField] private JoinRoom _joinButton1;
    [SerializeField] private JoinRoom _joinButton2;
    [SerializeField] private JoinRoom _joinButton3;

    [Header("Leave")]
    [SerializeField] private LeaveRoom leaveButton1;
    [SerializeField] private LeaveRoom leaveButton2;
    [SerializeField] private LeaveRoom leaveButton3;

    public event Action<Room> OnRoomEntered;
    public event Action<Room> OnRoomLeft;


    private void OnEnable()
    {
        _joinButton1.OnRequestRoomEntered += HandleRoomEntered;
        _joinButton2.OnRequestRoomEntered += HandleRoomEntered;
        _joinButton3.OnRequestRoomEntered += HandleRoomEntered;

        leaveButton1.OnRequestRoomLeft += HandleRoomLeft;
        leaveButton2.OnRequestRoomLeft += HandleRoomLeft;
        leaveButton3.OnRequestRoomLeft += HandleRoomLeft;
    }
    private void OnDisable()
    {
        _joinButton1.OnRequestRoomEntered -= HandleRoomEntered;
        _joinButton2.OnRequestRoomEntered -= HandleRoomEntered;
        _joinButton3.OnRequestRoomEntered -= HandleRoomEntered;

        leaveButton1.OnRequestRoomLeft -= HandleRoomLeft;
        leaveButton2.OnRequestRoomLeft -= HandleRoomLeft;
        leaveButton3.OnRequestRoomLeft -= HandleRoomLeft;
    }

    private void HandleRoomEntered(JoinRoom room)
    {
        if (room == null)
        {
            Debug.LogError("Room is null");
            return;
        }
        OnRoomEntered?.Invoke(room.Room);
        IAvatar localActor = SpatialBridge.actorService.localActor.avatar;

        localActor.SetPositionRotation(room.EntryPoint.position, room.EntryPoint.rotation);
    }

    private void HandleRoomLeft(LeaveRoom room)
    {
        if (room == null)
        {
            Debug.LogError("Room is null");
            return;
        }
        OnRoomLeft?.Invoke(room.Room);

        IAvatar localActor = SpatialBridge.actorService.localActor.avatar;
        localActor.SetPositionRotation(room.ExitPoint.localPosition, room.ExitPoint.localRotation);
    }
}