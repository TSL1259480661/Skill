using ClientData;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常用静态接口
/// </summary>
public static class NumberCommonHelper
{
    /// <summary>
    /// 百
    /// </summary>
    public const int Hundred = 100;

	/// <summary>
	/// 千
	/// </summary>
	public const int Thousand = 1000;

	/// <summary>
	/// 万
	/// </summary>
	public const int TenThousand = 10000;

	/// <summary>
	/// 十万
	/// </summary>
	public const int HundredThousand = 100000;

    /// <summary>
    /// 百万
    /// </summary>
    public const int Million = 1000000;

	/// <summary>
    /// 千万
    /// </summary>
    public const int TenMillion = 10000000;

    /// <summary>
    /// 亿
    /// </summary>
    public const long Billion = 100000000;

    /// <summary>
    /// 百亿
    /// </summary>
    public const long TenBillion = 10000000000;

	/// <summary>
	/// 天（单位：秒）
	/// </summary>
	public const int Day = 86400;

	/// <summary>
	/// 时（单位：秒）
	/// </summary>
	public const int Hour = 3600;

	/// <summary>
	/// 分（单位：秒）
	/// </summary>
	public const int Minute = 60;

    // 定义单位和对应的阈值
    private static readonly (string Unit, long LowerBound, long UpperBound)[] units = new[]
    {
        ("K", 1000, 100000),  // K: 1000 to 99999
        ("M", 1000000, 1000000000),  // M: 1000000 to 999999999
        ("B", 1000000000, 1000000000000L),  // B: 1000000000 to 999999999999
        ("T", 1000000000000L, 1000000000000000L),  // T: 1000000000000 to 999999999999999
        ("Q", 1000000000000000L, 1000000000000000000L)  // Q: 1000000000000000 to 999999999999999999
    };

	/// <summary>
	/// 数值显示转化
	/// 大于100万的以万为单位，大于100亿的以亿为单位，均保留小数点后2位
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string ParseNumberUnit(long value)
    {
        if (value < Million)
        {
            return value.ToString();
        }
        else if (value >= TenBillion)
        {
            double radio = Convert.ToDouble(value) / Billion;
            return string.Format("{0}亿", radio.ToString("0.##"));
        }
        else
        {
            double radio = Convert.ToDouble(value) / TenThousand;
            return string.Format("{0}万", radio.ToString("0.##"));
        }
    }

	/// <summary>
	/// 货币通用显示规则
	/// </summary>
	/// <returns></returns>
	public static string ParseCoinNumberUnit(int value)
	{
		if (value < Million)
		{
			return value.ToString();
		}
		else
		{
			double radio = Convert.ToDouble(value) / Thousand;
			return string.Format("{0}k", radio.ToString("0.#"));
		}
	}

