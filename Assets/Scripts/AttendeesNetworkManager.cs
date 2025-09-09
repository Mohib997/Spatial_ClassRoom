using SpatialSys.UnitySDK;
using System;
using System.Linq;
using UnityEngine;

public class AttendeesNetworkManager : MonoBehaviour
{
    public const byte EVENT_ID_All = 0;
    public const byte EVENT_ID_OTHERS = 1;
    public const byte EVENT_ID_Specific = 2;

    //[SerializeField] private PostManager _postManager;

    public event Action<int, string> OnRecieveAttendeeNetworkEventAll;
    public event Action<int, string> OnRecieveAttendeeNetworkEventOthers;
    public event Action<int, string> OnRecieveAttendeeNetworkEventSpecific;

    private void OnEnable()
    {
        SpatialBridge.networkingService.remoteEvents.onEvent += HandleNetworkEvent;
    }
    private void OnDisable()
    {
        SpatialBridge.networkingService.remoteEvents.onEvent -= HandleNetworkEvent;
    }

    private void HandleNetworkEvent(NetworkingRemoteEventArgs args)
    {
        int actorNumber = args.senderActor;
        // Handle the network event received from the server
        if (args.eventID != EVENT_ID_All && args.eventID != EVENT_ID_OTHERS && args.eventID != EVENT_ID_Specific)
        {
            Debug.Log($"Received unknown network event ID: {args.eventID}");
            return;
        }
        if (args.eventID == EVENT_ID_All)
        {
            OnRecieveAttendeeNetworkEventAll?.Invoke(actorNumber, args.eventArgs[0].ToString());
        }
        else if (args.eventID == EVENT_ID_OTHERS)
        {
            OnRecieveAttendeeNetworkEventOthers?.Invoke(args.senderActor, args.eventArgs[0].ToString());
        }
        else if (args.eventID == EVENT_ID_Specific)
        {
            OnRecieveAttendeeNetworkEventSpecific?.Invoke(args.senderActor, args.eventArgs[0].ToString());
        }
    }

    public void SendEventToAllPlayers(string msg)
    {
        // Send a network event to all players
        SpatialBridge.networkingService.remoteEvents.RaiseEventAll(EVENT_ID_All, msg);
    }

    public void SendEventToOthers(string msg)
    {
        // Send a network event to all players except the sender
        SpatialBridge.networkingService.remoteEvents.RaiseEventOthers(EVENT_ID_OTHERS, msg);
    }

    public void SendEventToSpecificPlayers(int[] targetActors, object[] msg)
    {
        // Send a network event to specific players
        SpatialBridge.networkingService.remoteEvents.RaiseEvent(targetActors, EVENT_ID_Specific, msg);
    }

    public void CreateAttendeesEntriesForNewActorJoined()
    {
        var actors = SpatialBridge.actorService.actors;
        var attendeeEntries = FindObjectsOfType<AttendeesEntry>(true);
        if (actors.Count > 1)
        {
            var targetActor = new int[] { actors[actors.Keys.Last()].actorNumber };
            for (int i = 0; i < attendeeEntries.Length; i++)
            {
                string data = $"{attendeeEntries[i].ActorNumber}#{attendeeEntries[i].RoomNumber}";
                var obj = new object[] { data };
                SendEventToSpecificPlayers(targetActor, obj);
            }
        }
    }

    public void OnNotifyToRoomJoinToOthers(int actorNum, string roomNumber)
    {
        // true is for when actor joined 
        var actors = SpatialBridge.actorService.actors;
        if (actors.Count <= 1)
        {
            return;
        }
        else
        {
            string attendeeinfo = $"{actorNum}#{roomNumber}#true";
            SendEventToOthers(attendeeinfo);
        }
    }

    public void OnNotifyLeftRoomToOthers(string roomNum)
    {
        string attendeeinfo = $"{SpatialBridge.actorService.localActorNumber}#{roomNum}#false";
        SendEventToOthers(attendeeinfo);
    }

    public void OnNotifyLeftServerToOthers(int actorNum)
    {
        var actors = SpatialBridge.actorService.actors;
        var attendeeEntries = FindObjectsOfType<AttendeesEntry>(true);
        if (actors.Count > 0)
        {
            for (int i = 0; i < attendeeEntries.Length; i++)
            {
                if (attendeeEntries[i].ActorNumber == actorNum)
                {
                    string attendeeinfo = $"{actorNum}#{attendeeEntries[i].RoomNumber}#false";
                    SendEventToAllPlayers(attendeeinfo);
                    break;
                }
            }
        }
    }
}