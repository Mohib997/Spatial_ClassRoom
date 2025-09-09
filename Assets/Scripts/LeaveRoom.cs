using System;
using UnityEngine;

public class LeaveRoom : MonoBehaviour, IUIRaycastable
{
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private Room _room;
    public event Action<LeaveRoom> OnRequestRoomLeft;

    public Transform ExitPoint
    {
        get => _exitPoint;
        set => _exitPoint = value;
    }
    public Room Room => _room;

    public void OnRaycastHit()
    {
        OnRequestRoomLeft?.Invoke(this);
    }
}
