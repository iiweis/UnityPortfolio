using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityRandom = UnityEngine.Random;

public class MainModel : MonoBehaviour
{
    private readonly BoolReactiveProperty isStart = new BoolReactiveProperty();
    private readonly IntReactiveProperty level = new IntReactiveProperty(1);
    private readonly ReactiveProperty<TimeSpan> elapsed = new ReactiveProperty<TimeSpan>();
    private readonly ReactiveProperty<TimeSpan> remainingTime = new ReactiveProperty<TimeSpan>();
    private readonly ReactiveProperty<bool?> result = new ReactiveProperty<bool?>();
    private readonly BoolReactiveProperty isSlash = new BoolReactiveProperty();
    private readonly ReactiveProperty<TimeSpan> timeLimit = new ReactiveProperty<TimeSpan>();

    private IDisposable intervalOvervable;
    private Dictionary<int, TimeSpan> timeLimitPerLevel;

    /// <summary>
    /// �Q�[�����J�n���Ă��邩�ǂ���
    /// </summary>
    public IReactiveProperty<bool> IsStart => isStart;

    public IReactiveProperty<int> Level => level;

    /// <summary>
    /// �Q�[���J�n����̌o�ߎ���
    /// </summary>
    public IReadOnlyReactiveProperty<TimeSpan> Elapsed => elapsed;

    /// <summary>
    /// �c�莞��
    /// </summary>
    public IReactiveProperty<TimeSpan> RemainingTime => remainingTime;

    /// <summary>
    /// ����
    /// </summary>
    public IReactiveProperty<bool?> Result => result;

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyReactiveProperty<bool> IsSlash => isSlash;

    /// <summary>
    /// ��������
    /// </summary>
    public IReadOnlyReactiveProperty<TimeSpan> TimeLimit => timeLimit;

    private void Start()
    {
        // ���x�����Ƃ̐�������
        timeLimitPerLevel = new Dictionary<int, TimeSpan>()
        {
            { 1, TimeSpan.FromSeconds(3) },
            { 2, TimeSpan.FromSeconds(2) },
            { 3, TimeSpan.FromSeconds(1) },
            { 4, TimeSpan.FromSeconds(0.7) },
            { 5, TimeSpan.FromSeconds(0.6) },
            { 6, TimeSpan.FromSeconds(0.5) },
            { 7, TimeSpan.FromSeconds(0.4) },
            { 8, TimeSpan.FromSeconds(0.3) },
            { 9, TimeSpan.FromSeconds(0.2) },
            { 10, TimeSpan.FromSeconds(0.15) },
        };

        // 5�`15�b�ҋ@���Ă���Q�[���J�n
        TimeSpan dueTime = TimeSpan.FromSeconds(5 + UnityRandom.Range(0, 10 + 1));
        Observable.Timer(dueTime).SubscribeWithState(this, (_, myself) =>
        {
            // �Q�[���J�n��ʒm
            myself.isStart.Value = true;
        }, myself =>
        {
            if (myself.isStart.Value)
            {
                // �Q�[���J�n

                // �������Ԑݒ�
                TimeSpan timeLimit = myself.timeLimitPerLevel[myself.level.Value];
                myself.timeLimit.Value = timeLimit;
                myself.remainingTime.Value = timeLimit;

                // ���Ԍv���p��Observable�𔭍s
                myself.intervalOvervable = Observable.TimeInterval(Observable.EveryUpdate()).SubscribeWithState(this, (value, myself) =>
                {
                    // �o�ߎ��Ԃ͉��Z
                    myself.elapsed.Value += value.Interval;

                    // �c�莞�Ԃ͌��Z
                    myself.remainingTime.Value -= value.Interval;
                }).AddTo(this);
            }
        }).AddTo(this);

        isStart.SubscribeWithState(this, (value, myself) =>
        {
            if (!value)
            {
                // �Q�[���I����͌o�ߎ��Ԍv���p��Observable�͕K�v�Ȃ��̂Ŕj��
                myself.intervalOvervable?.Dispose();
            }
        }).AddTo(this);


        remainingTime.SubscribeWithState(this, (value, myself) =>
        {
            if (value.Ticks < 0)
            {
                // �������Ԍo�߂ŃQ�[�����s
                myself.isStart.Value = false;
            }
        }).AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isStart.Value || intervalOvervable is null)
            {
                // ���Ԍv�����n�܂��Ă��Ȃ��Ƃ��Ƀ{�^���������Ă����玸�s
                result.Value = false;
                return;
            }

            isSlash.Value = true;
            isSlash.Value = false;
        }
    }
}
