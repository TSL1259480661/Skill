using App;
using System;

public class NumericComponent
{
	private readonly Random random = new Random(Guid.NewGuid().GetHashCode());

	private Numeric numeric;

	private static UDebugger debugger = new UDebugger("NumericComponent");

	public const float THOUSAND_RATE = 10000.0f;

	public NumericComponent()
	{
		//numeric = new Numeric();
	}

	private int dodgeK1;
	private int dodgeK2;
	private int attackK;
	private int criticalK1;
	private int criticalK2;
	private int criticalBaseHurt;

	private int hp_than_extradmgRate;
	private int lv_less_dmg;
	private int distance_less_dmg;
	//public void Init(Numeric numeric, int dodgeK1, int dodgeK2, int attackK, int criticalK1, int criticalK2, int criticalBaseHurt, int hp_than_extradmgRate, int lv_less_dmg)
	//{
	//	this.dodgeK1 = dodgeK1;
	//	this.dodgeK2 = dodgeK2;
	//	this.attackK = attackK;
	//	this.criticalK1 = criticalK1;
	//	this.criticalK2 = criticalK2;
	//	this.criticalBaseHurt = criticalBaseHurt;

	//	this.hp_than_extradmgRate = hp_than_extradmgRate;
	//	this.lv_less_dmg = lv_less_dmg;
	//	BindNumeric(numeric);
	//}

	public void Init(Numeric numeric, ClientData.Global_Param_global_paramCategory category)
	{
		//this.dodgeK1 = category.Get(3028).value;
		//this.dodgeK2 = category.Get(3029).value;
		//this.attackK = category.Get(3030).value;
		//this.criticalK1 = category.Get(3031).value;
		//this.criticalK2 = category.Get(3032).value;
		////this.criticalBaseHurt = category.Get(3013).value;

		//this.hp_than_extradmgRate = category.Get(3034).value;
		//this.lv_less_dmg = category.Get(3035).value;
		//this.distance_less_dmg = category.Get(3036).value;

		BindNumeric(numeric);
	}

	private void BindNumeric(Numeric numeric)
	{
		this.numeric = numeric;
	}

	public void Set(NumericType key, int value)
	{
		this.numeric.Set(key, value);
	}

	public void Clear()
	{
		this.numeric.Clear();
	}

	public static NumericComponent operator +(NumericComponent numeric1, NumericComponent numeric2)
	{
		if (numeric2 != null)
		{
			numeric1.numeric += numeric2.numeric;
		}

		return numeric1;
	}
	public static NumericComponent operator -(NumericComponent numeric1, NumericComponent numeric2)
	{
		if (numeric2 != null)
		{
			numeric1.numeric -= numeric2.numeric;
		}
		return numeric1;
	}

}

