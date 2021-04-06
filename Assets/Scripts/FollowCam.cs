using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //兴趣点常量坐标

    public float easing = 0.05f;

    public float camZ;  //摄像机的z坐标
    
    public Vector2 minXY = Vector2.zero;

    private void Awake()
    {
        camZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        if (POI == null)
        {
            return;
        }
        //获取兴趣点的位置
        Vector3 destination = POI.transform.position;
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

            //在摄像机当前位置和目标位置之间增添差值
        destination = Vector3.Lerp(transform.position, destination, easing);
        
        //保持 destination.z 的值为 camZ 使摄像机足够远
        destination.z = camZ;
        //将摄像机的位置设置到 destination
        transform.position = destination;
        //设置摄像机的正交值,使地面始终处于画面之中
        Camera.main.orthographicSize = destination.y + 10;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
