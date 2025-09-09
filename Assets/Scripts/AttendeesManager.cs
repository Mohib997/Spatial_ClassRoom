using UnityEngine;
using SpatialSys.UnitySDK;
using UnityEngine.UI;
using System.Collections.Generic;

public class AttendeesManager : MonoBehaviour
{
    [Header("AssetReference")]
    [SerializeField] private AttendeesEntry _attendeesEntry;
    [SerializeField] private ScrollRect[] _scrollRectAttendeesContainer;
    [SerializeField] private AttendeesNetworkManager _attendeesNetworkManager;
    [SerializeField] private PostNetworkManager _postNetworkManager;
    [SerializeField] private TeleportHandler _teleportHandler;

    private List<string> _attendeesActorNumberEntries = new List<string>();
    private Dictionary<int, IActor> actors = new Dictionary<int, IActor>();

    private void Awake()
    {
        _teleportHandler = FindObjectOfType<TeleportHandler>(true);
        _attendeesNetworkManager = FindObjectOfType<AttendeesNetworkManager>(true);
        _postNetworkManager = FindObjectOfType<PostNetworkManager>(true);
    }

    private void OnEnable()
    {
        SpatialBridge.actorService.onActorJoined += OnActorJoined;
        SpatialBridge.actorService.onActorLeft += OnActorLeft;
        _teleportHandler.OnRoomEntered += OnActorRoomEntered;
        _teleportHandler.OnRoomLeft += OnActorRoomLeft;
        _attendeesNetworkManager.OnRecieveAttendeeNetworkEventSpecific += OnRecieveNetworkEventSpecific;
        _attendeesNetworkManager.OnRecieveAttendeeNetworkEventOthers += OnRecieveNetworkEventOthers;
        _attendeesNetworkManager.OnRecieveAttendeeNetworkEventAll += OnRecieveNetworkEventALL;
    }

    private void OnDisable()
    {
        SpatialBridge.actorService.onActorJoined -= OnActorJoined;
        SpatialBridge.actorService.onActorLeft -= OnActorLeft;
        _teleportHandler.OnRoomEntered -= OnActorRoomEntered;
        _teleportHandler.OnRoomLeft -= OnActorRoomLeft;
        _attendeesNetworkManager.OnRecieveAttendeeNetworkEventSpecific -= OnRecieveNetworkEventSpecific;
        _attendeesNetworkManager.OnRecieveAttendeeNetworkEventOthers -= OnRecieveNetworkEventOthers;
        _attendeesNetworkManager.OnRecieveAttendeeNetworkEventAll -= OnRecieveNetworkEventALL;
    }

    private void OnActorJoined(ActorJoinedEventArgs args)
    {

        IActor joinedActor = SpatialBridge.actorService.actors[args.actorNumber];
        actors.Add(args.actorNumber, joinedActor);
        SpatialBridge.coreGUIService.DisplayToastMessage(joinedActor.displayName + " joined the space");

        _attendeesNetworkManager.CreateAttendeesEntriesForNewActorJoined();
        _postNetworkManager.CreatePostViewEntriesForNewActorJoined(args.actorNumber);

        Debug.Log($"BESTOO-> OnActorJoined Actor Joined: {joinedActor?.actorNumber} - {joinedActor?.displayName} - LOCALDICTCOUNT: {actors.Count}");
    }

    private void OnActorLeft(ActorLeftEventArgs args)
    {
        Debug.Log($"BESTOO-> Actor Left" + args.actorNumber);
        _attendeesNetworkManager.OnNotifyLeftServerToOthers(args.actorNumber);
    }

    private void OnRecieveNetworkEventSpecific(int actorNum, string attendeeInfo)
    {
        string[] attendeeData = attendeeInfo.Split('#');
        if (attendeeData.Length != 2)
        {
            Debug.Log("Invalid post data received.");
            return;
        }
        string actorNumber = attendeeData[0];
        string roomNumber = attendeeData[1];

        foreach (var room in FindObjectsOfType<Room>(true))
        {
            if (room.RoomNumber == roomNumber)
            {
                IActor actor = SpatialBridge.actorService.actors[int.Parse(actorNumber)];
                for (int i = 0; i < _attendeesActorNumberEntries.Count; i++)
                {
                    if (_attendeesActorNumberEntries[i] == actorNumber)
                    {
                        return;
                    }
                }

                room.OnActorJoinedInRoom(SpatialBridge.actorService.actors[int.Parse(actorNumber)]);
                _attendeesActorNumberEntries.Add(actorNumber);
                break;
            }
        }
    }
    private void OnRecieveNetworkEventOthers(int actorNum, string msg)
    {
        string[] attendeeData = msg.Split('#');
        if (attendeeData.Length != 3)
        {
            Debug.Log("Invalid post data received.");
            return;
        }
        string actorNumber = attendeeData[0];
        string roomNumber = attendeeData[1];
        string state = attendeeData[2];

        foreach (var room in FindObjectsOfType<Room>(true))
        {
            if (room.RoomNumber == roomNumber)
            {
                int actorNumInt = int.Parse(actorNumber);
                IActor actor = actors[actorNumInt];
                if (state.Equals("true"))
                {
                    room.OnActorJoinedInRoom(actor);
                }
                else
                {
                    room.OnActorLeftRoom(actor);
                }
                break;
            }
        }
    }

    private void OnRecieveNetworkEventALL(int actorNum, string msg)
    {
        string[] attendeeData = msg.Split('#');
        if (attendeeData.Length != 3)
        {
            Debug.Log("Invalid post data received.");
            return;
        }
        string actorNumber = attendeeData[0];
        string roomNumber = attendeeData[1];
        string state = attendeeData[2];
        foreach (var room in FindObjectsOfType<Room>(true))
        {
            if (room.RoomNumber == roomNumber)
            {
                int actorNumInt = int.Parse(actorNumber);

                if (actors.ContainsKey(actorNumInt))
                {
                    IActor actor = actors[actorNumInt];
                    actors.Remove(actorNumInt);
                    room.OnActorLeftRoom(actor);
                    break;
                }
            }
        }
    }

    private void OnActorRoomEntered(Room room)
    {
        if (room != null)
        {
            room.OnActorJoinedInRoom(SpatialBridge.actorService.actors[SpatialBridge.actorService.localActor.actorNumber]);
            _attendeesNetworkManager.OnNotifyToRoomJoinToOthers(SpatialBridge.actorService.localActor.actorNumber, room.RoomNumber);
        }
    }

    private void OnActorRoomLeft(Room room)
    {
        if (room != null)
        {
            AttendeesEntry attendeesEntry = room.OnActorLeftRoom(SpatialBridge.actorService.actors[SpatialBridge.actorService.localActorNumber]);
            _attendeesNetworkManager.OnNotifyLeftRoomToOthers(room.RoomNumber);
        }
    }
}