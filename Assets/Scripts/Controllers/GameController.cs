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
    public bool gameOver = false;

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
        
        if (notification.type == Notification.Type.PLAYER_DIED)
        {
            GameOver();
        }
    }

    public void LoadLevel(int level)
    {
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
     
        isPaused = false;
        gameOver = false;
        broadcaster = new Broadcaster();
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
        Broadcaster.RegisterNotifiable(this, Notification.Type.PLAYER_DIED);
    }

    private void Update()
    {
        if (!gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
    }
    
    private void GameOver()
    {
        gameOver = true;
        isPaused = true;
        
        Notification notification = new Notification(Notification.Type.GAME_OVER);
        Broadcaster.Notify(notification);
    }
}