using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Notifications;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameController : MonoBehaviour, INotifiable
    {
        // Static
        // -------------------------------------

        public enum Sounds
        {
            EXPLOSION,
            CRYSTAL_PICKUP
        }
        
        public static GameController instance;
    
        // Properties
        // -------------------------------------
    
        public bool isPaused = false;
        public bool isStarted = false;
        public bool gameOver = false;

        [SerializeField] private int requiredCrystals = 100;
        [SerializeField] private GameObject enemyGate;
        [SerializeField] private GameObject player;
        
        [Header("Sounds")]
        
        [SerializeField] private AudioClip explosionSound;
        [SerializeField] private AudioClip crystalPickupSound;
        
        private List<ToProtectController> toProtects;
        private Broadcaster broadcaster = new Broadcaster();
        private AudioSource audioSource;
    
        // Property accessors
        // -------------------------------------

        public Broadcaster Broadcaster
        {
            get { return broadcaster; }
        }

        // Public methods
        // -------------------------------------

        public void PlaySound(Sounds sound)
        {
            switch (sound)
            {
                case Sounds.EXPLOSION:
                    audioSource.clip = explosionSound;
                    audioSource.Play();
                    break;
                case Sounds.CRYSTAL_PICKUP:
                    audioSource.clip = crystalPickupSound;
                    audioSource.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sound", sound, null);
            }
        }
        
        public void OnNotification(Notification notification)
        {
            if (notification.type == Notification.Type.DEFENSIVE_STRUCTURE_DESTROYED)
            {
                DefenseDestroyedNotification destroyedNotification = notification as DefenseDestroyedNotification;
                if (destroyedNotification != null && destroyedNotification.defenseType.Equals("Tower"))
                {
                    GameOver();
                }
            }
            
            if (notification.type == Notification.Type.PLAYER_CRYSTALS_UPDATED)
            {
                PlayerCrystalsNotification crystalsNotification = notification as PlayerCrystalsNotification;
                if (crystalsNotification != null && crystalsNotification.crystalsQuantity >= requiredCrystals)
                {
                    GameWin();
                }
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
     
            isPaused = true;
            isStarted = false;
            gameOver = false;
            broadcaster = new Broadcaster();
            toProtects = new List<ToProtectController>();
            audioSource = GetComponent<AudioSource>();
        
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
            Broadcaster.RegisterNotifiable(this, Notification.Type.DEFENSIVE_STRUCTURE_DESTROYED);
            Broadcaster.RegisterNotifiable(this, Notification.Type.PLAYER_DIED);
            Broadcaster.RegisterNotifiable(this, Notification.Type.PLAYER_CRYSTALS_UPDATED);

            StartCoroutine(FollowPlayer(3.0f));
        }

        private void Start()
        {
            Notification notification = new RequiredCrystalsNotification(requiredCrystals);
            Broadcaster.Notify(notification);
        }
        
        private void Update()
        {
            if (!gameOver && isStarted && Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;

                if (isPaused)
                {
                    Notification notification = new Notification(Notification.Type.GAME_PAUSED_ON);
                    Broadcaster.Notify(notification);
                }
                else
                {
                    Notification notification = new Notification(Notification.Type.GAME_PAUSED_OFF);
                    Broadcaster.Notify(notification);
                }
            }
        }
    
        private void GameOver()
        {
            gameOver = true;
            isPaused = true;
        
            Notification notification = new Notification(Notification.Type.GAME_OVER);
            Broadcaster.Notify(notification);
        }

        private void GameWin()
        {
            gameOver = true;
            isPaused = true;
            
            Notification notification = new Notification(Notification.Type.GAME_WIN);
            Broadcaster.Notify(notification);
        }

        private IEnumerator FollowPlayer(float delay = 0)
        {
            float startAt = Time.time + delay;
            
            while (Time.time < startAt)
            {
                yield return null;
            }
            
            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            cameraController.target = player;
            cameraController.followSpeed = 0.05f;
            cameraController.followsTarget = true;

            isPaused = false;
            isStarted = true;

            // Wait 2 seconds before restoring the camera speed.
            startAt = Time.time + 2.0f;
            
            while (Time.time < startAt)
            {
                yield return null;
            }

            cameraController.followSpeed = 0.5f;
        }
    }
}