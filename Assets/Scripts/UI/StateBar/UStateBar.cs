using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UStateBar : _UP
{
    public override string Name => "UStateBar";
    public Text TextYear, TextHour, TextState, TextSpeed;
    public Button ButtonSlow, ButtonPlay, ButtonFaster;
    public GameObject ImagePlaying, ImagePausing;

    public override void WhenFirstShow()
    {
        ButtonSlow.onClick.AddListener(OnClickSlower);
        ButtonFaster.onClick.AddListener(OnClickFaster);
        ButtonPlay.onClick.AddListener(OnClickPlaying);
        _E.AddListener(EventID.TickEnd, WhenRefresh);
        base.WhenFirstShow();
    }

    public override void WhenDestroy()
    {
        _E.RemoveListener(EventID.TickEnd, WhenRefresh);
        base.WhenDestroy();
    }

    public override void WhenRefresh()
    {
        // 速度
        if (_D.Inst.Playing)
        {
            ImagePausing.SetActive(true);
            ImagePlaying.SetActive(false);
            TextSpeed.text = string.Format("x{0}", _D.Inst.Speed);
            // 时间
            TextYear.text = string.Format("{0}年{1}月{2}日",
                2019 + _D.Inst.Years,
                _D.Inst.Months,
                _D.Inst.Days);
            TextHour.text = string.Format("{0:D02}:{1:D02}:{2:D02}",
                _D.Inst.Hours,
                _D.Inst.Minutes,
                Random.Range(0, 60));
            // 人数
            TextState.text = string.Format("总人口：{0}  患病人数：{1}",
                _D.Inst.Health + _D.Inst.Patient, _D.Inst.Patient);
        }
        else {
            ImagePausing.SetActive(false);
            ImagePlaying.SetActive(true);
            TextSpeed.text = "Pausing";
        }
        base.WhenRefresh();
    }

    private void OnClickPlaying()
    {
        if (_D.Inst.Playing)
            _D.Inst.Pause();
        else
            _D.Inst.Unpause();
    }

    private void OnClickSlower()
    {
        _D.Inst.Slower();
    }

    private void OnClickFaster()
    {
        _D.Inst.Faster();
    }
}
