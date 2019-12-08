using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour
{
    //열거형 변수 선언
    public enum MoveType
    {
        WAY_POINT
        ,LOOK_AT
        ,TELEPORT
    }

    public MoveType moveType = MoveType.WAY_POINT;
    public Transform[] points; //arrPoint, aPoint(X)

    //이동 속도
    public float speed = 1.0f;
    //회전 속도
    public float damping = 2.0f;
    //이동해야할 위치의 Index
    public int nextIdx = 1;

    //MainCamera의 Transform 컴포넌트를 저장
    private Transform camTr;
    //CharacterController 컴포넌트를 저장할 변수
    private CharacterController cc;

    void Start()
    {
        //Camera.main ==> 'MainCamera' 태그를 달고있는 게임오브젝트를 지칭.
        camTr = Camera.main.GetComponent<Transform>();
        cc    = GetComponent<CharacterController>();

        GameObject wayPointGroup = GameObject.Find("WayPointGroup");
        points = wayPointGroup.GetComponentsInChildren<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveType)
        {
            case MoveType.WAY_POINT:
                MoveWayPoint();
                break;

            case MoveType.LOOK_AT:
                MoveLookAt();
                break;
        }
    }

    void MoveWayPoint()
    {
        //1) 방향벡터를 계산
        Vector3 moveDir = points[nextIdx].position - transform.position;
        //2) 방향벡터를 쿼터니언 타입의 각도로 계산
        Quaternion rot = Quaternion.LookRotation(moveDir);
        //3) 보간함수를 이용해서 부드럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
        //4) 이동처리
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider coll)
    {
        //WayPoint 에 충돌여부를 판단
        if (coll.CompareTag("WAY_POINT"))
        {
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;

            // nextIdx = nextIdx + 1;
            // if (nextIdx >= points.Length)
            // {
            //     nextIdx = 1;
            // }
        }
    }


    void MoveLookAt()
    {
        cc.SimpleMove(camTr.forward * speed);
    }


}
