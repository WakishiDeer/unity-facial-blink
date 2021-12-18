using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBlink : MonoBehaviour
{
    private int blendShapeCount;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private Dictionary<string, int> blinkIndex;
    private Dictionary<string, float> blendWeight;
    private float blendSpeed = 1.8f;
    private bool isClose = false;
    
    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        // get # of blendshapes
        blendShapeCount = skinnedMesh.blendShapeCount;
        // instantiation and initialization for list of blink index
        blinkIndex = new Dictionary<string, int>();
        blinkIndex.Add("EyeBlink_L", 10);
        blinkIndex.Add("EyeBlink_R", 11);
        blinkIndex.Add("EyeSquint_L", 16);
        blinkIndex.Add("EyeSquint_R", 17);
        
        blendWeight = new Dictionary<string, float>();
        blendWeight.Add("EyeBlink_L", 0f);
        blendWeight.Add("EyeBlink_R", 0f);
        blendWeight.Add("EyeSquint_L", 0f);
        blendWeight.Add("EyeSquint_R", 0f);
        
        // if there are no blendshape, quit app
        if (blendShapeCount < 1)
        {
            Application.Quit();
        }
        else
        {
            // after waiting for `waitTime`, do the following method (lambda one)
            StartCoroutine(SwitchOpenClose(3.0f, () =>
            {
                isClose = !isClose;
            }));
        }
    }

    private IEnumerator SwitchOpenClose(float waitTime, Action action)
    {
        while (true)
        {
            // wait for `waitTime`
            yield return new WaitForSeconds(waitTime);
            // Proceed the process
            action();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void Close()
    {
        foreach (var kvp in blinkIndex)
        {
            if (blendWeight[kvp.Key] <= 100f)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(kvp.Value, blendWeight[kvp.Key]);
                blendWeight[kvp.Key] += blendSpeed; // add value to each weight of blendshape    
            }
            else
            {
                isClose = !isClose;
                break;
            }
        } 
    }

    private void Open()
    {
        foreach (var kvp in blinkIndex)
        {
            if (blendWeight[kvp.Key] > 0f)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(kvp.Value, blendWeight[kvp.Key]);
                blendWeight[kvp.Key] -= blendSpeed; // subtract value to each weight of blendshape    
            }
        } 
    }
}
