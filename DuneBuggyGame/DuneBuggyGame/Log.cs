using System;
using System.IO;
using System.Text;
using System.Threading;

namespace DuneBuggyGame
{
    /// <summary>
    /// <para>
    /// The log class provides a simple yet powerful framework to write 
    /// logs to disk. The main use for this class is its ability to 
    /// write exceptions to disk in a readable format. It also
    /// provides a way to write ordinary logs. All log files are written
    /// with a timestamp in the file name to make sorting easier.
    /// </para><para>
    /// This class can be used either as a static class (using the 
    /// "Instance" property), or as an instatiated class, which provides 
    /// more control over where logs can be written to, as well as 
    /// the log file name.
    /// </para>
    /// </summary>
    public class Log
    {
        #region Default Constants

        /// <summary>
        /// The Newline constant. Refers to "Environment.NewLine"
        /// </summary>
        protected string _N = Environment.NewLine;

        /// <summary>
        /// The location on disk which logs are written out to.
        /// </summary>
        /// <remarks>
        /// In a production environment this could be pulled from
        /// the web.config file or from the directory the program
        /// is being run from, however for debug purposes in visual
        /// studio, this is hard-coded because web projects, when
        /// being debugged, are not run where you'd expect them to
        /// run thanks to the virtual server visual studio starts up.
        /// </remarks>
        protected const string _DefaultLogDirectory = @"C:\DuneBuggyLogs\";

        /// <summary>
        /// The name of an Error log file. This string contains a 
        /// formatting placeholder where the timestamp will be
        /// added to the file name. This formatting placeholder is 
        /// used by the .NET Framework's string formatting API to
        /// add the timestamp to the string very easily.
        /// </summary>
        protected const string _DefaultErrorLogFileName = "Error-{0}.txt";
        /// <summary>
        /// The name of a Log file. This string contains a formatting
        /// placeholder where the timestamp will be added to the file
        /// name. This formatting placeholder is used by the .NET 
        /// Framework's string formatting API to add the timestamp to 
        /// the string very easily.
        /// </summary>
        protected const string _DefaultActivityLogFileName = "Log-{0}.txt";

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes out an exception message to a log file called "Error-TIMESTAMP.txt".
        /// The string representation of the exception is written to the log file, as well
        /// as the stack trace, and any inner exception information (if it exists). An
        /// overloaded method exists which allows additional information to be added to the
        /// log if it could assist in resolving the exception.
        /// </summary>
        /// <remarks>
        /// This Error() was created to make error logging simple and easy to deal with.
        /// There is no need to format exceptions every time an error occurs, just pass in
        /// a caught exception and the exception is formatted nicely in a text file with a
        /// timestamp. An overload exists to provide additional information for an exception.
        /// </remarks>
        /// <param name="e">An exception to write to a log.</param>
        public void Write(Exception e)
        {
            Write(e, null, _DefaultLogDirectory);
        }

        /// <summary>
        /// Writes out an exception message to a log file called "Error-TIMESTAMP.txt".
        /// The string representation of the exception is written to the log file, as well
        /// as the stack trace, and any inner exception information (if it exists). This
        /// method overload allows for additional information to be sent to the method
        /// if it could assist in the resolving of an error.
        /// </summary>
        /// <param name="e">An exception to write to a log.</param>
        /// <param name="additionalInfo">Any additional information that may help resolve a problem if one occurs.</param>
        /// <param name="directory">
        /// The directory to write the log to. If the directory 
        /// does not exist, it will be created if the application 
        /// has priveledges to do so.
        /// </param>
        public void Write(Exception e, string additionalInfo, string directory)
        {
            Write(e, additionalInfo, directory, _DefaultErrorLogFileName);
        }

