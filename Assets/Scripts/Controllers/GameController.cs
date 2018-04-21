using System.Collections.Generic;
using Controllers;
using Interfaces;
using Notifications;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour, INotifiable
{
    // Static
    // -------------------------------------
    
    public static GameController instance;
    
    // Properties
    // -------------------------------------
    
    public bool isPaused = false;

    private List<ToProtectController> toProtects;
    private Broadcaster broadcaster = new Broadcaster();
    
    // Property accessors
    // -------------------------------------

    public Broadcaster Broadcaster
    {
        get { return broadcaster; }
    }

    // Public methods
    // -------------------------------------
    
    public void OnNotification(Notification notification)
    {
        if (notification.type == Notification.Type.TO_PROTECT_DESTROYED)
        {
            GameOver();
        }
    }

    public void LoadLevel(int level)
    {
        isPaused = false;
        broadcaster = new Broadcaster();
        SceneManager.LoadScene(level);
    }

    public void RetryLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    
    // Private methods
    // -------------------------------------
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Disabled the Singleton behaviour for now.
        // The notifiable objects seems to be duplicated.
        
        //if (instance != this)
        //{
        //    Destroy(gameObject);
        //}
        
        //DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        Debug.Log("GameController::Init");
        
        toProtects = new List<ToProtectController>();
        
        GameObject[] toProtectsArray = GameObject.FindGameObjectsWithTag("ToProtect");
        foreach (GameObject obj in toProtectsArray)
        {
            ToProtectController controller = obj.GetComponent<ToProtectController>();
            if (controller != null)
            {
                toProtects.Add(controller);
            }
        }
        
        // Notifications
        Broadcaster.RegisterNotifiable(this, Notification.Type.TO_PROTECT_DESTROYED);
    }

    private void GameOver()
    {
        isPaused = true;
        
        Notification notification = new Notification(Notification.Type.GAME_OVER);
        Broadcaster.Notify(notification);
    }
}