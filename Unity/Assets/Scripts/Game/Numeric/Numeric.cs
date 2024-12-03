using App;
using System;
using System.Collections.Generic;

public partial class Numeric
{
	private static UDebugger debugger = new UDebugger("NumericDamage");

	private static int lastNum;
	private int[] numericMap;


	public const float THOUSAND_RATE = 10000.0f;
	private Action<NumericType> onUpdate;

	public Numeric(Action<NumericType> updateAction = null)
	{
		InitArray();

		numericMap = new int[lastNum];
		onUpdate = updateAction;
	}

	private static void InitArray()
	{
		var values = Enum.GetValues(typeof(NumericType));

		lastNum = (int)values.GetValue(values.Length - 1) + 1;

		//#if UNITY_EDITOR
		//		debugger.Log(string.Format("目前总数值字段数量{0},数组长度{1}", values.Length, lastNum));
		//#endif
	}



	public int this[NumericType numericType]
	{
		get
		{
			return GetByKey(numericType);
		}
		set
		{
			SetByKey(numericType, value);
			if (onUpdate != null)
			{
				onUpdate(numericType);
			}
			else
			{
				Update(numericType);
			}
		}
	}


	public float GetAsFloat(NumericType numericType)
	{
		return (float)(GetByKey(numericType) / Numeric.THOUSAND_RATE);
	}
	public float GetAsFloat(int numericType)
	{
		return (float)(GetByKey(numericType) / Numeric.THOUSAND_RATE);
	}

	public int GetAsInt(NumericType numericType)
	{
		return GetByKey(numericType);
	}

	// 非万分率数值使用Set
	public void Set(NumericType key, int value)
	{
		this[key] = value;
	}

	// 万分率数值使用SetRate
	// public void SetRate(NumericType key, int value)
	// {
	// 	this[key] = value;
	// }

	public float GetThousandPct(NumericType key)
	{
		return numericMap[(int)key];
	}

	public int GetByKey(int key)
	{
		return numericMap[key];
	}
	public int GetByKey(NumericType key)
	{
		return GetByKey((int)key);
	}

	public void SetByKey(int key, int value)
	{
		this.numericMap[key] = value;
	}

	public void SetByKey(NumericType key, int value)
	{
		SetByKey((int)key, value);
	}



	public void Clear()
	{
		for (int i = 0; i < numericMap.Length; i++)
		{
			numericMap[i] = 0;
		}
	}



	private void Update(NumericType numericType)
	{
		switch (numericType)
		{
			case NumericType.max_hp:
			case NumericType.max_hp_rate:
				UpdateFinal1(NumericType.max_hp, NumericType.max_hp_rate, NumericType.f_max_hp);
				break;

			case NumericType.atk:
				UpdateFinal1(NumericType.atk, NumericType.atk_rate, NumericType.f_atk);
				break;
			case NumericType.atk_rate:
				UpdateFinal1(NumericType.atk, NumericType.atk_rate, NumericType.f_atk);
				OnAtkRateUpdate();
				break;

			case NumericType.def:
			case NumericType.def_rate:
				UpdateFinal1(NumericType.def, NumericType.def_rate, NumericType.f_def);
				break;

			case NumericType.move_speed:
			case NumericType.move_speed_rate:
				UpdateFinal1(NumericType.move_speed, NumericType.move_speed_rate, NumericType.f_move_speed);
				break;

			case NumericType.heal:
			case NumericType.heal_rate:
				UpdateFinal1(NumericType.heal, NumericType.heal_rate, NumericType.f_heal);
				break;
			case NumericType.gun_atk:
			case NumericType.fire_atk:
			case NumericType.ice_atk:
			case NumericType.flash_atk:
			case NumericType.dark_atk:
			case NumericType.light_atk:
			case NumericType.phy_atk:
				UpdateFinalElementAtk(numericType);
				break;
		}
	}



	//通用更新终值方法1 (万分比)
	//例：最终攻击力=攻击力*(1+攻击力%)
	private void UpdateFinal1(NumericType bas, NumericType rate, NumericType final)
	{
		this.SetFinalValue(final, (Numeric.THOUSAND_RATE + this[rate]) / Numeric.THOUSAND_RATE * this[bas]);
	}

	//最终射击间隔=射击间隔/(1+射速%)
	//最终换弹时间=换弹时间/(1+换弹速度%)
	private void UpdateFinal2(NumericType rate, NumericType final, int param)
	{
		this.SetFinalValue(final, param / ((Numeric.THOUSAND_RATE + this[rate]) / Numeric.THOUSAND_RATE));
	}

	//最终枪械攻击力=(枪械攻击力)*(1+攻击力%)
	private void UpdateFinalElementAtk(NumericType elementAtkType)
	{
		UpdateFinal1(elementAtkType, NumericType.atk_rate, (NumericType)((int)elementAtkType + 100));
	}

	private void OnAtkRateUpdate()
	{
		//UpdateFinalElementAtk(NumericType.gun_atk);
		//UpdateFinalElementAtk(NumericType.fire_atk);
		int min = (int)NumericType.gun_atk;
		int max = (int)NumericType.phy_atk;
		for (int i = min; i <= max; i++)
		{
			UpdateFinalElementAtk((NumericType)i);
		}
	}


	//更新终值
	private void SetFinalValue(NumericType final, float value)
	{
		this[final] = (int)value;
	}

	//更新终值
	private void SetFinalValue(NumericType final, int value)
	{
		this[final] = value;
	}






	public static Numeric operator +(Numeric numeric1, Numeric numeric2)
	{
		if (numeric2 != null)
		{
			for (int i = 0; i < numeric2.numericMap.Length; i++)
			{
				numeric1.numericMap[i] += numeric2.numericMap[i];
			}
		}

		return numeric1;
	}
	public static Numeric operator -(Numeric numeric1, Numeric numeric2)
	{

		for (int i = 0; i < numeric2.numericMap.Length; i++)
		{
			numeric1.numericMap[i] -= numeric2.numericMap[i];
		}
		return numeric1;
	}
	public static Dictionary<NumericType, int> NumericAdd(Dictionary<NumericType, int> numeric1, Dictionary<NumericType, int> numeric2)
	{
		foreach (var item in numeric2)
		{
			if (numeric1.ContainsKey(item.Key))
			{
				numeric1[item.Key] += item.Value;
			}
			else
			{
				numeric1[item.Key] = item.Value;
			}
		}
		return numeric1;
	}

	public static Dictionary<NumericType, int> NumericAdd(Dictionary<NumericType, int> numeric1, Dictionary<NumericType, int> numeric2, float scalePct)
	{
		foreach (var item in numeric2)
		{
			if (numeric1.ContainsKey(item.Key))
			{
				numeric1[item.Key] += (int)(item.Value * scalePct);
			}
			else
			{
				numeric1[item.Key] = (int)(item.Value * scalePct);
			}
		}
		return numeric1;
	}
	public static Dictionary<NumericType, int> NumericSub(Dictionary<NumericType, int> numeric1, Dictionary<NumericType, int> numeric2)
	{
		foreach (var item in numeric2)
		{
			if (numeric1.ContainsKey(item.Key))
			{
				numeric1[item.Key] -= item.Value;
			}
			else
			{
				numeric1[item.Key] = -item.Value;
			}
		}
		return numeric1;
	}
}
