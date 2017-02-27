﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHold : ClimbingHold
{

    private int enterCount;
    private IEnumerator coroutine;
    public string sceneName;

    void OnStart()
    {
        enterCount = 0;
    }
    public GameObject canvasGameObject;

    void OnUpdate()
    {

    }

    // must call setup script
    public void setup(string sceneName)
    {
        this.sceneName = sceneName;
        coroutine = TransitionToSceneWithDelay(sceneName, 1);
    }

    IEnumerator TransitionToSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        StopCoroutine(coroutine);
        enterCount--;
        if (enterCount == 0)
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite != null)
            {
                Sprite currSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                gameObject.GetComponent<SpriteRenderer>().sprite = (currSprite == ClimbingHold.customHoldSprite0)
                    ? ClimbingHold.customHoldSprite1
                    : ClimbingHold.customHoldSprite0;
            }
            else
            {
                gameObject.GetComponent<LineRenderer>()
                    .startColor = UnityEngine.Color.cyan;
                gameObject.GetComponent<LineRenderer>()
                    .endColor = UnityEngine.Color.cyan;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Triggering collision");
        StartCoroutine(coroutine);
        enterCount++;
        if (enterCount > 0)
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite != null)
            {
                Sprite currSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                gameObject.GetComponent<SpriteRenderer>().sprite = (currSprite == ClimbingHold.customHoldSprite0)
                    ? ClimbingHold.customHoldSprite1
                    : ClimbingHold.customHoldSprite0;
            }
            else
            {
                gameObject.GetComponent<LineRenderer>()
                    .startColor = UnityEngine.Color.green;
                gameObject.GetComponent<LineRenderer>()
                    .endColor = UnityEngine.Color.green;
            }
        }
    }

    void OnMouseDown()
    {
        OnTriggerEnter2D(null);
    }

    private void OnDisable()
    {
        TextMesh textMesh = gameObject.GetComponentInChildren<TextMesh>();
        Destroy(textMesh);
    }
}
