using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace Assets.Logic.ColorChanger
{
    public class SliderChangeColor : MonoBehaviour
    {
        [SerializeField]
        private Renderer TargetRenderer;

        [SerializeField]
        private ColorManager _colorManager;

        public void OnSliderUpdatedRed(SliderEventData eventData)
        {
            TargetRenderer = GetComponentInChildren<Renderer>();
            if ((TargetRenderer != null) && (TargetRenderer.material != null))
            {
                TargetRenderer.material.color = new Color(eventData.NewValue, TargetRenderer.sharedMaterial.color.g, TargetRenderer.sharedMaterial.color.b);
                _colorManager.ChangeColor(TargetRenderer.material.color);
            }
        }

        public void OnSliderUpdatedGreen(SliderEventData eventData)
        {
            TargetRenderer = GetComponentInChildren<Renderer>();
            if ((TargetRenderer != null) && (TargetRenderer.material != null))
            {
                TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.r, eventData.NewValue, TargetRenderer.sharedMaterial.color.b);
                _colorManager.ChangeColor(TargetRenderer.material.color);
            }
        }

        public void OnSliderUpdateBlue(SliderEventData eventData)
        {
            TargetRenderer = GetComponentInChildren<Renderer>();
            if ((TargetRenderer != null) && (TargetRenderer.material != null))
            {
                TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.r, TargetRenderer.sharedMaterial.color.g, eventData.NewValue);
                _colorManager.ChangeColor(TargetRenderer.material.color);
            }
        }
    }
}
