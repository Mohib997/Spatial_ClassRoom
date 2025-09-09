using SpatialSys.UnitySDK;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    [SerializeField] private string _roomNumber;
    [SerializeField] private PostViewerEntry _postViewEntry;
    [SerializeField] private AttendeesEntry _attendeesEntry;
    [SerializeField] private RectTransform _SpawnParentPostVeiw;
    [SerializeField] private RectTransform _SpawnParentAttendees;
    [SerializeField] private TMP_InputField _postInputField;
    [SerializeField] private TMP_InputField _inpTitlePost;
    [SerializeField] private Button _btnSubmit;
    [SerializeField] private Button _btnWritepost;

    public event Action<Room, string> OnPostSubmitted;

    public string RoomNumber => _roomNumber;

    public RectTransform SpawnParentPostVeiw => _SpawnParentPostVeiw;

    

    private void Start()
    {
        _postInputField.text = string.Empty;
        DestroyCaret(_postInputField);
        DestroyCaret(_inpTitlePost);

        if (_btnWritepost != null)
            _btnWritepost.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_btnWritepost != null)
            _btnWritepost.onClick.AddListener(OnWritePostClicked);
        _btnSubmit.onClick.AddListener(OnSubmitPost);
    }

    private void OnDisable()
    {
        _btnSubmit.onClick.RemoveAllListeners();
        if (_btnWritepost != null)
            _btnWritepost.onClick.RemoveAllListeners();
    }

    private void DestroyCaret(TMP_InputField inputField)
    {
        // Try to get the caret using TMP_SelectionCaret, but only destroy if it exists and is not already destroyed
        var caretComponent = inputField.GetComponentInChildren<TMP_SelectionCaret>();
        if (caretComponent != null && caretComponent.gameObject != null)
        {
            Destroy(caretComponent.gameObject);
        }
    }

    public void OnWritePostClicked()
    {
        _inpTitlePost.text = string.Empty;
        _postInputField.text = string.Empty;
        _inpTitlePost.interactable = true;
        _postInputField.interactable = true;
        if (_btnWritepost != null) _btnWritepost.gameObject.SetActive(false);
        if (_btnSubmit != null) _btnSubmit.gameObject.SetActive(true);
    }

    private void OnSubmitPost()
    {
        string title = string.IsNullOrEmpty(_inpTitlePost.text) ? "Untitled Post" : _inpTitlePost.text;
        string content = _postInputField.text;

        if (string.IsNullOrEmpty(content))
        {
            SpatialBridge.coreGUIService.DisplayToastMessage("Please enter some content for the post.");
            Debug.LogWarning("Post content is empty.");
            return;
        }
        string author = SpatialBridge.actorService.localActor.displayName;
        string postData = $"{title}#{content}#{_roomNumber}#{author}";

        // Clear the input fields after submission
        _inpTitlePost.text = string.Empty;
        _postInputField.text = string.Empty;

        OnPostSubmitted?.Invoke(this, postData);
    }

    public AttendeesEntry OnActorJoinedInRoom(IActor joinedActor)
    {
        ActorData actorData = new ActorData();
        AttendeesEntry entry = new AttendeesEntry();
        actorData.AssignFromActor(joinedActor, this, () => {

            // Instantiate UI after avatar is downloaded
            entry = Instantiate(_attendeesEntry, _SpawnParentAttendees);
            entry.UserName = actorData.displayName;
            entry.RoomNumber = RoomNumber;
            entry.ActorNumber = actorData.actorNumber;
            entry.JoinTime = DateTime.Now;
            entry.ProfilePicture = ConvertTextureToSprite(actorData.avatarPicture);
        });
        return entry;
    }

    public AttendeesEntry OnActorLeftRoom(IActor leftActor)
    {
        AttendeesEntry attendeesEntry = new AttendeesEntry();
        foreach (Transform child in _SpawnParentAttendees)
        {
            AttendeesEntry entry = child.GetComponent<AttendeesEntry>();
            if (entry != null && entry.ActorNumber == leftActor.actorNumber)
            {
                Destroy(child.gameObject);
                break;
            }
        }
        return attendeesEntry;
    }

    public Sprite ConvertTextureToSprite(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.Log("BESTOO-> TEXTURE IS NULL");
            return null;
        }
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void SetInputFields(string title, string content)
    {
        _inpTitlePost.text = title;
        _postInputField.text = content;
        _inpTitlePost.interactable = false;
        _postInputField.interactable = false;
        if (_btnSubmit != null) _btnSubmit.gameObject.SetActive(false);
        if (_btnWritepost != null) _btnWritepost.gameObject.SetActive(true);
    }
}