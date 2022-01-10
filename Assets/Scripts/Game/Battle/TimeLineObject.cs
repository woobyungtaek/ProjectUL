﻿/*
타임 라인 객체 ( 반드시 상속 받은 상태로 사용 )
전투 시 시간이 흐르는 객체
TimeLine UI에 프로필 등록 및 값 변경시 갱신
대기 시간이 0가 되면 대기 리스트에 추가
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TimeLineObject
{
    public TimeLineObject()
    {
        // 생성 즉시 BattleManager의 CreateUI 실행
        BattleManager.Instance.CreateThumbNailUI(this);
    }

    // 기준 시간
    private static readonly float StandardTime = 100.0f;

    // 현재 스피드
    private float mSpeed = 10;

    // 현재 시간
    private float mCurrentTime;

    public float CurrentTime
    {
        get { return mCurrentTime; }
    }

    public void FlowTime()
    {
        // 시간이 흐른다. Update문에서 실행된다.
        // speed만큼의 속도로 시간이 지난다.
        // 기준 시간 만큼 되면 대기 리스트에 추가된다.
        mCurrentTime += (mSpeed * Time.deltaTime *  Time.timeScale);
        //Debug.Log($"{mCurrentTime}");

        if(mCurrentTime > StandardTime)
        {
            Debug.Log("대기 리스트에 추가 해야해");
            BattleManager.Instance.AddTurnWaitObj(this);
            mCurrentTime = 0;
        }
    }

    private void SetCurrentTime(float time)
    {
        // 현재 시간을 특정 시간으로 바꾼다.
        // time값이 100이면 최초 상태로 초기화
    }

    // 행동권을 획득했을 때 할 행동을 스스로 정의해야한다.
    public abstract void TurnActionFunc();
}
