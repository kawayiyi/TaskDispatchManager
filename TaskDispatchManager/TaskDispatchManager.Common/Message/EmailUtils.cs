using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Security.Permissions;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// 邮件操作类
    /// </summary>
    /// <remarks>冯瑞 2012-03-23</remarks>
    public class EmailUtils
    {
        /*
         *     用法：
        * 
                //邮件对象
                MailMessage mm = new MailMessage();
                //邮件内容；
                mm.Body = "<p>测试</p>";
                //邮件内容正文编码
                mm.BodyEncoding = System.Text.Encoding.UTF8;
                //发件人邮箱地址
                mm.From = new MailAddress("drjworld@qq.com");
                //正文是否可以使用HTML格式
                mm.IsBodyHtml = true;
                //回复地址和发件人地址应该是一样的。
                mm.ReplyTo = new MailAddress("drjworld@qq.com");
                //邮件标题
                mm.Subject = "邮件标题";
                //指定邮件标题编码格式
                mm.SubjectEncoding = System.Text.Encoding.UTF8;
                //收件人地址集合，可以群发
                mm.To.Add(new MailAddress("durongjian@gmail.com"));
                //电子邮件发件人地址
                mm.Sender = new MailAddress("drjworld@qq.com");
                //添加邮件附件
                Attachment att = new Attachment(@"C:\Users\admin\Desktop\test.jpg");
                mm.Attachments.Add(att);
                //简单邮件传输协议对象
                SmtpClient client = new SmtpClient();
                // 电子邮件通过网络发送
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //设置通信服务器
                client.Host = "smtp.qq.com";
                //用于验证发件人身份凭证。
                client.Credentials = new System.Net.NetworkCredential("address", "password");
                //发送邮件
                client.Send(mm);
         * 
        */

        #region 属性

        private bool mailSendResult;
        /// <summary>
        /// 异步邮件发送结果(只读)
        /// </summary>
        public bool MailSendResult
        {
            get { return mailSendResult; }
        }

        /// <summary>
        /// 发件人Email
        /// </summary>
        public string PosterMail { get; set; }
        /// <summary>
        /// 发件人Email密码
        /// </summary>
        public string PosterPwd { get; set; }
        /// <summary>
        /// 发件Email的SMTP服务器地址
        /// </summary>
        public string PosterSmtpHost { get; set; }
        /// <summary>
        /// 邮件正文的编码格式
        /// </summary>
        public System.Text.Encoding BodyEncoding { get; set; }
        /// <summary>
        /// 正文是否可以使用HTML格式
        /// </summary>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// 回复地址（回复地址和发件人地址应该是一样的）
        /// </summary>
        public string ReplyTo { get; set; }
        /// <summary>
        /// 电子邮件发送渠道
        /// </summary>
        public SmtpDeliveryMethod TheDeliveryMethod { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 默认发件账户信息
        /// </summary>
        public EmailUtils()
        {
            //设置默认发件人Email
            this.PosterMail = string.Empty;
            //设置默认发件人Email密码
            this.PosterPwd = string.Empty;
            //设置默认发件人Email的SMTP邮件服务器
            this.PosterSmtpHost = string.Empty;
            //邮件文本内容默认采用UTF8格式
            this.BodyEncoding = System.Text.Encoding.UTF8;
            //默认允许邮件内容含有HTML内容
            this.IsBodyHtml = true;
            //设置回复Email账户（默认等于发件人Email）
            this.ReplyTo = this.PosterMail;
            //电子邮件发送渠道（通过网络发送）
            this.TheDeliveryMethod = SmtpDeliveryMethod.Network;
        }

        /// <summary>
        /// 设置发件账户信息
        /// </summary>
        /// <param name="posterMail">发件人Email</param>
        /// <param name="posterPwd">发件人Email密码</param>
        /// <param name="posterSmtpHost">发件Email的SMTP地址</param>
        public EmailUtils(string posterMail, string posterPwd, string posterSmtpHost)
        {
            this.PosterMail = posterMail;
            this.PosterPwd = posterPwd;
            this.PosterSmtpHost = posterSmtpHost;
            //邮件文本内容默认采用UTF8格式
            this.BodyEncoding = System.Text.Encoding.UTF8;
            //默认允许邮件内容含有HTML内容
            this.IsBodyHtml = true;
            //设置回复Email账户（默认等于发件人Email）
            this.ReplyTo = this.PosterMail;
            //电子邮件发送渠道（通过网络发送）
            this.TheDeliveryMethod = SmtpDeliveryMethod.Network;
        }

        /// <summary>
        /// 设置发件账户信息
        /// </summary>
        /// <param name="posterMail">发件人Email</param>
        /// <param name="posterPwd">发件人Email密码</param>
        /// <param name="posterSmtpHost">发件Email的SMTP地址</param>
        /// <param name="bodyEncoding">邮件内容编码格式</param>
        /// <param name="isBodyHtml">邮件内容是否允许含有HTML代码</param>
        /// <param name="replyTo">回复邮件地址</param>
        /// <param name="theDeliveryMethod">邮件发送渠道</param>
        public EmailUtils(string posterMail, string posterPwd, string posterSmtpHost, System.Text.Encoding bodyEncoding, bool isBodyHtml, string replyTo, SmtpDeliveryMethod theDeliveryMethod)
        {
            this.PosterMail = posterMail;
            this.PosterPwd = posterPwd;
            this.PosterSmtpHost = posterSmtpHost;
            //邮件内容编码格式
            this.BodyEncoding = bodyEncoding;
            //是否允许包含HTML代码
            this.IsBodyHtml = isBodyHtml;
            //设置回复Email账户
            this.ReplyTo = replyTo;
            //电子邮件发送渠道
            this.TheDeliveryMethod = theDeliveryMethod;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="mailTitle">邮件标题</param>
        /// <param name="mailBody">邮件内容</param>
        /// <param name="toEmail">收件人列表</param>
        /// <param name="ccEmail">抄送</param>
        /// <param name="bccEmail">密送</param>
        /// <param name="async">是否异步发送</param>
        /// <returns></returns>
        public async Task PostEmail(string mailTitle, string mailBody, List<string> toEmail, List<string> ccEmail = null, List<string> bccEmail = null, bool async = true)
        {
            await PostEmailHaveAttachment(mailTitle, mailBody, toEmail, ccEmail, bccEmail, null, async);
        }

        /// <summary>
        /// 发送电子邮件（包含附件）
        /// </summary>
        /// <param name="mailTitle">邮件标题</param>
        /// <param name="mailBody">邮件内容</param>
        /// <param name="toEmail">收件箱列表</param>
        /// <param name="ccEmail">抄送</param>
        /// <param name="bccEmail">密送</param>
        /// <param name="attachment">附件列表（e.g.　System.Net.Mail.Attachment[] attachment = { new System.Net.Mail.Attachment(@"C:\Users\admin\Desktop\test.jpg") } ）</param>
        /// <param name="async">是否异步发送</param>
        /// <returns></returns>
        public async Task PostEmailHaveAttachment(string mailTitle, string mailBody, List<string> toEmail, List<string> ccEmail = null, List<string> bccEmail = null, List<Attachment> attachment = null, bool async = true)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(this.PosterMail);
            //mailMsg.From = new MailAddress(this.PosterMail,"service");
            mailMsg.IsBodyHtml = this.IsBodyHtml;
            mailMsg.BodyEncoding = this.BodyEncoding;
            //mailMsg.ReplyTo = this.ReplyTo;


            //添加收件人（可群发）
            foreach (var item in toEmail)
            {
                mailMsg.To.Add(new MailAddress(item.ToString()));
            }

            if (null != ccEmail)
            {
                //添加抄送
                foreach (var item in ccEmail)
                {
                    mailMsg.CC.Add(item);
                }
            }

            if (null != bccEmail)
            {
                //添加密送
                foreach (var item in bccEmail)
                {
                    mailMsg.Bcc.Add(item);
                }
            }

            //邮件标题
            mailMsg.Subject = mailTitle;
            //邮件内容
            mailMsg.Body = mailBody;

            if (null != attachment)
            {
                //添加附件
                foreach (var item in attachment)
                {
                    mailMsg.Attachments.Add(item);
                }
            }

            //简单邮件传输协议对象
            SmtpClient smtpClient = new SmtpClient();

            //电子邮件发送渠道
            smtpClient.DeliveryMethod = this.TheDeliveryMethod;
            smtpClient.Host = this.PosterSmtpHost;

            //发送Gmail邮件时，要加上下面两行代码，否则不成功
            //smtpClient.Port = 587;
            //smtpClient.EnableSsl = true;

            smtpClient.Credentials = new NetworkCredential(this.PosterMail, this.PosterPwd);

            if (async) //异步发送
            {
                //异步发送完成获取发送状态 
                smtpClient.SendCompleted += new SendCompletedEventHandler(sendCompletedCallback);
                //异步发送 
                //smtpClient.SendAsync(mailMsg, String.Empty);
                await smtpClient.SendMailAsync(mailMsg);
            }
            else //同步发送
            {
                smtpClient.Send(mailMsg);
            }
        }

        #endregion

        #region private

        /// <summary>
        /// 获取异步发送结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                mailSendResult = false;
            }
            if (e.Error != null)
            {
                mailSendResult = false;
            }
            else
            {
                mailSendResult = true;
            }
        }

        #endregion
    }
}