    /// <summary>
    /// 判断配置数组是否有效
    /// </summary>
    /// <returns></returns>
    public static bool IsVaildProtobufArray(ProtobufArrayInt data)
    {
        if (data == null || data.IntArrays == null || data.IntArrays.Length <= 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 解读倒计时格式
    /// </summary>
    public static string ParseTimeToString(int second)
    {
        TimeSpan span = new TimeSpan(0, 0, second);
        return span.ToString(@"hh\:mm\:ss");
    }

	/// <summary>
	/// 配置时间格式解读为时间戳(单位：毫秒)
	/// </summary>
	public static long ParseTimestampByConfig(string time_value)
	{  
	    if (string.IsNullOrEmpty(time_value))
		{
			return 0u;
		}
		else
		{
		    if (!time_value.Contains(":"))
			{
				time_value = string.Format("{0} 05:00:00", time_value);
			}
			DateTime date = DateTime.ParseExact(time_value, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			//TimeSpan span = date - ServerTimeModule.epochTime;
			//return (long)span.TotalMilliseconds;
			return 0u;
		}
	}

	/// <summary>
	/// 解读商品时间
	/// </summary>
	public static string ParseTimeByTimeSpan(long span)
	{
		long value = span / Thousand;
	    if (value >= Day)
		{
			int day = Mathf.CeilToInt(value / Convert.ToSingle(Day));
			return string.Format("{0}天", day);
		}
		else if (value >= Hour)
		{
			int hour = Mathf.CeilToInt(value / Convert.ToSingle(Hour));
			return string.Format("{0}小时", hour);
		}
		else
		{
			int minute = Mathf.CeilToInt(value / Convert.ToSingle(Minute));
			return string.Format("{0}分钟", minute);
		}
	}

	/// <summary>
	/// 时间转小时字符串
	/// </summary>
	public static string ParseHour(int value)
	{
		int hour = Mathf.FloorToInt(value / 3600F);
		return string.Format("{0}小时", hour);
	}

	/// <summary>
	/// 时间转时分字符串
	/// </summary>
	public static string ParseHourAndMinute(int value)
	{
		int hour = Mathf.FloorToInt(value / 3600F);
		int minute = Mathf.CeilToInt((value % 3600) / 60F);
		return string.Format("{0}小时{1}分", hour, minute);
	}

	/// <summary>
	/// 属性列表追加
	/// </summary>
	public static void Append(this Dictionary<NumericType, int> target, NumericType type, int value)
	{ 
	    if (target.ContainsKey(type))
		{
			target[type] += value;
		}
		else
		{
			target.Add(type, value);
		}
	}

	/// <summary>
	/// 属性列表追加
	/// </summary>
	public static void Append(this Dictionary<NumericType, int> target, Dictionary<NumericType, int> value)
	{
	    foreach (KeyValuePair<NumericType, int> pair in value)
		{
			target.Append(pair.Key, pair.Value);
		}
	}

	/// <summary>
	/// 根据坐标生成唯一Keys
	/// </summary>
	public static int CreateGridKeyByPosition(Vector3 pos)
	{
		return (int)(pos.x * 100) * 100 + (int)(pos.z * 100);
	}

	/// <summary>
	/// 初始4位规则，> 9999 显示 9999
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public static string ParseNumberKeepFourDigits(long num)
	{
		return num < TenThousand ? num.ToString() : "9999";
	}

	/// <summary>
	/// 初始5位规则
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public static string ParseNumberKeepFiveDigits(long num)
	{
		if (num < HundredThousand)
		{
			return num.ToString();
		}
		else if( num >= HundredThousand && num < TenMillion)
		{
			
			double radio = Convert.ToDouble(num) / TenThousand;
			var tmpValue = Math.Floor(radio * 10) / 10;
            return string.Format("{0}万", tmpValue.ToString("F1"));
		}
		else if (num >= TenMillion && num < Billion)
		{
			double radio =  Convert.ToDouble(num) / TenThousand;
			var tmpValue = Math.Floor(radio * 10) / 10;
            return string.Format("{0}万", tmpValue.ToString("F0"));
		}
		else
		{
			return "9999+万";
		}
	}

	/// <summary>
	/// 初始6位规则
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public static string ParseNumberKeepSixDigits(long num)
	{
		if (num < Million)
		{
			return num.ToString();
		}
		else if( num >= Million && num < TenMillion)
		{
			double radio = Convert.ToDouble(num) / TenThousand;
			var tmpValue = Math.Floor(radio * 100) / 100;
            return string.Format("{0}万", tmpValue.ToString("F2"));
		}
		else if (num >= TenMillion && num < Billion)
		{
			double radio = Convert.ToDouble(num) / TenThousand;
			var tmpValue = Math.Floor(radio * 100) / 100;
            return string.Format("{0}万", tmpValue.ToString("F1"));
		}
		else
		{
			return "9999+万";
		}
	}

	/// <summary>
	/// 局内伤害飘字显示
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public static string ParseNumberInGame(long num)
	{

		if (num < 1e3)
		{
			return num.ToString();
		}

		else if (num > 1e18)
		{
			// 如果数值非常大，超出定义的范围
			return "999Q";
		}
		else
		{
			double tmpValue = 0;
			string tmpStr = "";
			foreach (var (unit, lowerBound, upperBound) in units)
			{
				if (num >= lowerBound && num < upperBound) // 检查当前单位范围
				{
					tmpValue = num / (double)lowerBound;
					if (tmpValue >= 100)
					{
						tmpStr = ((long)tmpValue).ToString() + unit;
						break;
					}
					else
					{
						double roundedValue = Math.Floor(tmpValue * 10) / 10;
						tmpStr = roundedValue.ToString("F1") + unit;
						break;
					}
				}
			}
			return tmpStr;
		}
	}

	//万分比，四舍五入保留1位小数
	public static string ParseThousandPercent(int value) 
	{
		return $"{Math.Round(value / 100f, 1,MidpointRounding.AwayFromZero)}%";
	}

	//万分比，四舍五入保留1位小数
	public static string ParseThousandPercent(float value)
	{
		return $"{Math.Round(value / 100f, 1, MidpointRounding.AwayFromZero)}%";
	}

	//属性显示值，只显示时使用
	public static string ParseAttrValue(int value,int showType)
	{
		if (showType==1)
		{
			return ParseThousandPercent(value);
		}
		return value.ToString();
	}

	public static string ParseAttrValue(float value, int showType)
	{
		if (showType == 1)
		{
			return ParseThousandPercent(value);
		}
		return value.ToString();
	}
}
