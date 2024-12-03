

public interface IMessage
{
}


//请求协议
public interface IRequest : IMessage
{
	string url { get; }
}

//响应协议
public interface IResponse : IMessage
{
}

