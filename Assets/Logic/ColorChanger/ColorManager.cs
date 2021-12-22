using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Logic.ColorChanger
{
    public class ColorManager : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor = Color.white;

        [SerializeField] private HandTracking handTracking;

        private Color _color;
        private void Awake()
        {
            _color = _defaultColor;
        }

        public void ChangeColor(Color color)
        {
            _color = color;
            handTracking.UpdateColor(color);

        }

        public Color GetColor()
        {
            return _color;
        }
    }
}
