using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public List<CharacterData> followers;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    public void AddFollower(CharacterData follower)
    {
        if (follower.CompareTag("Character"))
        {
            if (!followers.Contains(follower))
            {
                followers.Add(follower);
            }
        }
    }

    public void RemoveFollower(CharacterData follower)
    {
        if (followers.Contains(follower))
        {
            followers.Remove(follower);
        }
    }

}
