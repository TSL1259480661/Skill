using App;
using ClientData;
using ET;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UIEventType;
using UnityEngine;
using UnityEngine.Networking;


public interface IHttpNetwork
{
	//public void Send(IRequest request);

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">响应消息类</typeparam>
	/// <param name="request">请求消息</param>
	/// <param name="succeedCallback">成功回调和响应消息</param>
	/// <param name="failCallback">失败回调和错误代码</param>
	public void Call<T>(IRequest request, Action<T> succeedCallback, Action<int, string> failCallback = null) where T : IMessage;
}

/*
	Get请求通过URL传递数据的格式：URL中请求的文件名后跟着“?”；多组键值对，键值对之间用“&”进行分割；
URL中包含汉字、特殊符号、需要对这些字符进行编码。

*/
public class HttpClient : MonoBehaviour, IHttpNetwork
{
	private static UDebugger debugger = new UDebugger("HttpClient");
	App.EventSystem eventSystem;

	public class HttpSession
	{
		public static int sid = 0;

		public int id;
		public bool showLoad;//是否显示网络加载中动画
		public bool queue;//是否排队(发送时没有排队)
		public long sendTime;//发送时间
		public long recvTime;//接收时间
		public bool reply;//是否接收完成

		public string name;//协议名称
		public string url;//协议地址
		public IMessage requestData; //请求对象
		public IMessage responseData;//返回对象
		public int errorCode;//错误码
		public string errorMsg;//错误信息
		public Action responseCallBack;//回调
	}
	static List<HttpSession> sessions = new List<HttpSession>();


	public long ServerTime
	{
		get;
		private set;
	}

	public long LastSendTime
	{
		get;
		private set;
	}
	public long LastRecvTime
	{
		get;
		private set;
	}

	/// <summary>
	/// 账号Token
	/// </summary>
	public string Token
	{
		get;
		set;
	}


	#region
	/// <summary>
	/// 临时用的
	/// </summary>
	public static bool isLocal = false;
	public Dictionary<Type, IResponse> localResponeseMap = new Dictionary<Type, IResponse>();
	#endregion

	public void Init(App.EventSystem eventSystem)
	{
		this.eventSystem = eventSystem;

		//map.Add(typeof(BadgeInfoResp), new BadgeInfoResp());
		localResponeseMap.Add(typeof(UserLoginResp), new UserLoginResp() { });
		localResponeseMap.Add(typeof(FinishCombatResp), new FinishCombatResp() { });
		localResponeseMap.Add(typeof(GetRoleItemsResp), new GetRoleItemsResp() { });
		//map.Add(typeof(SetGuidePointResp), new SetGuidePointResp() { success = true });
		//localResponeseMap.Add(typeof(GetRoleInfoResp), new GetRoleInfoResp() { baseInfo = new RoleDto() { nickname = "假名字" } });
		localResponeseMap.Add(typeof(StartCombatResp), new StartCombatResp() { combatUid = "123", costs = new List<RoleItemChangeDto>() });
		//map.Add(typeof(BeginnerGiftInfoResp), new BeginnerGiftInfoResp() { });
	}

	public int ParseHttpErrorCode(string httpError)
	{
		var match = Regex.Match(httpError, @"HTTP/1.1 (\d+) ");
		if (match.Success)
		{
			if (int.TryParse(match.Groups[1].Value, out int statusCode))
			{
				return statusCode;
			}
		}
		return -1; // 返回-1表示解析失败
	}

	#region 资源下载

