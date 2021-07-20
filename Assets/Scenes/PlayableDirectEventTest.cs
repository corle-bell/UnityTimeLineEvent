using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayableDirectEventTest : MonoBehaviour
{
    public Bm.PlayableDirectorEventController directorEventController;
    // Start is called before the first frame update
    void Awake()
    {
        directorEventController.AddEventByPercent("进度", 0.5f, ()=> {
            Log("进度");
        });


        directorEventController.AddEventByTime("时间", 1.0f, () => {
            Log("时间");
        });

        directorEventController.AddEventByPercent("进度", 1.0f, () => {
            Log("结束了");
        });
    }


    public void Log(string _text)
    {
        Debug.Log("PlaybleEvent: "+_text);
    }

   
}
