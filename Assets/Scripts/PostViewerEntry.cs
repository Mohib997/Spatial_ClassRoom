using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostViewerEntry : MonoBehaviour, IUIRaycastable
{
    [SerializeField] private TMP_Text _postTitle;
    [SerializeField] private TMP_Text _postAuthor;
    [SerializeField] private TMP_Text _roomNumber;
    private Room _room;
    //public PostManager postManager;
    private string _postContent;

    public string PostTitle
    {
        get => _postTitle.text;
        set => _postTitle.text = value;
    }
    public string PostContent
    {
        get => _postContent;
        set => _postContent = value;
    }

    public string PostAuthor
    {
        get => _postAuthor != null ? _postAuthor.text.Replace("Author: ", "") : "Anonymous";
        set { if (_postAuthor != null) _postAuthor.text = $"Author: {value}"; }
    }
    public string RoomNumber
    {
        get
        {
            if (_roomNumber != null && _roomNumber.text != null)
            {
                var parts = _roomNumber.text.Split('#');
                return parts.Length > 1 ? parts[1] : "N/A";
            }
            return "N/A";
        }
        set { if (_roomNumber != null) _roomNumber.text = $"R#{value}"; }
    }

    public Room Room
    {
        get => _room;
        set
        {
            _room = value;
        }
    }

    public void OnRaycastHit()
    {
        Debug.Log("UI Button Clicked!" + gameObject.name);
        if (Room != null)
        {
            Room.SetInputFields(PostTitle, PostContent);
        }
    }
}