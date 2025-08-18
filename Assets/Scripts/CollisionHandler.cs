using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip finishSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!isControllable || !isCollidable) 
        {
            return;
        }

        switch (collision.gameObject.tag) 
        {
            case "Friendly":
                break;
            case "Finish":
                StartFinishSequence();
                break;
            case "Fuel":
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void StartFinishSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSFX);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    //private void Update()
    //{
    //    RespondToDebugKeys();
    //}

    //void RespondToDebugKeys()
    //{
    //    if (Keyboard.current.lKey.wasPressedThisFrame)
    //    {
    //        LoadNextLevel();
    //    }
    //    else if (Keyboard.current.rKey.wasPressedThisFrame)
    //    {
    //        ReloadLevel();
    //    }
    //    else if (Keyboard.current.cKey.wasPressedThisFrame)
    //    {
    //        isCollidable = !isCollidable;
    //    }
    //}
}
