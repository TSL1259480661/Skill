
using System;

/// <summary>
/// LCG 算法: 使用线性同余生成器 (LCG) 算法生成随机数。这种算法是最简单的伪随机数生成算法之一。
/// 公式为 X_{n+1} = (a * X_n + c) % m，其中 a, c 和 m 是常数。
/// </summary>
public class RandomLCG
{
	private const long a = 6364136223846793005;
	private const long c = 1;
	private const long m = (1L << 32);
	private long seed;

	public RandomLCG(long seed)
	{
		this.seed = seed;
	}

	public int Next()
	{
		seed = (a * seed + c) % m;
		return (int)(seed & 0x7FFFFFFF); // 返回正数
	}
	public int Next(int maxValue)
	{
		if (maxValue <= 0)
			throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue must be greater than 0");
		return Next() % maxValue;
	}

	public void SetSeed(long seed)
	{
		this.seed = seed;
	}
}
