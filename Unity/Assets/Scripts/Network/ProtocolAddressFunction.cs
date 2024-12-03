
public static class ProtocolAddressFunction
{
	/// <summary>
	/// IP地址
	/// </summary>
	private static string Head = string.Empty;

	/// <summary>
	/// 外网端
	/// </summary>
	private static string Part = string.Empty;

	/// <summary>
	/// 切换IP
	/// </summary>
	/// <param name="ip"></param>
	public static void ChangeIP(string ip, string part)
	{
		Part = part;
		// Head = string.Format("http://{0}", ip);
		Head = ip;
	}

	/// <summary>
	/// 组装协议
	/// </summary>
	public static string ParseProto(string proto)
	{
	    if (string.IsNullOrEmpty(Part))
		{
			return string.Format("{0}/{1}", Head, proto);
		}
		else
		{
			return string.Format("{0}/{1}/{2}", Head, Part, proto);
		}
	}
}
