using App;
using System;

public class LinkedItemNode : IObjectPoolItem
{
	public LinkedItemNode Next { get; internal set; }
	public LinkedItemNode Prev { get; internal set; }

	public LinkedItemList linkedItemList;

	public int traversalIndex;

	private Action<LinkedItemNode> recycle;
	public void SetRecycle(Action<LinkedItemNode> recycle)
	{
		this.recycle = recycle;
	}

	public virtual void Recycle()
	{
		recycle?.Invoke(this);
	}

	public virtual void OnReuse()
	{
	}

	public virtual void OnRecycle()
	{
		linkedItemList?.Delink(this);

		recycle = null;

		Next = null;
		Prev = null;

		linkedItemList = null;
	}
}

public class LinkedItemMultyNode : LinkedItemNode
{
	public LinkedItemNode extraNode = new LinkedItemNode();
}

public class LinkedItemList : IObjectPoolItem
{
	private static UObjectPool<LinkedItemList> pool = new UObjectPool<LinkedItemList>(100000);

	public static LinkedItemList Get()
	{
		return pool.Get();
	}

	private static UDebugger debugger = new UDebugger("LinkedItemList");
	public int traversalIndex;

	private LinkedItemNode _first;
	private LinkedItemNode _last;

	public LinkedItemNode First
	{
		get
		{
			return _first.Next;
		}
	}

	public LinkedItemNode Last
	{
		get
		{
			return _last.Prev;
		}
	}

	public LinkedItemNode StaticLast
	{
		get
		{
			return _last;
		}
	}

	public int Count { get; private set; }

	public LinkedItemList()
	{
		_first = new LinkedItemNode();
		_last = new LinkedItemNode();

		_first.Next = _last;
		_last.Prev = _first;
	}

	public void AddFirst(LinkedItemNode newNode)
	{
		if (newNode != null)
		{
			newNode.linkedItemList = this;

			Count++;

			_first.Next.Prev = newNode;

			newNode.Next = _first.Next;
			newNode.Prev = null;
			_first.Next = newNode;
		}
	}

	public void AddLast(LinkedItemNode newNode)
	{
		if (newNode != null)
		{
			newNode.linkedItemList = this;

			Count++;

			_last.Prev.Next = newNode;

			newNode.Next = _last;
			newNode.Prev = _last.Prev;

			_last.Prev = newNode;
		}
	}

	public LinkedItemNode Delink(LinkedItemNode node)
	{
		if (node != null)
		{
			if (node.linkedItemList == this)
			{
				Count--;

				if (node.Prev != null)
				{
					node.Prev.Next = node.Next;

					if (node.Prev.Next == node)
					{
						debugger.LogError("node.Prev.Next == node");
					}
				}

				if (node.Next != null)
				{
					node.Next.Prev = node.Prev;

					if (node.Next.Prev == node)
					{
						debugger.LogError("node.Next.Prev == node");
					}
				}

				LinkedItemNode next = node.Next;

				node.Next = null;
				node.Prev = null;
				node.linkedItemList = null;

				return next;
			}
			else
			{
				debugger.LogError("DlinkNode is not from this LinkedItemList.");
			}
		}
		return null;
	}

	//public void ForEach(Action<LinkedItemNode> onEach, bool compareTraversalIndex = true)
	//{
	//	if (Count > 0)
	//	{
	//		LinkedItemNode current = _first.Next;
	//		while (current != null && current != _last)
	//		{
	//			if (!compareTraversalIndex || current.traversalIndex != traversalIndex)
	//			{
	//				current.traversalIndex = traversalIndex;
	//				onEach?.Invoke(current);
	//			}
	//			current = current.Next;
	//		}
	//	}
	//}

	public void ClearAllNodes()
	{
		LinkedItemNode current = _first.Next;
		while (current != null && current != _last)
		{
			var pre = current;
			current = current.Next;
			pre.Recycle();
		}
	}

	public void Clear(bool autoClearNodes = false)
	{
		if (autoClearNodes && Count > 0)
		{
			ClearAllNodes();
		}

		Count = 0;

		_first.Prev = null;
		_last.Next = null;

		_first.Next = _last;
		_last.Prev = _first;
	}

	public void Recycle()
	{
		pool.Recycle(this);
	}

	public void OnReuse()
	{
	}

	public void OnRecycle()
	{
		Clear(false);
	}
}
