using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 富文本类型
/// </summary>
public enum eRichType
{
    /// <summary>
	/// 无效
	/// </summary>
    none = 0,

	/// <summary>
	/// 下划线
	/// </summary>
	under,

	/// <summary>
	/// 删除线
	/// </summary>
	delete,
}

/// <summary>
/// 基础信息
/// </summary>
public class BaseRichInfo
{
	/// <summary>
	/// 起始顶点索引
	/// </summary>
	public int start_index;

	/// <summary>
	/// 结束顶点索引
	/// </summary>
	public int end_index;

	/// <summary>
	/// 富文本类型
	/// </summary>
	public eRichType rich_type;

	/// <summary>
	/// 换行的起点
	/// </summary>
	public List<int> line_feed_index = new List<int>();
}

/// <summary>
/// 删除线信息
/// </summary>
public class DeleteLineInfo : BaseRichInfo
{
	/// <summary>
	/// 删除线颜色
	/// </summary>
	public Color color;
}

/// <summary>
/// 绘制参数
/// </summary>
public class DrawData
{
    /// <summary>
	/// 起点 - X
	/// </summary>
	public float start_x;

	/// <summary>
	/// 终点 - X
	/// </summary>
	public float end_x;

	/// <summary>
	/// 下限 - Y
	/// </summary>
	public float min_y;

	/// <summary>
	/// 上限 - Y
	/// </summary>
	public float max_y;

	/// <summary>
	/// 信息
	/// </summary>
	public BaseRichInfo info;
}

/// <summary>
/// 图文并排控件
/// </summary>
public class RichText : Text
{
	/// <summary>
	/// 空格符
	/// </summary>
	private const string space_char = " ";

	/// <summary>
	/// 换行符
	/// </summary>
	private const string line_char = "\n";

	/// <summary>
	/// 富文本类型参数
	/// </summary>
	private const string type_arg = "type";

	/// <summary>
	/// 颜色参数
	/// </summary>
	private const string color_arg = "color";

	/// <summary>
	/// 解析完成后的最终文本
	/// </summary>
	private string output_text;

	/// <summary>
	/// 富文本正则
	/// </summary>
	private static readonly Regex rich_regex = new Regex(@"<rich (.*?)>(.*?)(</rich>)", RegexOptions.Singleline);

	/// <summary>
	/// 文本信息
	/// </summary>
	private static readonly List<Regex> rich_regex_array = new List<Regex>()
	{
	   new Regex(@"<size=(.*?)>(.*?)</size>", RegexOptions.Singleline),
	   new Regex(@"<color=(.*?)>(.*?)</color>", RegexOptions.Singleline)
	};

	/// <summary>
	/// 富文本信息列表
	/// </summary>
	private readonly List<BaseRichInfo> rich_array = new List<BaseRichInfo>();

	/// <summary>
	/// 文本构造器
	/// </summary>
	private StringBuilder textBuilder = new StringBuilder();

	/// <summary>
	/// 扩展适配宽
	/// </summary>
	public override float preferredWidth
	{
		get
		{
			TextGenerationSettings settings = GetGenerationSettings(Vector2.zero);
			if (output_text != null)
			{
				return cachedTextGeneratorForLayout.GetPreferredWidth(output_text, settings) / pixelsPerUnit;
			}
			else
			{
				return cachedTextGeneratorForLayout.GetPreferredWidth(m_Text, settings) / pixelsPerUnit;
			}
		}
	}

	/// <summary>
	/// 解读输出文本
	/// </summary>
	public override void SetVerticesDirty()
	{
		base.SetVerticesDirty();
		if (supportRichText)
		{
			output_text = ParseOutLine(m_Text);
		}
		else
		{
			output_text = m_Text;
		}
	}

	/// <summary>
	/// 设置删除线文本
	/// </summary>
	public void SetDeleteText(string context, string color = "#FF0000")
	{
		text = string.Format("<rich type = delete;color={0}>{1}</rich>", color, context);
	}

	/// <summary>
	/// 设置下划线文本
	/// </summary>
	public void SetUnderText(string context)
	{
		text = string.Format("<rich type = under>{0}</rich>", context);
	}

	/// <summary>
	/// 获得单行高度
	/// </summary>
	/// <returns></returns>
	protected float ParseCharHeight()
	{
		TextGenerator textGenerator = cachedTextGeneratorForLayout;
		TextGenerationSettings textSettings = GetGenerationSettings(Vector2.zero);
		return (textGenerator.GetPreferredHeight("A", textSettings) / pixelsPerUnit);
	}

	/// <summary>
	/// 解读参数
	/// </summary>
	protected Dictionary<string, string> ParseArg(string arg)
	{
		string[] split_array = arg.Split(';');
		Dictionary<string, string> array = new Dictionary<string, string>();
		foreach (string element in split_array)
		{
			string[] parm = element.Split('=');
			array.Add(parm[0].Trim(), parm[1].Trim());
		}
		return array;
	}

