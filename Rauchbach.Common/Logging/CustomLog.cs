using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Rauchbach.Common.Logging
{
    public class CustomLog : CustomLogFactory
    {
        public const string Begin = "Begin";
        public const string LineMarker = "Line";
        public const string Finish = "Finish";


        public ILogger ILogger { get; set; }

        public CustomLog(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            ILoggerFactory = loggerFactory;
        }
        public CustomLog(CustomLogFactory customLogFactory) : base(customLogFactory.ILoggerFactory)
        {
            IDs = customLogFactory.IDs;
            ILoggerFactory = customLogFactory.ILoggerFactory;
        }
    }

    public class CustomLogFactory
    {
        #region Properties

        public CustomLogVault IDs { get; set; }
        public ILoggerFactory ILoggerFactory { get; set; }
        #endregion

        #region Constructor

        public CustomLogFactory(ILoggerFactory loggerFactory)
        {
            ILoggerFactory = loggerFactory;
            IDs = new CustomLogVault();
        }
        #endregion


        public CustomLog CreateLogger<T>() where T : class
        {
            CustomLog log = new CustomLog(this);
            log.ILogger = ILoggerFactory.CreateLogger<T>();
            return log;
        }

        //Add Scoped Identifiers, that will be preserved in all Logs (prevents duplication)
        public void AddID(string key, object value)
        {
            if (!IDs.Keys.Any(x => x.Item1 == key))
            {
                IDs.Keys.Add((key, value));
            }
        }
    }

    public class CustomLogVault
    {
        public List<(string, object)> Keys { get; set; } = new List<(string, object)>();
    }

    public static class CustomLogExtension
    {
        public static void LogCustom(this CustomLog log,
                                        LogLevel logLevel,
                                        EventId? eventId = null,
                                        Exception exception = null,
                                        string message = null,
                                        [CallerMemberName] string memberName = "",
                                        [CallerLineNumber] int sourceLineNumber = 0,
                                        params ValueTuple<string, object>[] args)
        {
            if (log.ILogger.IsEnabled(logLevel))
            {
                message ??= CustomLog.LineMarker;

                List<(string, object)> temp = args.ToList();
                foreach ((string, object) item in log.IDs.Keys)
                {
                    if (!temp.Any(x => x.Item1 == item.Item1))
                    {
                        temp.Add((item.Item1, item.Item2));
                    }
                }

                CustomLogData customLogData = new CustomLogData(message, memberName, sourceLineNumber, temp.ToArray());

                string data = JsonConvert.SerializeObject(customLogData);

                if (exception is null)
                {
                    if (eventId is null)
                    {
                        log.ILogger.Log(logLevel, "{data}", data);
                    }
                    else
                    {
                        log.ILogger.Log(logLevel, eventId.Value, "{data}", data);
                    }
                }
                else
                {
                    if (eventId is null)
                    {
                        log.ILogger.Log(logLevel, exception, "{data}", data);
                    }
                    else
                    {
                        log.ILogger.Log(logLevel, eventId.Value, exception, "{data}", data);
                    }
                }
            }
        }

        #region Helper

        protected class CustomLogData
        {
            public CustomLogData(string message,
                                 string method,
                                 int line,
                                 params ValueTuple<string, object>[] values)
            {
                Method = method;
                Line = line;
                Message = message;

                if (values.Length > 0)
                {
                    Values = GetDynamicObject(values.ToDictionary(x => x.Item1, x => x.Item2));
                }
            }

            public string Method { get; }
            public int Line { get; }
            public string Message { get; }

            public dynamic Values { get; }
        }


        private static dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new MyDynObject(properties);
        }

        private sealed class MyDynObject : DynamicObject
        {
            private readonly Dictionary<string, object> _properties;

            public MyDynObject(Dictionary<string, object> properties)
            {
                _properties = properties;
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return _properties.Keys;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    result = _properties[binder.Name];
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    _properties[binder.Name] = value;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
