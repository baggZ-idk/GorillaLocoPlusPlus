
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaLocomotion
{
    public class GorillaFingers : MonoBehaviour
    {
        [Serializable]
        public class Finger
        {
            public List<Transform> Bones = new List<Transform>();

            [Min(0f)]
            public float MaxAngle = 160f;

            public Vector3 RotationAxis = Vector3.right;

            [Range(0f, 1f)]
            public float Curl;
        }

        public Finger Pinky = new Finger();
        public Finger Index = new Finger();
        public Finger Thumb = new Finger();

        public bool UseLeftHand = true;

        [Range(0f, 1f)]
        public float ButtonCurl = 1f;

        private Quaternion[][] initialRotations;

        private void Awake()
        {
            CacheInitialRotations();
        }

        private void CacheInitialRotations()
        {
            Finger[] fingers = { Pinky, Index, Thumb };

            initialRotations = new Quaternion[fingers.Length][];

            for (int i = 0; i < fingers.Length; i++)
            {
                Finger finger = fingers[i];

                initialRotations[i] =
                    new Quaternion[finger.Bones.Count];

                for (int j = 0; j < finger.Bones.Count; j++)
                {
                    Transform bone = finger.Bones[j];

                    if (bone != null)
                        initialRotations[i][j] = bone.localRotation;
                }
            }
        }

        private void LateUpdate()
        {
            GorillaInputs inputs = UseLeftHand
                ? GorillaInputs.Left
                : GorillaInputs.Right;

            Pinky.Curl = inputs.GripPressure;
            Index.Curl = inputs.TriggerPressure;

            Thumb.Curl =
                inputs.PrimaryButton != GorillaInputs.InputState.Off ||
                inputs.SecondaryButton != GorillaInputs.InputState.Off
                    ? ButtonCurl
                    : 0f;

            Finger[] fingers = { Pinky, Index, Thumb };

            for (int i = 0; i < fingers.Length; i++)
                ApplyFinger(fingers[i], initialRotations[i]);
        }

        private void ApplyFinger(
            Finger finger,
            Quaternion[] startingRotations)
        {
            if (finger.Bones == null ||
                startingRotations == null)
                return;

            int boneCount = finger.Bones.Count;

            if (boneCount == 0)
                return;

            Vector3 axis = finger.RotationAxis.normalized;

            if (axis == Vector3.zero)
                return;

            float curl = Mathf.Clamp01(finger.Curl);
            float totalAngle = finger.MaxAngle * curl;
            float anglePerBone = totalAngle / boneCount;

            for (int i = 0; i < boneCount; i++)
            {
                Transform bone = finger.Bones[i];

                if (bone == null)
                    continue;

                bone.localRotation =
                    startingRotations[i] *
                    Quaternion.AngleAxis(anglePerBone, axis);
            }
        }
    }
}
