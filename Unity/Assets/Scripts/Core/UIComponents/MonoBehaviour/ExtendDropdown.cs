using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityEngine.UI
{
	/// <summary>
	/// 扩展Dropdown
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class ExtendDropdown : Dropdown
	{
		/// <summary>
		/// templateSize
		/// </summary>
		[SerializeField]
		public Vector2 templateSize;

		/// <summary>
		/// templateAnchoredPosition
		/// </summary>
		[SerializeField]
		public Vector2 templateAnchoredPosition;

		/// <summary>
		/// 控制选checkmark的Active,适用于多个选中状态的图片
		/// </summary>
		[SerializeField]
		public bool controlCheckActive = false;

		public Action<bool, Toggle> onToggleValueChange;

		/// <summary>
		/// 重写CreateBlocker
		/// </summary>
		protected override GameObject CreateBlocker(Canvas rootCanvas)
		{
			GameObject blocker = base.CreateBlocker(rootCanvas);
			Canvas blockerCanvas = blocker.GetComponent<Canvas>();
			if (blockerCanvas != null)
			{
				blockerCanvas.sortingOrder += 1;
			}
			return blocker;
		}

		/// <summary>
		/// 展示信息
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();
			if (template != null)
			{
				template.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, templateSize.y);
				template.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, templateSize.x);
				template.anchoredPosition = templateAnchoredPosition;
			}
		}
		protected override DropdownItem CreateItem(DropdownItem itemTemplate)
		{
			DropdownItem dropdownItem = base.CreateItem(itemTemplate);
			dropdownItem.toggle.onValueChanged.AddListener((ison) =>
			{
				onToggleValueChange?.Invoke(ison, dropdownItem.toggle);
			});
			return dropdownItem;
		}

	}
}
