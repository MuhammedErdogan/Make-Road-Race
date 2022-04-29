/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;

namespace CGameStudio
{
    public class BaseCameraController : MonoBehaviour
    {
        public FixedViewTypes fixedViewType = FixedViewTypes.None;

        protected virtual void Start() => FixView();

        private void FixView()
        {
            if (fixedViewType == FixedViewTypes.None) return;

            Transform anchorHolder = transform.Find("ViewAnchors");
            if (anchorHolder == null) return;

            Transform anchor1 = anchorHolder.Find("Anchor1");
            if (anchor1 == null) return;

            Transform anchor2 = anchorHolder.Find("Anchor2");
            if (anchor2 == null) return;

            switch (fixedViewType)
            {
                case FixedViewTypes.Vertically:
                    Vector3 bottomEdge = Camera.main.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, 1)));
                    Vector3 topEdge = Camera.main.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight - 1, 1)));

                    Camera.main.fieldOfView = Camera.main.fieldOfView * (Vector3.Angle(anchor1.localPosition, anchor2.localPosition) / Vector3.Angle(bottomEdge, topEdge));
                    break;

                case FixedViewTypes.Horizontally:
                    Vector3 leftEdge = Camera.main.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, 1)));
                    Vector3 rightEdge = Camera.main.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 1, Camera.main.pixelHeight / 2, 1)));

                    Camera.main.fieldOfView = Camera.main.fieldOfView * (Vector3.Angle(anchor1.localPosition, anchor2.localPosition) / Vector3.Angle(leftEdge, rightEdge));
                    break;

                default:
                    break;
            }
        }

        public void SetNewFixedView()
        {
            if (fixedViewType == FixedViewTypes.None)
            {
                Transform existingAnchorHolder = transform.Find("ViewAnchors");
                if (existingAnchorHolder != null)
                {
                    DestroyImmediate(existingAnchorHolder.gameObject);
                }

                return;
            }

            Transform anchorHolder = transform.Find("ViewAnchors");
            if (anchorHolder == null)
            {
                anchorHolder = new GameObject("ViewAnchors").transform;
                anchorHolder.parent = transform;
                anchorHolder.transform.localPosition = Vector3.zero;
                anchorHolder.transform.localEulerAngles = Vector3.zero;
            }

            Transform anchor1 = anchorHolder.Find("Anchor1");
            if (anchor1 == null)
            {
                anchor1 = new GameObject("Anchor1").transform;
                anchor1.parent = anchorHolder;
                anchor1.transform.localPosition = Vector3.zero;
                anchor1.transform.localEulerAngles = Vector3.zero;
            }

            Transform anchor2 = anchorHolder.Find("Anchor2");
            if (anchor2 == null)
            {
                anchor2 = new GameObject("Anchor2").transform;
                anchor2.parent = anchorHolder;
                anchor2.transform.localPosition = Vector3.zero;
                anchor2.transform.localEulerAngles = Vector3.zero;
            }

            switch (fixedViewType)
            {
                case FixedViewTypes.Vertically:
                    anchor1.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, 1));
                    anchor2.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight - 1, 1));
                    break;
                case FixedViewTypes.Horizontally:
                    anchor1.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, 1));
                    anchor2.position = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 1, Camera.main.pixelHeight / 2, 1));
                    break;
                default:
                    break;
            }
        }
    }

    public enum FixedViewTypes { None, Vertically, Horizontally }
}