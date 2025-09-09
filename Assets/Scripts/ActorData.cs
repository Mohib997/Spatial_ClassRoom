using System;
using System.Collections;
using SpatialSys.UnitySDK;
using UnityEngine;

public class ActorData
{
    public int actorNumber { get; private set; }
    public string userID { get; private set; }
    public string username { get; private set; }
    public string displayName { get; private set; }
    public bool isSpaceOwner { get; private set; }
    public Texture2D avatarPicture { get; private set; }

    // Event relayed from IActor
    public event Action<bool> onAvatarExistsChanged;

    private IActor _actorRef;

    public void AssignFromActor(IActor actor, MonoBehaviour context, Action onComplete = null)
    {
        _actorRef = actor;
        actorNumber = actor.actorNumber;
        userID = actor.userID;
        username = actor.username;
        displayName = actor.displayName;
        isSpaceOwner = actor.isSpaceOwner;

        actor.onAvatarExistsChanged += HandleAvatarExistsChanged;

        context.StartCoroutine(DownloadProfilePicture(actor, onComplete));
    }

    private IEnumerator DownloadProfilePicture(IActor actor, Action onComplete)
    {
        var request = actor?.GetProfilePicture();

        // Wait until it is done
        while (!request.isDone)
        {
            yield return null;
        }

        if (request.succeeded)
        {
            Debug.Log($"BESTOO-> Picture Downloaded Successfully for {displayName}");
            avatarPicture = request.texture;
        }
        else
        {
            Debug.LogWarning($"BESTOO-> Failed to download profile picture for {displayName}");
        }

        onComplete?.Invoke(); // call callback if provided
    }

    private void HandleAvatarExistsChanged(bool exists)
    {
        Debug.Log($"Avatar exists changed for actor {actorNumber}: {displayName} -> {exists}");
        onAvatarExistsChanged?.Invoke(exists);
    }
}