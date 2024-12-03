using UnityEngine;
using UnityEngine.UIElements;

namespace UILoopScroll
{
	//[RequireComponent(typeof(CanvasGroup))]
	public class LoopScrollItem : MonoBehaviour
	{
		protected RectTransform mRectTransform;
		protected DRect mRect;
		protected LoopScrollView mListView;

		public bool DisableRectSize = false;

		public LoopScrollView listView
		{
			get { return mListView; }
			set { mListView = value; }
		}

		private CanvasGroup _canvasGroup;
		public CanvasGroup canvasGroup
		{
			get
			{
				if (_canvasGroup == null)
				{
					_canvasGroup = GetComponent<CanvasGroup>();
					if (_canvasGroup == null)
					{
						_canvasGroup = gameObject.AddComponent<CanvasGroup>();
					}
				}
				return _canvasGroup;
			}
		}

		public RectTransform rectTransform
		{
			get
			{
				if (mRectTransform == null)
				{
					mRectTransform = transform as RectTransform;
				}
				return mRectTransform;
			}
		}


		/// <summary>
		/// 绑定的逻辑矩形。
		/// </summary>
		public DRect DRect
		{
			set
			{
				mRect = value;
			}
			get { return mRect; }
		}

		public void OnRenderer()
		{
			listView?.onRendererItem?.Invoke(mRect.Index, gameObject);
			if (listView.isItemSameSize == false && !DisableRectSize)
			{
				SetSzie(rectTransform.rect.size);
			}
		}

		public void OnClickItem()
		{
			listView?.onClickItem?.Invoke(mRect.Index, gameObject);
		}

		private void SetSzie(Vector2 size)
		{
			if (mRect.size != size)
			{
				listView.ChangeRect(mRect.Index, size);
				listView.RefreshAll();
			}
		}

		public void SetVisible(bool isVisible)
		{
			rectTransform.anchoredPosition = isVisible ? Vector2.zero : new Vector2(100000, 100000);

			//canvasGroup.alpha = isVisible ? 1 : 0;
			//canvasGroup.interactable = isVisible;
			//canvasGroup.blocksRaycasts = isVisible;
		}
	}
}
