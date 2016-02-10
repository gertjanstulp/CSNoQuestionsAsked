/*
    The MIT License(MIT)

    Copyright(c) 2015 Dimitri Slappendel

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.

    https://github.com/Archomeda/csl-common-shared-library
*/

using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NoQuestionsAsked
{
    public static class ModLogger
    {
        private static bool _loaded;

        public static bool DebugLogging { get; set; }

        public static void ModLoaded()
        {
            if (!_loaded)
            {
                _loaded = true;
                clearLog();
            }
        }

        public static void Debug(string message)
        {
            if (DebugLogging)
            {
                // Debug messages are only written to the debug logfile to prevent excessive logging in the UI
                message = FormatMessage("Debug", message);
                LogToFile(message);
            }
        }

        public static void Debug(string message, params object[] args)
        {
            Debug(string.Format(message, args));
        }

        public static void Info(string message)
        {
            message = FormatMessage("Info", message);

            LogToUnity(UnityEngine.Debug.Log, message);
            LogToDebugOutputPanel(PluginManager.MessageType.Message, message);
            LogToFile(message);
        }

        public static void Info(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public static void Warning(string message)
        {
            message = FormatMessage("Warning", message);

            LogToUnity(UnityEngine.Debug.LogWarning, message);
            LogToDebugOutputPanel(PluginManager.MessageType.Warning, message);
            LogToFile(message);
        }

        public static void Warning(string format, params object[] args)
        {
            Warning(string.Format(format, args));
        }

        public static void Error(string message)
        {
            message = FormatMessage("Error", message);

            LogToUnity(UnityEngine.Debug.LogError, message);
            LogToDebugOutputPanel(PluginManager.MessageType.Error, message);
            LogToFile(message);
        }

        public static void Error(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        public static void Exception(Exception exception)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("An unexpected exception occured:");

            Exception currentException = exception;
            while (currentException != null)
            {
                message.AppendLine(exception.ToString());
                currentException = currentException.InnerException;
            }

            Error(message.ToString());
        }

        public static void DumpObject(object myObject)
        {
            StringBuilder objectDetails = new StringBuilder();
            objectDetails.AppendLine();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myObject))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(myObject);
                objectDetails.AppendLine(string.Format("{0}: {1}", name, value));
            }
            Debug(objectDetails.ToString());
        }

        private static void LogToUnity(Action<object> logFunc, string message)
        {
            logFunc(message);
        }

        private static void LogToDebugOutputPanel(PluginManager.MessageType messageType, string message)
        {
            DebugOutputPanel.AddMessage(messageType, message);
        }

        private static void LogToFile(string message)
        {
            string logFileName = ModPaths.GetLogFilePath();
            File.AppendAllText(logFileName, message);
            File.AppendAllText(logFileName, Environment.NewLine);
        }

        private static string FormatMessage(string type, string message)
        {
            return string.Format("[NoQuestionsAsked] - {0} {1} - {2}", type, DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff"), message);
        }

        private static void clearLog()
        {
            string fileName = ModPaths.GetLogFilePath();
            if (File.Exists(fileName))
                File.WriteAllText(fileName, string.Empty);
        }
    }
}
