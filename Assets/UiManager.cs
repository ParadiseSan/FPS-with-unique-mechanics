using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] Canvas StartUI;


    // Singleton instance
    private static UiManager _instance;
    

    
    public static UiManager Instance
    {
        get
        {
            // If the instance hasn't been set yet, try to find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<UiManager>();

                // If it still hasn't been found, create a new GameObject and add the GameManager component to it
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<UiManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {

        
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

    public void CloseStartUI()
    {
        StartUI.transform.GetChild(0).gameObject.SetActive(false);
    }
}
