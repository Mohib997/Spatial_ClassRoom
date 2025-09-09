using SpatialSys.UnitySDK;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostManager : MonoBehaviour
{
    public static PostManager Instance { get; private set; }

    [SerializeField] private PostViewerEntry _postViewEntry;
    [SerializeField] private RectTransform _SpawnParentPostVeiw;
    [SerializeField] private TMP_InputField _postInputField;
    [SerializeField] private TMP_InputField _inpTitlePost;
    [SerializeField] private Button _btnSubmit;
    [SerializeField] private Button _btnWritepost;

    [SerializeField] private Room _room1;
    [SerializeField] private Room _room2;
    [SerializeField] private Room _room3;

    [SerializeField] private PostNetworkManager _networkManager;

    private void Awake()
    {
        if (!_networkManager)
        {
            _networkManager = FindObjectOfType<PostNetworkManager>(true);
        }
    }
    private void OnEnable()
    {
        _room1.OnPostSubmitted += OnSubmitPost;
        _room2.OnPostSubmitted += OnSubmitPost;
        _room3.OnPostSubmitted += OnSubmitPost;

        _networkManager.OnRecievePostNetworkEventOthers += OnRecieveNetworkEvent;
        _networkManager.OnRecievePostNetworkEventSpecific += OnRecieveNetworkEventSpecific;
    }
    private void OnDisable()
    {
        _room1.OnPostSubmitted -= OnSubmitPost;
        _room2.OnPostSubmitted -= OnSubmitPost;
        _room3.OnPostSubmitted -= OnSubmitPost;

        _networkManager.OnRecievePostNetworkEventOthers -= OnRecieveNetworkEvent;
        _networkManager.OnRecievePostNetworkEventSpecific -= OnRecieveNetworkEventSpecific;
    }

    private void OnRecieveNetworkEventSpecific(int actorNumber, string msg)
    {
        // Attendees Entry Instatiate
        string[] postData = msg.Split('#');
        if (postData.Length < 4)
        {
            Debug.Log("Invalid post data received.");
            return;
        }
        string title = postData[0];
        string content = postData[1];
        string roomNum = postData[2];
        string author = postData[3];


        if (SpatialBridge.actorService.actors.Keys.First() == actorNumber)
        {
            Room[] rooms = FindObjectsOfType<Room>(true);
            for (int i = 0; i < rooms.Length; i++)
            {
                PostViewerEntry entry = Instantiate(_postViewEntry, rooms[i].SpawnParentPostVeiw);
                entry.PostTitle = postData[0];
                entry.PostContent = postData[1];
                entry.Room = rooms[i];
                entry.RoomNumber = roomNum;
                entry.PostAuthor = author;
            }
        }

    }

    public void OnSubmitPost(Room room, string postInfo)
    {
        CreatePostViewEntries(postInfo, room);
        _networkManager.SendEventToOthers(postInfo);
    }

    private void OnRecieveNetworkEvent(int actorNumber, string msg)
    {
        string[] postData = msg.Split('#');
        if (postData.Length < 4)
        {
            Debug.LogWarning("Invalid post data received.");
            return;
        }
        string title = postData[0];
        string content = postData[1];
        string roomNumber = postData[2];
        string author = postData[3];

        Room[] rooms = FindObjectsOfType<Room>(true);
        for (int i = 0; i < rooms.Length; i++)
        {
            PostViewerEntry entry = Instantiate(_postViewEntry, rooms[i].SpawnParentPostVeiw);
            entry.PostTitle = title;
            entry.PostContent = content;
            entry.Room = rooms[i];
            entry.RoomNumber = roomNumber;
            entry.PostAuthor = author;
        }
    }

    private void CreatePostViewEntries(string postInfo, Room room = null)
    {
        Room[] rooms = FindObjectsOfType<Room>(true);
        string[] postData = postInfo.Split('#');
        if (postData.Length < 4)
        {
            Debug.LogWarning("Invalid post data received.");
            return;
        }
        for (int i = 0; i < rooms.Length; i++)
        {
            PostViewerEntry entry = Instantiate(_postViewEntry, rooms[i].SpawnParentPostVeiw);
            entry.PostTitle = postData[0];
            entry.PostContent = postData[1];
            entry.Room = rooms[i];
            entry.RoomNumber = postData[2];
            entry.PostAuthor = postData[3];
        }
    }
}