	/// <summary>
	/// 下载图片资源
	/// </summary>
	private IEnumerator LoadTextureByURL(string url, Action<Texture2D> action)
	{
		using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
		{
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.Success)
			{
				Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				if (texture != null)
				{
					action?.Invoke(texture);
				}
				else
				{
					debugger.LogError("Failed to download image data");
					yield break;
				}
			}
			else
			{
				debugger.LogError("Failed to download image: " + www.error);
				yield break;
			}
		}
	}

	/// <summary>
	/// 下载图片资源
	/// </summary>
	public void DownloadTextureByURL(string url, Action<Texture2D> action)
	{
		StartCoroutine(LoadTextureByURL(url, action));
	}

	#endregion

	#region ProtoBuf


	public void Call<T>(IRequest request, Action<T> succeedCallback, Action<int, string> failCallback = null) where T : IMessage
	{
		Call(request.url, true, true, request, succeedCallback, failCallback);
	}

	public void Call<T>(string protoName, bool showLoad, bool queue, IMessage request, Action<T> succeedCallback, Action<int, string> failCallback = null) where T : IMessage
	{
		#region TODO:待移除的代码
		if (isLocal)
		{
			Timer.Instance.Add(0.1f, 1, (timeid, obj) =>
			{
				IMessage data = default(T);
				if (localResponeseMap.ContainsKey(typeof(T)))
				{
					data = localResponeseMap[typeof(T)];
				}
				succeedCallback?.Invoke((T)data);
			});
			return;
		}

		#endregion


		foreach (var session in sessions)
		{
			if (session.name == protoName)
			{
				debugger.Log("Duplicate message：", protoName);
				return;
			}
		}

		// 获取当前协调世界时（UTC）时间的毫秒表示
		double millisecondsUtc = DateTime.UtcNow.TimeOfDay.TotalMilliseconds;
		LastSendTime = (long)millisecondsUtc;

		var sess = new HttpSession()
		{
			id = HttpSession.sid++,
			name = protoName,
			url = ProtocolAddressFunction.ParseProto(protoName),
			requestData = request,
			reply = false,
			showLoad = showLoad,
			queue = queue,
			sendTime = LastSendTime,
		};
		sess.responseCallBack = () =>
		{
			if (sess.errorCode != 0)
			{
				failCallback?.Invoke(sess.errorCode, sess.errorMsg);
			}
			else
			{
				succeedCallback?.Invoke((T)sess.responseData);
			}
		};
		if (sess.queue)
		{
			sessions.Add(sess);
		}

		//eventSystem.Dispatch<UIEventType.NetRequest>(new UIEventType.NetRequest()
		//{
		//	session = sess
		//});
		StartCoroutine(HttpRequest<T>(sess));
	}

	static StringBuilder tempstring = new StringBuilder(200);

	private IEnumerator HttpRequest<T>(HttpSession session) where T : IMessage
	{
		tempstring.Clear();
		tempstring.Append("Request name:");
		tempstring.AppendLine(session.requestData.GetType().Name);
		byte[] data = Serialize(session.requestData);

		using (UnityWebRequest www = UnityWebRequest.PostWwwForm(session.url, ""))
		{
			var uploadHandler = new UploadHandlerRaw(data);
			www.uploadHandler = uploadHandler;
			//www.uploadHandler.contentType = "application/json";
			www.SetRequestHeader("Content-Type", "application/x-protobuf");
			//设置`Authorization`，除登录接口以外，服务器API基本都需要填写此消息头证明请求者的玩家身份
			if (!string.IsNullOrEmpty(Token))
				www.SetRequestHeader("Authorization", "Bearer " + Token);
			www.timeout = 10;

			//输出请求的URL
			tempstring.Append("Request URL:");
			tempstring.AppendLine(www.url);
			//输出请求的方法（GET、POST等）
			//tempstring.Append("Request Method:");
			//tempstring.AppendLine(www.method);
			//输出请求头
			tempstring.Append("Request Headers:");
			tempstring.AppendLine(www.GetRequestHeader("headerName"));
			//输出请求体
			tempstring.Append("Request Body:");
			if (www.uploadHandler.data != null)
				tempstring.AppendLine(session.requestData.ToString());
			else tempstring.AppendLine("null");
			string requestLog = tempstring.ToString();
			debugger.Log(requestLog);
			
			//发送请求
			yield return www.SendWebRequest();
			//yield return new WaitForSecondsRealtime(UnityEngine.Random.RandomRange(1, 9));

			//输出响应头
			tempstring.Clear();
			tempstring.Append("Response Headers:");
			tempstring.AppendLine(www.GetResponseHeader("headerName"));

			//输出响应体
			tempstring.Append("Response Body:");
			tempstring.AppendLine(www.downloadHandler.text);
			debugger.Log(requestLog, tempstring.ToString());

			IMessage responseData = null;
			int errorCode = -1;
			string errorMsg = "unknown";

			//4.处理响应
			if (www.result == UnityWebRequest.Result.Success || www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
			{
				try
				{
					var downloaddata = www.downloadHandler.data;
					var respone = HandleRecive<T>(downloaddata);
					responseData = respone.Item1;
					errorCode = respone.Item2;
					errorMsg = respone.Item3;
				}
				catch (Exception)
				{
					responseData = default(T);
					errorCode = ParseHttpErrorCode(www.error);
				}
			}
			else if (www.result == UnityWebRequest.Result.ConnectionError)
			{
				errorCode = -2;
				errorMsg = "Network connection error";
			}
			//5.错误处理
			if (errorCode != 0)
			{
				errorMsg = string.Format("Error Code:{0},{1}", errorCode, errorMsg);
				debugger.LogError("http request fail: ", session.requestData.GetType().Name, "(", session.url, ")", "-> ", errorMsg);
				//eventSystem.Dispatch<TIP>(new TIP() { tip = errorMsg, time = 1.5F });
				//if (www.result == UnityWebRequest.Result.ConnectionError)
				//{
				//	eventSystem.Dispatch<ConnectionError>(new ConnectionError());
				//}
			}
			Back(session, responseData, errorCode, errorMsg);
		}
	}

	void Back<T>(HttpSession backSession, T responeData, int errorCode, string errorMsg) where T : IMessage
	{
		LastRecvTime = (long)DateTime.Now.TimeOfDay.TotalMilliseconds;
		backSession.reply = true;
		backSession.recvTime = LastRecvTime;
		backSession.responseData = responeData;
		backSession.errorCode = errorCode;
		backSession.errorMsg = errorMsg;
		//eventSystem.Dispatch<UIEventType.NetResponse>(new UIEventType.NetResponse() { session = backSession });

		if (!backSession.queue)
		{
			try
			{
				backSession.responseCallBack.Invoke();
			}
			catch (Exception e)
			{
				debugger.LogError(e);
			}
			return;
		}
		int removeCount = 0;
		bool isbreak = false;
		for (int i = 0; i < sessions.Count; i++)
		{
			HttpSession session = sessions[i];
			if (session.reply && !isbreak)
			{
				removeCount++;
			}
			else
			{
				isbreak = true;
			}
		}

		for (int i = 0; i < removeCount; i++)
		{
			HttpSession session = sessions[0];
			sessions.RemoveAt(0);
			try
			{
				session.responseCallBack.Invoke();
			}
			catch (Exception e)
			{
				debugger.LogError(e);
			}
		}
	}

	private static (T, int, string) HandleRecive<T>(byte[] downloaddata) where T : IMessage
	{
		int erroeCode = -1;
		string erroeMsg = "unknown";
		if (downloaddata != null && downloaddata.Length != 0)
		{
			RawResponse respone = Deserialize<RawResponse>(downloaddata);
			erroeCode = respone.code;
			erroeMsg = respone.msg;
			if (respone.code == 0 && respone.data != null && respone.data.Length != 0)
			{
				T responeMsg = Deserialize<T>(respone.data);
				return (responeMsg, erroeCode, erroeMsg);
			}
		}
		return (default(T), erroeCode, erroeMsg);
	}

	static byte[] Serialize<T>(T Instance)
	{
		using (MemoryStream ms = new MemoryStream())
		{
			Serializer.Serialize(ms, Instance);
			return ms.ToArray();
		}
	}
	static T Deserialize<T>(byte[] data)
	{
		using (MemoryStream ms = new MemoryStream(data))
		{
			T responeMsg = Serializer.Deserialize<T>(ms);
			return responeMsg;
		}
	}

	#endregion
}
