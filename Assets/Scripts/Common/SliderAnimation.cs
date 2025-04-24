// Copyright (C) 2015-2021 gamevanilla - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UltimateClean
{
    /// <summary>
    /// This component is used to provide idle slider animations in the demos.
    /// </summary>
    public class SliderAnimation : MonoBehaviour
    {
        public TextMeshProUGUI text;

        public float duration = 1;

        private Image image;
        private SlicedFilledImage slicedImage;

        private StringBuilder strBuilder = new StringBuilder(4);
        private int lastPercentage = -1;

        private Coroutine _coroutine;

        private void Awake()
        {
            image = GetComponent<Image>();
            slicedImage = GetComponent<SlicedFilledImage>();

            // if (duration > 0)
            //     StartCoroutine(Animate());
        }

        public void StartAnimate(int curr, int goal, float duration, bool isLoop = false)
        {
            this.duration = duration;
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Animate(curr, goal, isLoop));
        }
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private IEnumerator Animate(int curr, int goal, bool isLoop)
        {
            if (curr == 0)
            {
                if (image != null)
                    image.fillAmount = 0;
                else if (slicedImage != null)
                    slicedImage.fillAmount = 0;
                text.text = $"{curr} / {goal}";
                yield break;
            }
            while (true)
            {
                var ratio = 0.0f;
                var maxRatio = curr / (float)goal;
                var multiplier = 1.0f / duration;
                while (ratio < maxRatio)
                {
                    ratio += Time.deltaTime * multiplier;

                    if (image != null)
                        image.fillAmount = ratio;
                    else if (slicedImage != null)
                        slicedImage.fillAmount = ratio;

                    var count = (int)(ratio/1.0f * goal);
                    if (count != lastPercentage)
                    {
                        lastPercentage = count;
                        if (text != null)
                        {
                            strBuilder.Clear();
                            text.text = strBuilder.Append(count).Append(" / ").Append(goal).ToString();
                        }
                    }

                    yield return null;
                }

                if (text != null)
                    text.text = $"{curr} / {goal}";

                yield return null;

                if (!isLoop) break;

                while (ratio > 0)
                {
                    ratio -= Time.deltaTime * multiplier;

                    if (image != null)
                        image.fillAmount = ratio;
                    else if (slicedImage != null)
                        slicedImage.fillAmount = ratio;

                    if (text != null)
                        text.text = $"{(int)(ratio/1.0f * 100)}%";

                    yield return null;
                }
            }
        }
    }
}