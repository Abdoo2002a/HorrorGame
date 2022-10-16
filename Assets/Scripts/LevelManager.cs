using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    public GameObject miniGamePlayer;

    public GameObject gamePosition;

    public GameObject roomsTemplate;

    public GameObject FirstRoom;
    
    [HideInInspector]
    public float timeForSpawningKey;

    [HideInInspector]
    public float timeForSpawning;

    [HideInInspector]
    public float timer;
    // if you spawn this room the dungen get genrated
    //[HideInInspector]           for debuging 
  
    private void Awake()
    {
        timeForSpawning = MiniGame.instance.currentDesk.disk.difficulty;      
        timeForSpawningKey = timeForSpawning + 0.1f;     
        if(instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); } 
    }

    private void Start()
    {
        Instantiate(gamePosition, gamePosition.transform.position, gamePosition.transform.rotation);
        spawnLevel();
        Invoke("spawnKey", timeForSpawningKey);
    }

    private void Update()
    {
        if(timeForSpawning < 0)
        {
            return;
        } else { timeForSpawning -= Time.deltaTime;}
    }
    void spawnLevel()
    {
        Instantiate(roomsTemplate, gameObject.transform);
        Instantiate(FirstRoom, gamePosition.transform.position, FirstRoom.transform.rotation);
        Instantiate(miniGamePlayer, gamePosition.transform.position + miniGamePlayer.transform.position, miniGamePlayer.transform.rotation);      
    }

    void spawnKey()
    {
        for (int i = 0; i < RoomTemplates.Instance.roomList.Count; i++)
        {
            if (i == RoomTemplates.Instance.roomList.Count - 1)
            {
                Instantiate(RoomTemplates.Instance.key, RoomTemplates.Instance.roomList[i].transform.position, RoomTemplates.Instance.key.transform.rotation);
            }
        }
    }


    public void destroyCurrentLevel()
    {
        Destroy(GameObject.Find("2dplayer(Clone)"));
        foreach (GameObject room in RoomTemplates.Instance.roomList)
        {
            Destroy(room);
        }
        foreach (GameObject room in RoomTemplates.Instance.blockRooms)
        {
            Destroy(room);
        }
        RoomTemplates.Instance.roomList.Clear();
        RoomTemplates.Instance.blockRooms.Clear();
        Destroy(GameObject.Find("key(Clone)"));
        Destroy(GameObject.Find("gamePos(Clone)"));
        Destroy(RoomTemplates.Instance.transform.gameObject);
        Destroy(gameObject);
    }

    



}
