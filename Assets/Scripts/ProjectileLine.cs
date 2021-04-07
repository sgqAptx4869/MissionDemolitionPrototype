using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;

    public float minDist = 0.1f;

    public LineRenderer line;

    private GameObject _poi;

    public List<Vector3> points;

    private void Awake()
    {
        S = this; //设置单例对象
        //获取对线渲染器的引用
        line = GetComponent<LineRenderer>();
        //在需要使用线渲染器之前 将其禁用
        line.enabled = false;
        //初始化三维向量点的List
        points = new List<Vector3>();
    }

    public GameObject Poi
    {
        get => _poi;
        set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //用于直接清除线条
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //在线条上加一个点
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //如果该点与上一个点的位置不够远，则返回
            return;
        }

        if (points.Count == 0) //当前是发射点
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            // 添加一根线条，帮助之后瞄准
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2; //设置前两个点
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            line.enabled = true;
        }
        else
        {
            //正常加点的操作
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                //如果当前还没有点 返回原点
                return Vector3.zero;
            }

            return points[points.Count - 1];
        }
    }

    private void FixedUpdate()
    {
        if (Poi == null)
        {
            //如果兴趣点不存在,则找一个
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.CompareTag("Projectile"))
                {
                    Poi = FollowCam.POI;
                }
                else
                {
                    return; //未找到直接返回
                }
            }
            else
            {
                return;
            }
        }
        //如果存在兴趣点 则在FixedUpdate中它的位置上增加一个点
        AddPoint();
        if (FollowCam.POI == null)
        {
            //当 FollowCam.POI 为null 时使当前的Poi 也为null
            Poi = null;
        }
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