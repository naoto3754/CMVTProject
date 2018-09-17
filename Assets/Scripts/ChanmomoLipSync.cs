using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanmomoLipSync : MonoBehaviour 
{
	[SerializeField]
    private KeyboardVisibleController _VisibleController;

	[SerializeField]
    private KeyCode[] _Keys = new KeyCode[OVRLipSync.VisemeCount];

	[SerializeField]
    [Range(1, 100)]
    private int smoothAmount = 70;

    // PRIVATE

    // Look for a Phoneme Context (should be set at the same level as this component)
    private OVRLipSyncContextBase lipsyncContext = null;

    // Capture the old viseme frame (we will write back into this one)
    private OVRLipSync.Frame oldFrame = new OVRLipSync.Frame();

    void Start()
    {
        // make sure there is a phoneme context assigned to this object
        lipsyncContext = GetComponent<OVRLipSyncContextBase>();
        if (lipsyncContext == null)
        {
            Debug.LogWarning("LipSyncContextTextureFlip.Start WARNING:" +
                " No lip sync context component set to object");
        }
        else
        {
            // Send smoothing amount to context
            lipsyncContext.Smoothing = smoothAmount;
        }
    }

    void Update ()
    {
        if((lipsyncContext != null))
        {
            // trap inputs and send signals to phoneme engine for testing purposes

            // get the current viseme frame
            OVRLipSync.Frame frame = lipsyncContext.GetCurrentPhonemeFrame();
            if (frame != null)
            {
                // Perform smoothing here if on original provider
                if (lipsyncContext.provider == OVRLipSync.ContextProviders.Original)
                {
                    // Go through the current and old
                    for (int i = 0; i < frame.Visemes.Length; i++)
                    {
                        // Convert 1-100 to old * (0.00 - 0.99)
                        float smoothing = ((smoothAmount - 1) / 100.0f);
                        oldFrame.Visemes[i] =
                            oldFrame.Visemes[i] * smoothing +
                            frame.Visemes[i] * (1.0f - smoothing);
                    }
                }
                else
                {
                    oldFrame.Visemes = frame.Visemes;
                }

                SetVisemeToTexture();
            }
        }

        // Update smoothing value in context
        if (smoothAmount != lipsyncContext.Smoothing)
        {
            lipsyncContext.Smoothing = smoothAmount;
        }
    }

    void SetVisemeToTexture()
    {
        if(!_VisibleController)
        {
            return;
        }

        foreach(KeyCode removeKey in _Keys)
        {
            _VisibleController.RemovePushKey(removeKey);
        }


        // This setting will run through all the Visemes, find the
        // one with the greatest amplitude and set it to max value.
        // all other visemes will be set to zero.
        int   gV = -1;
        float gA = 0.0f;

        for (int i = 10; i < oldFrame.Visemes.Length; i++)
        {
            float viseme = oldFrame.Visemes[i];
            if(i == 11)
            {
                viseme = (oldFrame.Visemes[i] + oldFrame.Visemes[i + 1]) / 3.0f;
            }
            if(viseme > gA)
            {
                gV = i;
                gA = oldFrame.Visemes[i];
            }
        }

        if ((gV != -1) && (gV < _Keys.Length))
        {
            KeyCode key = _Keys[gV];

            _VisibleController.AddPushKey(key);
        }
    }
}
