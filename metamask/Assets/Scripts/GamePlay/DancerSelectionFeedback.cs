using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GamePlay
{
    public class DancerSelectionFeedback : AFeedback
    {
        [SerializeField] private Light spotlight;
        [SerializeField] private Vector2 spotlightIntensity = new Vector2(0.0f, 20.0f);
        [SerializeField] private Transform sizeTarget;
        


        private void Awake()
        {
            Assert.IsNotNull(spotlight);
        }

        private void OnEnable()
        {
        }


        protected override void Update()
        {
            base.Update();
            float lerpValue = curve.Evaluate(NormalizedPosition);
            spotlight.intensity = Mathf.Lerp(spotlightIntensity.x, spotlightIntensity.y, lerpValue);

            if (sizeTarget)
            {
                sizeTarget.localScale = new Vector3(lerpValue, lerpValue, lerpValue);
            }
        }
    }
}