using YooAsset;

/// <summary>
/// 资源文件查询服务类
/// </summary>
public class WebGLGameQueryServices : IBuildinQueryServices
{
	public bool Query(string packageName, string fileName, string fileCRC)
	{
		return true;
	}
}

public class RemoteServices : IRemoteServices
{
	private readonly string _defaultHostServer;
	private readonly string _fallbackHostServer;
	public RemoteServices(string defaultHostServer, string fallbackHostServer)
	{
		_defaultHostServer = defaultHostServer;
		_fallbackHostServer = fallbackHostServer;
	}

	public string GetRemoteFallbackURL(string fileName)
	{
		return $"{_defaultHostServer}/{fileName}";
	}

	public string GetRemoteMainURL(string fileName)
	{
		return $"{_fallbackHostServer}/{fileName}";
	}
}