	/// <summary>
	/// 解读富文本类型
	/// </summary>
	protected bool ParseRichType(Dictionary<string, string> array, out eRichType type)
	{
		type = eRichType.none;
		if (array.TryGetValue(type_arg, out string value))
		{
			type = (eRichType)Enum.Parse(typeof(eRichType), value);
			return true;
		}
		return false;
	}

	/// <summary>
	/// 解读有效的数量
	/// </summary>
	protected int ParseVaildLength(string value)
	{
		int length = 0;
		Match match = null;
		foreach (Regex regex in rich_regex_array)
		{
			if (regex.IsMatch(value))
			{
				Match rex_match = regex.Match(value);
				if (match == null || rex_match.Length >= match.Length)
				{
					match = rex_match;
				}
			}
		}
		if (match == null)
		{
			return value.Length;
		}
		else
		{
			return length + (value.Length - match.Length) + ParseVaildLength(match.Groups[2].Value);
		}
	}

	/// <summary>
	/// 解读删除线信息
	/// </summary>
	protected void ParseDeleteLineInfo(Dictionary<string, string> array, ref DeleteLineInfo info)
	{
		if (array.TryGetValue(color_arg, out string value))
		{
			ColorUtility.TryParseHtmlString(value, out info.color);
		}
		else
		{
			info.color = color;
		}
	}

	/// <summary>
	/// 获取解析后的输出文本
	/// </summary>
	protected virtual string ParseOutLine(string text)
	{
		int count = 0;
		int indexText = 0;
		rich_array.Clear();
		textBuilder.Clear();
		foreach (Match match in rich_regex.Matches(text))
		{
			// 前面常规的文本内容
			string append_value = text.Substring(indexText, match.Index - indexText);
			textBuilder.Append(append_value);

			// 空格和换行符没有顶点渲染，所以需要先去掉再计算顶点
			string temp_value = append_value.Replace(space_char, string.Empty);
			temp_value = temp_value.Replace(line_char, string.Empty);
			count += (append_value.Length - temp_value.Length);

			// 解读信息
			Group typeGroup = match.Groups[1];
			Dictionary<string, string> split_array = ParseArg(typeGroup.Value);
			if (ParseRichType(split_array, out eRichType type))
			{
				switch (type)
				{
					case eRichType.delete:
						{
							Group group = match.Groups[2];
							int startIndex = (textBuilder.Length - count) * 4;
							int length = ParseVaildLength(group.Value);
							DeleteLineInfo info = new DeleteLineInfo()
							{
								rich_type = type,
								start_index = startIndex,
								end_index = startIndex + (length * 4)
							};
							ParseDeleteLineInfo(split_array, ref info);
							rich_array.Add(info);
							textBuilder.Append(group.Value);
						}
						break;
					default:
						{
							Group group = match.Groups[2];
							int length = ParseVaildLength(group.Value);
							int startIndex = (textBuilder.Length - count) * 4;
							BaseRichInfo info = new BaseRichInfo()
							{
								rich_type = type,
								start_index = startIndex,
								end_index = startIndex + (length * 4)
							};
							rich_array.Add(info);
							textBuilder.Append(group.Value);
						}
						break;
				}
			}
			indexText = match.Index + match.Length;
		}
		textBuilder.Append(text.Substring(indexText, text.Length - indexText));
		return textBuilder.ToString();
	}

	/// <summary>
	/// 重置绘制
	/// </summary>
	/// <param name="toFill"></param>
	protected override void OnPopulateMesh(VertexHelper toFill)
	{
	    if (supportRichText)
		{
			string orignText = m_Text;
			m_Text = output_text;
			base.OnPopulateMesh(toFill);
			m_Text = orignText;

			// 处理富文本
			float char_height = ParseCharHeight();
			UIVertex vert = new UIVertex();
			foreach (BaseRichInfo info in rich_array)
			{
				info.line_feed_index.Clear();
				if (info.start_index >= toFill.currentVertCount)
				{
					continue;
				}
				toFill.PopulateUIVertex(ref vert, info.start_index);

				Vector3 pos = vert.position;
				Vector3 lastPos = pos;
				int line_index = info.start_index;
				info.line_feed_index.Add(line_index);
				for (int i = info.start_index, m = info.end_index; i < m; i += 4)
				{
					if (i >= toFill.currentVertCount)
					{
						break;
					}
					toFill.PopulateUIVertex(ref vert, i);
					vert.color = color;
					toFill.SetUIVertex(vert, i);
					pos = vert.position;
					if (i > line_index)
					{
						if (Mathf.Abs(pos.y - lastPos.y) >= char_height)
						{
							info.line_feed_index.Add(i);
						}
						lastPos = pos;
					}
				}
			}
			Draw(toFill, char_height);
		}
		else
		{
			base.OnPopulateMesh(toFill);
		}
	}

