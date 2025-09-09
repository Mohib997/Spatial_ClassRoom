using SpatialSys.UnitySDK;
using System;
using System.Linq;
using UnityEngine;

public class PostNetworkManager : MonoBehaviour
{
    private bool isInitialized = false;

    public const byte EVENT_ID_All = 0;
    public const byte EVENT_ID_OTHERS = 1;
    public const byte EVENT_ID_Specific = 2;

    public event Action<int, string> OnRecievePostNetworkEventAll;
    public event Action<int, string> OnRecievePostNetworkEventOthers;
    public event Action<int, string> OnRecievePostNetworkEventSpecific;

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
            string msg = args.eventArgs[0].ToString();
            OnRecievePostNetworkEventAll?.Invoke(actorNumber, msg);
        }
        else if (args.eventID == EVENT_ID_OTHERS)
        {
            OnRecievePostNetworkEventOthers?.Invoke(args.senderActor, args.eventArgs[0].ToString());
        }
        else if (args.eventID == EVENT_ID_Specific)
        {
            OnRecievePostNetworkEventSpecific?.Invoke(args.senderActor, args.eventArgs[0].ToString());
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

    public void CreatePostViewEntriesForNewActorJoined(int actorNum)
    {
        var actors = SpatialBridge.actorService.actors;
        Room[] rooms = FindObjectsOfType<Room>(true);
        PostViewerEntry[] postViewerEntries = FindObjectsOfType<PostViewerEntry>(true);
        if (actors.Count > 1)
        {
            var targetActor = new int[] { actorNum };
            foreach (PostViewerEntry entry in rooms[0].SpawnParentPostVeiw.GetComponentsInChildren<PostViewerEntry>())
            {
                string postData = $"{entry.PostTitle}#{entry.PostContent}#{entry.RoomNumber}#{entry.PostAuthor}";
                object[] obj = new object[] { postData }; // Wrap the string in an object array
                SendEventToSpecificPlayers(targetActor, obj);
            }
        }
    }
}