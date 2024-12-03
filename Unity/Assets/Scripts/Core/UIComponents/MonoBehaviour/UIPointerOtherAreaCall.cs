using System;
using UnityEngine;

    // 点击该rect 之外的区域触发回调
    public class UIPointerOtherAreaCall : MonoBehaviour
    {
        private Camera eventCamera;
        private Action callback;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = this.gameObject.GetComponent<RectTransform>();
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="camera">UI相机</param>
        /// <param name="callback">点击其他区域回调 </param>
        public void Init(Camera camera,System.Action callback)
        {
            this.eventCamera = camera;
            this.callback = callback;
        }
        
        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButton(0))
            {
                OnPointer(Input.mousePosition);
            }
#else
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OnPointer(Input.GetTouch(0).position);
            }
#endif
        }

        private void OnPointer(Vector2 screenPos)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPos, eventCamera))
            {
                this.callback?.Invoke();
            }
        }


    }

