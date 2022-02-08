using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private TextMeshProUGUI testText;
    private bool isEndingPanelEnable;
    float time;
    float anyKeyTime;
    // Start is called before the first frame update
    void Start()
    {
        anyKeyTime = 0;
        time = 0;
        endingText.color = new Color(endingText.color.r, endingText.color.g, endingText.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && isEndingPanelEnable)
        {
            SceneMove.instance.Title();
        }
        FadeInText();
        Debug.Log(LookObj.isDestination);
    }

    private void FadeInText()
    {
        if (LookObj.isDestination)
        {
            endingPanel.SetActive(true);
            if (time < 3f)
            {
                endingText.text = "You Are Survived";
                endingText.color = new Color(endingText.color.r, endingText.color.g, endingText.color.b, time / 3);
            }
            else
            {
                isEndingPanelEnable = true;
            }
            time += Time.deltaTime;
            if (isEndingPanelEnable == true)
            {
                
                testText.color = new Color(testText.color.r, testText.color.g, testText.color.b, anyKeyTime / 3);
                testText.text = "Press Any Key.";
                anyKeyTime += Time.deltaTime;
            }

                
        }
    }
}
