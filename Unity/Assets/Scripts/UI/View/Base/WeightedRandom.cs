using System.Collections.Generic;

class WeightedRandom<T>
{
	private struct Entry
	{
		public double accumulatedWeight;
		public T item;
	}

	private List<Entry> entries = new List<Entry>();
	private double accumulatedWeight;
	private System.Random rand = new System.Random();

	public void AddEntry(T item, double weight)
	{
		accumulatedWeight += weight;
		entries.Add(new Entry { item = item, accumulatedWeight = accumulatedWeight });
	}

	public T GetRandom()
	{
		double r = rand.NextDouble() * accumulatedWeight;
		foreach (Entry entry in entries)
		{
			if (entry.accumulatedWeight >= r)
			{
				return entry.item;
			}
		}
		return default(T); //should only happen when there are no entries
	}

	public void Clear()
	{
		entries.Clear();
	}
}