        /// <summary>
        /// Writes out an exception message to a log file with a given file name.
        /// The string representation of the exception is written to the log file, as well
        /// as the stack trace, and any inner exception information (if it exists). This
        /// method overload allows for additional information to be sent to the method
        /// if it could assist in the resolving of an error.
        /// </summary>
        /// <param name="e">An exception to write to a log.</param>
        /// <param name="additionalInfo">Any additional information that may help resolve a problem if one occurs.</param>
        /// <param name="directory">
        /// The directory to write the log to. If the directory 
        /// does not exist, it will be created if the application 
        /// has priveledges to do so.
        /// </param>
        /// <param name="fileName">
        /// The name to given to the log file. This must be a string
        /// using Composite Formatting. This is because the timestamp
        /// is added to the string and must be added at {0}.
        /// To see more about how to format a composite formatted string
        /// go to microsoft's article at this URL: 
        /// http://msdn2.microsoft.com/en-us/library/txafckwd.aspx
        /// </param>
        public void Write(Exception e, string additionalInfo, string directory, string fileName)
        {
            string errorMsg = _N + _N + "EXCEPTION:" + _N + e.ToString() + _N + _N + _N + "STACK TRACE:" + _N + e.StackTrace;

            // Traverse through any inner exceptions (if they exist)
            int innerExceptionCount = 0;
            while (e.InnerException != null)
            {
                ++innerExceptionCount;
                errorMsg += _N + _N + _N + "INNER EXCEPTION " + innerExceptionCount.ToString() + ":" + _N + e.InnerException.ToString() + _N + _N + _N + "STACK TRACE:" + _N + e.InnerException.StackTrace;
                e = e.InnerException;
            }

            if (additionalInfo != null && additionalInfo != string.Empty)
                errorMsg += _N + _N + "ADDITIONAL INFO:" + _N + additionalInfo;

            Write(errorMsg, directory, fileName);
        }

        /// <summary>
        /// Writes out a message to a log directory defined
        /// by a private constant within the Log class. The
        /// log file will contain a timestamp in the file name
        /// indicating when the log was created, down to the 
        /// millisecond. This method overload will write a
        /// log file of type "Log" by default.
        /// </summary>
        /// <param name="message">A message to write to the log file.</param>
        public void Write(string message)
        {
            Write(message, LogType.Log);
        }

        /// <summary>
        /// Writes out a message to a log directory defined
        /// by a private constant within the Log class. The
        /// log file will contain a timestamp in the file name
        /// indicating when the log was created, down to the 
        /// millisecond.
        /// </summary>
        /// <param name="message">A message to write to the log file.</param>
        /// <param name="logType">
        /// <para>
        /// The type of log to created. Depending on the log 
        /// being created, the filename will change slightly.
        /// </para><para>
        /// A LogType of Error will create a log file called 
        /// "Error" with the timestamp in the file name.
        /// </para><para>
        /// A LogType of Log will create a log file called 
        /// "Log" with the timestamp in the file name.
        /// </para>
        /// </param>
        public void Write(string message, LogType logType)
        {
            Write(message, LogType.Log, _DefaultLogDirectory);
        }


        /// <summary>
        /// Writes out a message to a log directory defined
        /// by a private constant within the Log class. The
        /// log file will contain a timestamp in the file name
        /// indicating when the log was created, down to the 
        /// millisecond.
        /// </summary>
        /// <param name="message">A message to write to the log file.</param>
        /// <param name="logType">
        /// <para>
        /// The type of log to created. Depending on the log 
        /// being created, the filename will change slightly.
        /// </para><para>
        /// A LogType of Error will create a log file called 
        /// "Error" with the timestamp in the file name.
        /// </para><para>
        /// A LogType of Log will create a log file called 
        /// "Log" with the timestamp in the file name.
        /// </para>
        /// </param>
        /// <param name="logDirectory">
        /// The location to save the log file. If this directory 
        /// does not exist, it will be created if the application
        /// has permission to create directories in that location.
        /// </param>
        public void Write(string message, LogType logType, string logDirectory)
        {
            if (logType == LogType.Error)
                Write(message, logDirectory, _DefaultErrorLogFileName);
            else
                Write(message, logDirectory, _DefaultActivityLogFileName);
        }

