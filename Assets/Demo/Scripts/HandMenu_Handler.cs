using Lynx;
using System;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

namespace Lynx
{

    public class HandMenu_Handler : MonoBehaviour
    {

        [Serializable]
        enum HandSides
        {
            Left,
            Right,
            Both
        }

        [SerializeField] private HandSides handedness = HandSides.Both;

        [Space]
        [SerializeField] private float LookAtThreashold = 15;
        [SerializeField] private float LookUpThreashold = 20;

        [Space]
        [SerializeField] private Transform HandMenu;
        [SerializeField] private Vector2 PanelOffset = new Vector2(0.12f, 0.06f);

        private bool isDisplayed = false;
        private bool? isOnLeft = null;

        private void Start()
        {
            if (handedness == HandSides.Left || handedness == HandSides.Both) LynxHandtrackingAPI.LeftHandDynamicUpdate += LeftHandUpdate;
            if (handedness == HandSides.Right || handedness == HandSides.Both) LynxHandtrackingAPI.RightHandDynamicUpdate += RightHandUpdate;
            LynxHandtrackingAPI.EnableUpdate();
            HandMenu.gameObject.SetActive(false);
        }

        private void LeftHandUpdate()
        {
            if (isDisplayed && isOnLeft == false)
                return;

            if (LynxHandtrackingAPI.LeftHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose palm))
            {
                Vector3 palmPos = palm.position + Camera.main.transform.parent.position;
                Vector3 palmForward = (-palm.up);
                float camAngle = Vector3.Angle(palmForward, (Camera.main.transform.position - palmPos));

                float upAngle = Vector3.Angle(palm.forward, Vector3.up);

                if (camAngle < LookAtThreashold && upAngle < LookUpThreashold)
                {
                    isDisplayed = true;
                    isOnLeft = true;
                    HandMenu.gameObject.SetActive(true);
                    Quaternion palmRot = palm.rotation * Camera.main.transform.parent.rotation;

                    HandMenu.position = palmPos + (palmRot * new Vector3(-PanelOffset.x, 0, PanelOffset.y));
                    HandMenu.rotation = palmRot * Quaternion.Euler(-90, -180, 0);
                }
                else
                {
                    isDisplayed = false;
                    isOnLeft = null;
                    HandMenu.gameObject.SetActive(false);
                }

            }
        }

        private void RightHandUpdate()
        {
            if (isDisplayed && isOnLeft == true)
                return;

            if (LynxHandtrackingAPI.RightHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose palm))
            {
                Vector3 palmPos = palm.position + Camera.main.transform.parent.position;
                Vector3 palmForward = (-palm.up);
                float camAngle = Vector3.Angle(palmForward, (Camera.main.transform.position - palmPos));

                float upAngle = Vector3.Angle(palm.forward, Vector3.up);

                if (camAngle < LookAtThreashold && upAngle < LookUpThreashold)
                {
                    isDisplayed = true;
                    isOnLeft = false;
                    HandMenu.gameObject.SetActive(true);
                    Quaternion palmRot = palm.rotation * Camera.main.transform.parent.rotation;

                    HandMenu.position = palmPos + (palmRot * new Vector3(PanelOffset.x, 0, PanelOffset.y));
                    HandMenu.rotation = palmRot * Quaternion.Euler(-90, -180, 0);
                }
                else
                {
                    isDisplayed = false;
                    isOnLeft = null;
                    HandMenu.gameObject.SetActive(false);
                }

            }
        }

    } 
}