using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuant
{
    [Serializable()] //クラスがシリアル化可能であることを示す属性
    public class RuntimeException : Exception
    {
        public string Command { get; }
        public int ExitCode { get; }
        public string StandardOutput { get; }
        public string StandardError { get; }

        public RuntimeException()
            : base()
        {
        }

        public RuntimeException(string message)
            : base(message)
        {
        }

        public RuntimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RuntimeException(string message, string command, int exitcode, string stdout, string stderr):base(message)
        {
            Command = command;
            ExitCode = exitcode;
            StandardOutput = stdout;
            StandardError = stderr;
        }


        //逆シリアル化コンストラクタ。このクラスの逆シリアル化のために必須。
        //アクセス修飾子をpublicにしないこと！（詳細は後述）
        protected RuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
