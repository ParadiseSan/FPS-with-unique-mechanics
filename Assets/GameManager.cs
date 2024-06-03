using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;
    internal bool startGame;

    AudioManager audioManager;
    public static GameManager Instance
    {
        get
        {
            // If the instance hasn't been set yet, try to find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                // If it still hasn't been found, create a new GameObject and add the GameManager component to it
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }
  
    private void Awake()
    {

        audioManager = FindObjectOfType(typeof(AudioManager)).GetComponent<AudioManager>();
        // Ensure there's only one instance of the GameManager
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnClickGameStart()
    {
       
        UiManager.Instance.CloseStartUI();
        audioManager.PlaySong();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startGame = true;
    }

}
