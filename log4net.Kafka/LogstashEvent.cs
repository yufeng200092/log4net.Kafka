﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log4net.Kafka
{
    /// <summary>
    /// logstash format
    /// </summary>
    public class LogstashEvent
    {
        public int version { get; set; }
        public string timestamp { get; set; }
        public string service { get; set; }
        public string source_host { get; set; }
        public string thread_name { get; set; }
        public string @class { get; set; }
        public string method { get; set; }
        public string line_number { get; set; }
        public string level { get; set; }
        public string logger_name { get; set; }
        public string msg { get; set; }
        public LogstashException exception { get; set; }
        public string user { get; set; }
        public string ip { get; set; }
    }
    public class LogstashException
    {
        public string exception_class { get; set; }
        public string exception_message { get; set; }
        public string stacktrace { get; set; }
    }


}
