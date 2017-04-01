using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Result
    {
        public Result()
        {
        }

        /// <summary>
        ///     初始化一个 业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        public Result(bool status)
        {
            this.status = status;
        }

        /// <summary>
        ///     初始化一个 定义返回消息的业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        /// <param name="message">业务返回消息</param>
        public Result(bool status, string message)
            : this(status)
        {
            this.message = message;
        }

        public Result(bool status, string message, object data)
            : this(status, message)
        {
            this.data = data;
        }

        /// <summary>
        ///     初始化一个 业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="code">业务操作结果类型</param>
        public Result(StatusCode code)
        {
            this.code = code;
        }

        /// <summary>
        ///     初始化一个 定义返回消息的业务操作结果信息类 的新实例
        /// </summary>
        /// <param name="resultType">业务操作结果类型</param>
        /// <param name="message">业务返回消息</param>
        public Result(StatusCode type, string message)
            : this(type)
        {
            this.message = message;
        }

        public Result(StatusCode type, string message, object data)
            : this(type, message)
        {
            this.data = data;
        }

        #region 属性

        /// <summary>
        ///     获取或设置 操作结果错误码
        /// </summary>
        public StatusCode code { get; set; }


        /// <summary>
        ///     获取或设置 数据结果状态
        /// </summary>
        public bool status { get; set; }

        /// <summary>
        ///     获取或设置 操作返回信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 成功可能时返回的数据
        /// </summary>
        public object data { get; set; }
        #endregion
    }
}
