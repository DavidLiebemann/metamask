using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace GamePlay
{
    public class DancerSelectionFeedback : AFeedback
    {
        [SerializeField] private Light spotlight;
        [SerializeField] private Vector2 spotlightIntensity = new Vector2(0.0f, 20.0f);


        private void Awake()
        {
            Assert.IsNotNull(spotlight);
        }


        protected override void Update()
        {
            base.Update();
            float lerpValue = curve.Evaluate(NormalizedPosition);
            spotlight.intensity = Mathf.Lerp(spotlightIntensity.x, spotlightIntensity.y, lerpValue);
        }
    }
}