        /// <summary>
        /// Writes out a message to a log directory with the log
        /// file name in the file name. Because a timestamp is added
        /// to the file name, a formatting placeholder ({0}) needs to be
        /// in the string so that this method does not generate an error.
        /// </summary>
        /// <param name="message">A message to write to the log file.</param>
        /// <param name="logDirectory">
        /// The location to save the log file. If this directory 
        /// does not exist, it will be created if the application 
        /// has permission to create directories in that location.
        /// </param>
        /// <param name="logFileName">
        /// The name to given to the log file. This must be a string
        /// using Composite Formatting. This is because the timestamp
        /// is added to the string and must be added at {0}.
        /// To see more about how to format a composite formatted string
        /// go to microsoft's article at this URL: 
        /// http://msdn2.microsoft.com/en-us/library/txafckwd.aspx
        /// </param>
        public void Write(string message, string logDirectory, string logFileName)
        {
            try
            {
                // Create the name of the log file
                string logName = string.Format(logFileName, DateTime.Now.ToString("yyyy-MM-dd--HH.mm.ss.fffffff"));
                
                string logDir = logDirectory;

                // Check for the log directory and 
                // create it if it does not exist
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                // Create the log file stream
                StreamWriter sw = new StreamWriter(logDir + logName, true, Encoding.UTF8);

                // Write the message to the log stream
                sw.WriteLine(message);

                // Close and cleanup
                sw.Close();
                sw.Dispose();
            }
            catch (Exception e)
            {   // If the write process fails, keep trying just 
                // a few more times. Maybe it will work...
                if (new System.Diagnostics.StackTrace().FrameCount < 15)
                {
                    // Wait a bit before trying again
                    Thread.Sleep(100);
                    Write(message, logDirectory, logFileName);
                }
                else
                {
                    throw new Exception("An error occured while trying to write out a log file." 
                                            + _N + _N + message, e);
                }
            }
        }
        #endregion

        #region Static Quick Instance 

        private static Log _Log;

        /// <summary>
        /// Gets a quick instance of the log class that uses
        /// default settings for the log directory (C:\NileLogs)
        /// error logs, and activity logs which can be used
        /// without the need to instantiate a Log object.
        /// </summary>
        public static Log Instance
        {
            get
            {
                if (_Log == null)
                    _Log = new Log(_DefaultLogDirectory, _DefaultErrorLogFileName, _DefaultActivityLogFileName);
                return _Log;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for Log class.
        /// </summary>
        public Log() { }

        /// <summary>
        /// Constructor to set all properties of the Log class on instantiation.
        /// </summary>
        /// <param name="logDirectory"></param>
        /// <param name="errorLogFileName"></param>
        /// <param name="activityLogFileName"></param>
        public Log(string logDirectory, string errorLogFileName, string activityLogFileName)
        {
            LogDirectory = logDirectory;
            ErrorLogFileName = errorLogFileName;
            ActivityLogFileName = activityLogFileName;
        }

        #endregion

        #region Private Fields
        private string _LogDirectory = _DefaultLogDirectory;
        private string _ErrorLogFileName = _DefaultErrorLogFileName;
        private string _ActivityLogFileName = _DefaultActivityLogFileName;
        #endregion

        #region Public Properties
        /// <summary>
        /// The directory to save logs to. If this directory does not
        /// exist, an attempt will be made to create it.
        /// </summary>
        public string LogDirectory
        {
            get { return _LogDirectory; }
            set { _LogDirectory = value; }
        }

        /// <summary>
        /// The name given to an error log file. This must be a string
        /// using Composite Formatting. This is because the timestamp
        /// is added to the string and must be added at {0}.
        /// To see more about how to format a composite formatted string
        /// go to microsoft's article at this URL: 
        /// http://msdn2.microsoft.com/en-us/library/txafckwd.aspx
        /// </summary>
        public string ErrorLogFileName
        {
            get { return _ErrorLogFileName; }
            set { _ErrorLogFileName = value; }
        }

        /// <summary>
        /// The name given to a general log file. This must be a string
        /// using Composite Formatting. This is because the timestamp
        /// is added to the string and must be added at {0}.
        /// To see more about how to format a composite formatted string
        /// go to microsoft's article at this URL: 
        /// http://msdn2.microsoft.com/en-us/library/txafckwd.aspx
        /// </summary>
        public string ActivityLogFileName
        {
            get { return _ActivityLogFileName; }
            set { _ActivityLogFileName = value; }
        }
        #endregion
    }


    /// <summary>
    /// The LogType enumeration holds the different 
    /// types of logs which can be written. The
    /// LogTypes are used to indicate the file name
    /// to use when writing a log file to disk.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// An error log.
        /// </summary>
        Error,
        /// <summary>
        /// A general log that could contain anything
        /// </summary>
        Log
    }
}
