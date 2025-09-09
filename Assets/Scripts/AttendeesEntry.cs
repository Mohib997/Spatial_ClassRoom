using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttendeesEntry : MonoBehaviour, IUIRaycastable
{
    [SerializeField] private TMP_Text _userNameText;
    [SerializeField] private Image _profilePicture;
    
    private Sprite _defaultProfilePicture;
    private string _roomNumber;
    private int _actorNumber;

    public System.DateTime JoinTime;
    // User info fields
    public string UserName
    {
        get => _userNameText.text;
        set
        {
            if (_userNameText != null)
            {
                _userNameText.text = value;
            }
        }
    }

    public Sprite ProfilePicture
    {
        get => _profilePicture.sprite;
        set
        {
            if (value == null)
            {
                value = _defaultProfilePicture; // Fallback to default if null
            }
            _profilePicture.sprite = value;
            // Optionally update UI element if needed
        }
    }

    public int ActorNumber
    {
        get => _actorNumber;
        set => _actorNumber = value;
    }
    public string RoomNumber
    {
        get => _roomNumber;
        set => _roomNumber = value;
    }
    private void Awake()
    {
        if (_userNameText == null)
        {
            _userNameText = GetComponentInChildren<TMP_Text>();
        }
    }

    public void OnRaycastHit()
    {
        Debug.Log("UI Button Clicked! " + UserName);
        // Handle your UI logic here
    }
}