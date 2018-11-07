using System;
using System.Globalization;
using System.IO;
using log4net.Core;
using log4net.Layout;
using System.Net;
using System.Net.Sockets;

namespace log4net.Kafka
{
    public class LogstashLayout : LayoutSkeleton
    {
        public string Service { get; set; }
        public LogstashLayout()
        {
            IgnoresException = false;
        }
        public override void ActivateOptions()
        {

        }

        public override string ContentType
        {
            get { return "application/json"; }
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var evt = GetJsonObject(loggingEvent);

            var message = evt.ToJson();

            writer.Write(message);
        }
        private LogstashEvent GetJsonObject(LoggingEvent loggingEvent)
        {
            var obj = new LogstashEvent
            {
                version = 1,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                service = Service,
                source_host = Environment.MachineName,
                thread_name = loggingEvent.ThreadName,
                user = loggingEvent.UserName,
                ip = GetLocalIPAddress(),
                @class = loggingEvent.LocationInformation.ClassName,
                method = loggingEvent.LocationInformation.MethodName,
                line_number = loggingEvent.LocationInformation.LineNumber,
                level = loggingEvent.Level.ToString(),
                logger_name = loggingEvent.LoggerName,
                msg = loggingEvent.RenderedMessage
            };

            if (loggingEvent.ExceptionObject != null)
            {
                obj.exception = new LogstashException
                {
                    exception_class = loggingEvent.ExceptionObject.GetType().ToString(),
                    exception_message = loggingEvent.ExceptionObject.Message,
                    stacktrace = loggingEvent.ExceptionObject.StackTrace
                };
            }
            return obj;
        }

        public string GetLocalIPAddress()
        {
            var addressList = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (var ip in addressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }

}