	/// <summary>
	/// 绘制富文本
	/// </summary>
	private void Draw(VertexHelper vh, float charHeight)
	{
		UIVertex vert = new UIVertex();
		List<DrawData> pos_array = new List<DrawData>();
		foreach (BaseRichInfo info in rich_array)
		{
			if (info.start_index >= vh.currentVertCount)
			{
				continue;
			}

			// 绘制段落
			for (int index = 0; index < info.line_feed_index.Count; ++index)
			{
				DrawData data = new DrawData();
				data.info = info;

				// 起点 - X
				vh.PopulateUIVertex(ref vert, info.line_feed_index[index]);
				data.start_x = vert.position.x;
				data.max_y = vert.position.y;

				// Y - 下限
				vh.PopulateUIVertex(ref vert, info.line_feed_index[index] + 3);
				data.min_y = vert.position.y;

				// 终点 - X
				if (index + 1 < info.line_feed_index.Count)
				{
					vh.PopulateUIVertex(ref vert, info.line_feed_index[index + 1] - 3);
					data.end_x = vert.position.x;
				}
				else
				{
					vh.PopulateUIVertex(ref vert, info.end_index - 3);
					data.end_x = vert.position.x;
				}
				pos_array.Add(data);
			}
		}

		// 整体绘制
		foreach (DrawData element in pos_array)
		{
			switch (element.info.rich_type)
			{
				case eRichType.under:
					MeshUnderLine(vh, element, charHeight);
					break;
				case eRichType.delete:
					MeshDeleteLine(vh, element, charHeight);
					break;
			}
		}
	}

	/// <summary>
	/// 绘制下划线
	/// </summary>
	private void MeshUnderLine(VertexHelper vh, DrawData data, float charHeight)
	{
		int size_scale = 500;
		Vector2 extents = rectTransform.rect.size;
		var setting = GetGenerationSettings(extents);
		setting.fontStyle = FontStyle.Bold;
		setting.fontSize = fontSize + size_scale;
		setting.verticalOverflow = VerticalWrapMode.Overflow;
		setting.horizontalOverflow = HorizontalWrapMode.Overflow;
		cachedTextGeneratorForLayout.Populate("—", setting);
		IList<UIVertex> lineVer = cachedTextGeneratorForLayout.verts;

		Vector3[] pos = new Vector3[4];
		float space = charHeight / 8F;
		float space_y = data.min_y - 2F;
		pos[0] = new Vector2(data.start_x, space_y);
		pos[1] = new Vector2(data.end_x, space_y);
		pos[2] = new Vector2(data.end_x, space_y - space);
		pos[3] = new Vector2(data.start_x, space_y - space);

		UIVertex[] tempVerts = new UIVertex[4];
		for (int i = 0; i < 4; ++i)
		{
			tempVerts[i] = lineVer[i];
			tempVerts[i].color = color;
			tempVerts[i].position = pos[i];
		}
		vh.AddUIVertexQuad(tempVerts);
	}

	/// <summary>
	/// 绘制删除线
	/// </summary>
	private void MeshDeleteLine(VertexHelper vh, DrawData data, float charHeight)
	{
		int size_scale = 500;
		Vector2 extents = rectTransform.rect.size;
		var setting = GetGenerationSettings(extents);
		setting.fontStyle = FontStyle.Bold;
		setting.fontSize = fontSize + size_scale;
		setting.verticalOverflow = VerticalWrapMode.Overflow;
		setting.horizontalOverflow = HorizontalWrapMode.Overflow;
		cachedTextGeneratorForLayout.Populate("—", setting);
		IList<UIVertex> lineVer = cachedTextGeneratorForLayout.verts;

		Vector3[] pos = new Vector3[4];
		float offect_right = 3F;
		float space = charHeight / 8F;
		float space_y = ((data.max_y - data.min_y) / 2F) + data.min_y;
		pos[0] = new Vector2(data.start_x, space_y + (space / 2F));
		pos[1] = new Vector2(data.end_x + offect_right, space_y + (space / 2F));
		pos[2] = new Vector2(data.end_x + offect_right, space_y - (space / 2F));
		pos[3] = new Vector2(data.start_x, space_y - (space / 2F));

		DeleteLineInfo info = data.info as DeleteLineInfo;
		UIVertex[] tempVerts = new UIVertex[4];
		for (int i = 0; i < 4; i++)
		{
			tempVerts[i] = lineVer[i];
			tempVerts[i].color = info.color;
			tempVerts[i].position = pos[i];
		}
		vh.AddUIVertexQuad(tempVerts);
	}
}
