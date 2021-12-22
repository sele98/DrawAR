
using Microsoft.MixedReality.Toolkit.Utilities;

namespace Assets.Logic
{
    public static class GestureUtils
    {

        private const float PINCH_TRESHOLD = 0.7f;

        public static bool IsPinching(Handedness trackedHand)
        {
            return HandPoseUtils.CalculateIndexPinch(trackedHand) > PINCH_TRESHOLD;
        }
    }
}
