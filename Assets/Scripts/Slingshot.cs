using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static public Slingshot S;
    
    public GameObject launchPoint;

    public GameObject prefabProjectile;

    public Vector3 launchPos;

    public GameObject projectile;

    public bool aimingMode;

    public float velocityMult = 8f;

    private Rigidbody projectileRigidbody;

    private void Awake()
    {
        S = this;
        
        var launchPointTransform = transform.Find("LaunchPoint");

        launchPoint = launchPointTransform.gameObject;
        
        launchPoint.SetActive(false);

        launchPos = launchPointTransform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null)
            {
                return Vector3.zero;
            }

            return S.launchPos;
        }
    }

   

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
        //print("SlingShot:OnMouseEnter()");
    }

    private void OnMouseExit()
    {
        //print("SlingShot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        aimingMode = true;
        
        projectile = Instantiate( prefabProjectile);

        projectile.transform.position = launchPos;

        projectile.GetComponent<Rigidbody>().isKinematic = true;

        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
    
    // Update is called once per frame
         void Update()
         {
             if (!aimingMode)
             {
                 return;
             }
     
             //获取鼠标光标在2d窗口中的当前坐标
             Vector3 mousePos2D = Input.mousePosition;
             mousePos2D.z = -Camera.main.transform.position.z;
             //坐标转换
             Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
             
             //计算launchPos 到 mousePos3D两点之间的坐标差
             Vector3 mouseDelta = mousePos3D - launchPos;
             //将mouseDelta 坐标差限制在弹弓的球状碰撞器半径范围内
             float maxMagnitude = GetComponent<SphereCollider>().radius;
             if (mouseDelta.magnitude > maxMagnitude)
             {
                 mouseDelta.Normalize();
                 mouseDelta *= maxMagnitude;
             }
             
             //将projectile移动到新位置
             Vector3 projPos = launchPos + mouseDelta;
             projectile.transform.position = projPos;
             if (Input.GetMouseButtonUp(0))
             {
                 //如果已经松开鼠标
                 aimingMode = false;
                 projectileRigidbody.isKinematic = false;
                 //赋予向量反方向的初始速度飞出
                 projectileRigidbody.velocity = -mouseDelta * velocityMult;
                 FollowCam.POI = projectile;
                 projectile = null;
                 MissionDemolition.ShotFired();
                 ProjectileLine.S.Poi = projectile;
             }
         }
}
