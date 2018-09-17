using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanmomoLipSync : MonoBehaviour 
{
	[SerializeField]
    private KeyboardVisibleController _VisibleController;

	[SerializeField]
    private KeyCode[] _Keys = new KeyCode[5];

    [Tooltip("aa, E, ih, oh, ou のそれぞれの音素へ遷移する際に、BlendShapeの重みを時間をかけて変化させるためのカーブ")]
    public AnimationCurve[] transitionCurves = new AnimationCurve[5];

    [Tooltip("カーブの値をBlendShapeに適用する際の倍率")]
    public float curveAmplifier = 100.0f;

    [Range(0.0f, 100.0f), Tooltip("この閾値未満の音素の重みは無視する")]
    public float weightThreashold = 2.0f;

    [Tooltip("BlendShapeの重みを変化させるフレームレート")]
    public float frameRate = 12.0f;

    [Tooltip("aa, E, ih, oh, ouの順で割り当てるBlendShapeのindex")]
    public int[] visemeToBlendShape = new int[5];

    [Tooltip("OVRLipSyncに渡すSmoothing amountの値")]
    public int smoothAmount = 100;

    [Tooltip("Eの倍率")]
    public float eRate = 1.0f;


    OVRLipSyncContextBase context;
    OVRLipSync.Viseme previousViseme = OVRLipSync.Viseme.sil;
    float transitionTimer = 0.0f;
    float frameRateTimer = 0.0f;

    void Start() 
    {
        context = GetComponent<OVRLipSyncContextBase>();
        if (context == null) 
        {
            Debug.LogError("同じGameObjectにOVRLipSyncContextBaseを継承したクラスが見つかりません。", this);
        }

        context.Smoothing = smoothAmount;
    }

    void Update() 
    {
        if (context == null) 
        {
            return;
        }

        var frame = context.GetCurrentPhonemeFrame();
        if (frame == null) {
            return;
        }

        transitionTimer += Time.deltaTime;

        // 設定したフレームレートへUpdate関数を低下させる
        frameRateTimer += Time.deltaTime;
        if (frameRateTimer < 1.0f / frameRate) {
            return;
        }
        frameRateTimer -= 1.0f / frameRate;

        foreach(var key in _Keys)
        {
            if(_VisibleController)
            {
                _VisibleController.RemovePushKey(key);
            }
        }

        // 最大の重みを持つ音素を探す
        var maxVisemeIndex = 0;
        var maxVisemeWeight = 0.0f;
        // 子音は無視する
        for (var i = (int)OVRLipSync.Viseme.aa; i < frame.Visemes.Length; i++) {
            var weight = frame.Visemes[i];
            if(i == (int)OVRLipSync.Viseme.E)
            {
                weight *= eRate;
            }
            //Debug.Log((OVRLipSync.Viseme)i + " = " + weight);
            if (weight > maxVisemeWeight) {
                maxVisemeWeight = weight;
                maxVisemeIndex = i;
            }
        }

        // 音素の重みが小さすぎる場合は口を閉じる
        if (maxVisemeWeight * 100.0f < weightThreashold) {
            transitionTimer = 0.0f;
            return;
        }

        // 音素の切り替わりでタイマーをリセットする
        if (previousViseme != (OVRLipSync.Viseme)maxVisemeIndex) {
            transitionTimer = 0.0f;
            previousViseme = (OVRLipSync.Viseme)maxVisemeIndex;
        }

        var visemeIndex = maxVisemeIndex - (int)OVRLipSync.Viseme.aa;
        if(visemeIndex >= 0 && visemeIndex < _Keys.Length)
        {
            if(_VisibleController)
            {
                _VisibleController.AddPushKey(_Keys[visemeIndex]);
            }
        }
    }
}