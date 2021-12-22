using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    [SerializeField] GameObject _fingertip;

    GameObject _indexCursor;
    MixedRealityPose _indexPose;

    void Start()
    {
        _indexCursor = Instantiate(_fingertip);
    }

    void Update()
    {
        _indexCursor.GetComponent<Renderer>().enabled = false;

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out _indexPose))
        {

            _indexCursor.GetComponent<Renderer>().enabled = true;
            _indexCursor.transform.position = _indexPose.Position;
        }
    }

    public void UpdateColor(Color color)
    {
        _indexCursor.GetComponent<Renderer>().material.color = color;

    }
}
