using DogSE.Server.Core.Net;
using DogSE.Server.Net;

namespace Example2
{
    public enum LoginResult
    {
        Success = 0,

        PwdError = 1,
    }

    public class MessagePackageManager
    {
        public static SR_Login ReadLogin(PacketReader reader)
        {
            var ret = new SR_Login();
            ret.UserName = reader.ReadUTF8String();
            ret.Pwd = reader.ReadUTF8String();

            return ret;
        }

        public static SR_SendMessage ReadSendMessage(PacketReader reader)
        {
            var ret = new SR_SendMessage();
            ret.Message = reader.ReadUTF8String();

            return ret;
        }
        public static SR_SendPrivateMessage ReadSendPrivateMessage(PacketReader reader)
        {
            var ret = new SR_SendPrivateMessage();
            ret.Name = reader.ReadUTF8String();
            ret.Message = reader.ReadUTF8String();

            return ret;
        }

        public static DogBuffer WriteLoginResult(LoginResult loginResult)
        {
            var writer = new PacketWriter();
            writer.SetNetCode((ushort)OpCode.LoginResult);
            writer.Write((int)loginResult);

            return writer.GetBuffer();
        }

        public static DogBuffer WriteSendMessageResult(string message)
        {
            var writer = new PacketWriter();
            writer.SetNetCode((ushort)OpCode.RecvMessage);
            writer.WriteUTF8Null(message);

            return writer.GetBuffer();
        }

        public static DogBuffer WriteSendPrivateMessageResult(string userName, string message)
        {
            var writer = new PacketWriter();
            writer.SetNetCode((ushort)OpCode.RecvPrivateMessage);
            writer.WriteUTF8Null(userName);
            writer.WriteUTF8Null(message);

            return writer.GetBuffer();
        }
    }


    public class SR_Login
    {
        public string UserName { get; set; }
        public string Pwd { get; set; }
    }

    public class SR_SendMessage
    {
        public string Message { get; set; }
    }
    public class SR_SendPrivateMessage
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }

}