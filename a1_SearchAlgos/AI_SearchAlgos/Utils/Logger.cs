using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace AI_SearchAlgos.Utils
{

    /// <summary>
    /// A Static Library that can be used for colour coded logging to the Console Window. 
    /// The library is thread safe and can be called without worry of interlaced output.
    /// Logs are processed and sent to three locations by defult:
    ///   o A Log file named with an appropriate date and .log extension in the Logs folder.
    ///   o The Console in a colour coded fashion.
    ///   o The Network for sending to the Data Center.
    /// Transmission over the network and to the console window are controlled with the
    /// NetworkReportLevel and ConsoleReportLevel properties respectively. 
    /// 
    /// Threading:
    /// Recently this logger has been enhanced with a working thread to allow it to be
    /// asynchronous and more easily handled at exit time. 
    /// </summary>
    public class Log
    {
        public enum LogLevel : byte
        {
            Raw = 0xff,
            Debug = 0x00,
            Status = 0x01,
            Information = 0x02,
            Warning = 0x03,
            Critical = 0x04,
            Success = 0x05
            
        }
        #region Properties

        private volatile LogLevel m_NetworkReportLevel;
        /// <summary>
        /// Network Report Level dictates which log entries are sent over
        /// the network to the Datacenter. All message with the level set
        /// or above will be sent.
        /// </summary>
        public static LogLevel NetworkReportLevel
        {
            get
            {
                return Instance.m_NetworkReportLevel;
            }
            set
            {
                Instance.m_NetworkReportLevel = value;
            }
        }

        private volatile LogLevel m_ConsoleReportLevel;
        /// <summary>
        /// Console Report Level dictates which log enties are printed to
        /// the console. All messages with the level set or above are printed
        /// to the console with colour coding.
        /// </summary>
        public static LogLevel ConsoleReportLevel
        {
            get
            {
                return Instance.m_ConsoleReportLevel;
            }
            set
            {
                Instance.m_ConsoleReportLevel = value;
            }
        }

        #endregion Properties

        #region Internal Members

        /// <summary>
        /// Our static library is really backed by a thread-safe singleton,
        /// here is our instance for that singleton.
        /// </summary>
        private static Log m_Logger;
        private static object _InstSyncRoot = new object();
        public static Log Instance
        {
            get
            {
                if (m_Logger == null)
                {
                    lock (_InstSyncRoot)
                    {
                        if (m_Logger == null)
                        {
                            m_Logger = new Log();
                        }
                    }
                }
                return m_Logger;
            }
        }

        /// <summary>
        /// The absolute path to the log storage folder. At this time it is 
        /// in the Logs subdirectory of the application.
        /// </summary>
        private string m_StorageLocation;

        /// <summary>
        /// The active log file we are writing to. New log files are created
        /// at each new instance of the application. 
        /// </summary>
        private Stream m_ActiveLog;
        public Stream ActiveLog
        {
            set
            {
                m_ActiveLog = value;
            }
        }

        // The semaphore and mutex are used to signal the worker of new messages 
        // and allow thread-safe access to the queue, respectively. 
        private Semaphore sem_Entries = new Semaphore(0, int.MaxValue);
        private Mutex mux_LogQueue = new Mutex();
        /// <summary>
        /// For the moment we are just using this LogMessage class internally for
        /// the send queue. 
        /// </summary>
        private class LogMessage
        {
            public LogMessage(String msg, LogLevel l)
            {
                Message = msg; 
                Level = l; 
                TimeStamp = StringUtils.GetTimeStamp();
            }
            public String Message;
            public String TimeStamp;
            public LogLevel Level;
        }
        /// <summary>
        /// This internal structure contains the log messages we have received in order so they may be added
        /// to the output. 
        /// </summary>
        private Queue<LogMessage> m_LogQueue;

        #endregion Internal Members

        #region Internal Methods
        /// <summary>
        /// Instantiation is performed on the first call of any of the log 
        /// methods below. 
        /// </summary>
        private Log()
        {
            m_LogQueue = new Queue<Log.LogMessage>();
            //TODO: Move to Configuration File.
            //TODO: Expose with Run-Time Configuration 
            m_ConsoleReportLevel = LogLevel.Status;
            m_NetworkReportLevel = LogLevel.Information;

            //TODO: Move to Configuration File.
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = "RapidVision: Log";

            //Setup our logging directory if its not already present.
            m_StorageLocation = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Logs");
            if (!System.IO.Directory.Exists(m_StorageLocation))
            {
                Console.WriteLine("Logger: Creating Logs Directory.");
                //TODO: Handle Exceptions
                System.IO.Directory.CreateDirectory(m_StorageLocation);
            }

            try
            {
                m_ActiveLog = File.Open(
                    m_StorageLocation + "\\" + StringUtils.GetDateTimeFileName() + ".log", //Logs\TimeStamp.log
                    FileMode.Create);
            }
            //TODO: Expand on potential exceptions
            catch (Exception e)
            {
                m_ActiveLog = null;
                Console.WriteLine("Logger: Could not create new Log File!");
                throw e;
            }
            Console.WriteLine("Logger: Ready-to-go.");

        }

        #endregion Internal Methods

        /// <summary>
        /// Log a Debug Message. 
        /// These messages can be used for verbose description of application activities. 
        /// </summary>
        /// <param name="Message"></param>
        public static void Debug(string Message)
        {
            Instance.QueueMessage(Message, LogLevel.Debug);
        }

        /// <summary>
        /// Log a Info Message.
        /// These messages provide incremental progress updates of the operation of the system
        /// such as a sent packet, or update in camera gimbal mode. 
        /// </summary>
        /// <param name="Message"></param>
        public static void Info(string Message)
        {
            Instance.QueueMessage(Message, LogLevel.Information);
        }

        /// <summary>
        /// Log a Warning Message.
        /// These messages alert the operator to potential indicators to a coming failure,
        /// Network disconnect, image corruption, non-responsive gimbal, etc.
        /// </summary>
        /// <param name="Message"></param>
        public static void Warning(string Message)
        {
            Instance.QueueMessage(Message, LogLevel.Warning);
        }

        /// <summary>
        /// Log a Critical Error message. 
        /// These records alert the operator to detected failure, 
        /// sustained network failure, camera failures, etc
        /// </summary>
        /// <param name="Message"></param>
        public static void Critical(string Message)
        {
            Instance.QueueMessage(Message, LogLevel.Critical);
        }
        
        /// <summary>
        /// Log a Status message. 
        /// These records are meant to show state changes, or 
        /// transition events such as the connection of a cammera.
        /// </summary>
        /// <param name="Message"></param>
        public static void Status(string Message)
        {
            m_Logger.QueueMessage(Message, LogLevel.Status);
        }

        public static void Success(string Message)
        {
            Instance.QueueMessage(Message, LogLevel.Success);
        }

        public static void Raw(string Message)
        {
            Instance.QueueMessage(Message, LogLevel.Raw);
        }

        /// <summary>
        /// Internal common logging logic to get the message into the queue. 
        /// </summary>
        private void QueueMessage(string Message, LogLevel Level)
        {
            //We will construct our new message to be written, 
            // wait for acccess to the Data Structure, and then queue and
            // signal the messages arrival. 
            LogMessage m = new LogMessage(Message, Level);
            Thread t = new Thread(() =>
                {
                    mux_LogQueue.WaitOne();
                    m_LogQueue.Enqueue(m);
                    mux_LogQueue.ReleaseMutex();
                    sem_Entries.Release();
                });
            t.Start();
        }

        /// <summary>
        /// Working method for the Log Message. This method actually handles the request
        /// for the log and formats it accordingly. 
        /// </summary>
        /// <param name="LMsg">The Message as it was qeued.</param>
        private void ProcessMessage(LogMessage LMsg)
        {
            //Grab the useful bits from the structure we had before. 
            string Message = LMsg.Message;
            LogLevel Level = LMsg.Level;
            string timeStamp = LMsg.TimeStamp;
            
            string prefix = null;
            bool ChangeConsole = Level >= m_ConsoleReportLevel;
            switch(Level)
            {
                case LogLevel.Raw:
                    if (ChangeConsole) Console.ForegroundColor = ConsoleColor.White;
                    prefix = "";
                    break;
                case LogLevel.Debug:
                    if(ChangeConsole) Console.ForegroundColor = ConsoleColor.Green;
                    prefix = "DBUG";
                    break;
                case LogLevel.Status:
                    if (ChangeConsole) Console.ForegroundColor = ConsoleColor.DarkGray;
                    prefix = "STAT";
                    break;
                case LogLevel.Information:
                    if(ChangeConsole) Console.ForegroundColor = ConsoleColor.Gray;
                    prefix = "INFO";
                    break;
                case LogLevel.Warning:
                    if(ChangeConsole) Console.ForegroundColor = ConsoleColor.Yellow;
                    prefix = "WARN";
                    break;
                case LogLevel.Critical:
                    if(ChangeConsole) Console.ForegroundColor = ConsoleColor.Red;
                    prefix = "FAIL";
                    break;
                case LogLevel.Success:
                    if (ChangeConsole) Console.ForegroundColor = ConsoleColor.Cyan;
                    prefix = "SUCC"; 
                    break;
            }

            string Output;
            //TODO: Move this string to configurator
            if(LMsg.Level != LogLevel.Raw)
                Output = string.Format("[{0}|{1}] {2}\n", timeStamp, prefix, Message);
            else
                Output = string.Format("{0}\n", Message);
            
            //Write it out to console for the current app;
            if (Level >= ConsoleReportLevel)
            {
                try
                {
                    Console.Write(Output);
                }
                catch { } 
                /* We dont care that it fails to write to the console, so long as it still gets 
                 * written to the log file */
            }

            //Write it out to the log file is available
            //All Messages are always written to the log file.
            if(m_ActiveLog != null)
            {
                byte[] encoded = System.Text.Encoding.ASCII.GetBytes(Output);
                m_ActiveLog.Write(encoded, 0, encoded.Length);
                m_ActiveLog.Flush();
            }
            else
            {
                //No Log file available to write to, is that a cause of concern?
            }

            //If the settings of the console were change we want to reset them incase the user is
            //using their own Output messages.
            if (ChangeConsole)
            {
                Console.ResetColor();
            }
        }

        #region Threading
        private Thread m_MasterThread;
        private volatile bool m_Exiting = false;
        public static void Start()
        {
           Instance.StartWorker();
        }

        private void StartWorker()
        {
            m_MasterThread = new Thread(WorkerLoop);
            m_MasterThread.Start();
        }
        
        /// <summary>
        /// This is the loop of the worker thread. 
        /// </summary>
        private void WorkerLoop()
        {
            LogMessage messageToProcess;
            while(!m_Exiting)
            {
                messageToProcess = null;
                
                //Try to get a message entry to log...
                if(sem_Entries.WaitOne(20))
                {
                    //We have an entry to process because we have been given a semaphore acecss.
                    if(!mux_LogQueue.WaitOne(20))
                    {
                        //We couldn't get access to the queue, so we will want
                        //to increment our semaphore to ensure we are still
                        //keep track of the write number in the queue. 
                        sem_Entries.Release();
                    }
                    else
                    {
                        //if we did get into the data structure we will want to
                        //grab the next log for processing and then give back our
                        //access
                        messageToProcess = m_LogQueue.Dequeue();
                        mux_LogQueue.ReleaseMutex();
                    }
                }

                if(messageToProcess != null)
                {
                    //We have a message to process, so lets do that!
                    ProcessMessage(messageToProcess);
                }
            }

            //If we have reached this point, exiting = true;
            //We might want to revisit this later, but right now we are going to try to 
            //get all the other log messages output before we kill ourselves. 
            //The logger should be the last thread that is signaled to terminate
            //and join on, so it should be able to complete this without issue, so we think. 
            while(m_LogQueue.Count > 0)
            {
                if(mux_LogQueue.WaitOne(20))
                {
                    LogMessage m = m_LogQueue.Dequeue();
                    mux_LogQueue.ReleaseMutex();
                    ProcessMessage(m);
                }
            }

        }

        //We dont really meant to have this accessible in the Static
        //Space, but it will work for the moment. 
        public static Thread RequestStop()
        {
            Instance.m_Exiting = true;
            return Instance.m_MasterThread;
        }
        
        
        public void AllStop()
        {
            //Although this is now common in my threading pattern
            //I am not going to bother with the overhead at the moment
            //as we are not using this method in the common case. 
            throw new NotImplementedException();
        }

        #endregion Threading
    }
}
