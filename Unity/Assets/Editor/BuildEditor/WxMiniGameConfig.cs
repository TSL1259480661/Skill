using WeChatWASM;

public class WxMiniGameConfig
{
	/// <summary>
	/// 游戏名
	/// </summary>
	public string Name = "TestGame";
	/// <summary>
	/// 小游戏APP ID
	/// </summary>
	public string AppId = "wx604a070a1f87cef1";
	/// <summary>
	/// 资源CD地址
	/// </summary>
	public string ResCdn = "https://bojun-res-public.oss-cn-chengdu.aliyuncs.com/cnd/webgl/";
	/// <summary>
	/// Unity预留内存
	/// </summary>
	public int UnityHeap = 256;
	/// <summary>
	/// 导出项目路径
	/// </summary>
	public string ExportPath = "E:/WorkSpace/UnitySpace/Unity/Publish/Wx";
	/// <summary>
	/// 首包加载方式
	/// </summary>
	public int FirstPackLoadType = 1;
	/// <summary>
	/// 是否清理StreamingAssets
	/// </summary>
	public bool IsClearStreamingAssets = true;
	/// <summary>
	/// 屏幕方向
	/// </summary>
	public WXScreenOritation ScreenOrientation = WXScreenOritation.Portrait;
}
