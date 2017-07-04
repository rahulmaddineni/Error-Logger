using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLLLibrary;
using System.Threading;
using System.Net.NetworkInformation;

namespace TestClient
{
    class Client
    {
        /// <summary>
        /// How many messages should each thread send before shutting down?
        /// </summary>
        private static int NUMBER_OF_MESSAGES_PER_THREAD = 10;

        /// <summary>
        /// How many threads should be sending messages concurrently?
        /// </summary>
        private static int NUMBER_OF_THREADS_SENDING_MESSAGES = 1;

        /// <summary>
        /// Minimum log level supported by your logger
        /// </summary>
        private static int MIN_LOG_LEVEL = 1;

        /// <summary>
        /// Maximum log level supported by your logger
        /// </summary>
        private static int MAX_LOG_LEVEL = 4;

        /// <summary>
        /// How often should an exception be sent into the logger - every X number of messages
        /// </summary>
        private static int HOW_OFTEN_AN_EXCEPTION = 2;

        static void Main(string[] args)
        {
            Console.Title = "Test Client";
            Console.WriteLine("Test Client Inserting Logs into Rest through DLL:");
            Console.WriteLine("================================================\n");
            
            List<Thread> threads = new List<Thread>();

            // Setup threads
            for (int i = 1; i <= NUMBER_OF_THREADS_SENDING_MESSAGES; i++)
            {
                int threadNumber = i;
                Random rand = new Random();

                Thread newThread = new Thread(x =>
                {
                    Exception exception = new Exception();
                    int logLevel;

                    for (int messageNumber = 1; messageNumber <= NUMBER_OF_MESSAGES_PER_THREAD; messageNumber++)
                    {
                        //int appid = rand.Next(1, 8);
                        int appid = 13;

                        DLL dll = new DLL(appid, 1);

                        Console.WriteLine("Inserting into " + appid);

                        logLevel = rand.Next(MIN_LOG_LEVEL, MAX_LOG_LEVEL);

                        if (messageNumber % HOW_OFTEN_AN_EXCEPTION != 0)
                        {
                            dll.Log(exception.Message, logLevel, exception);
                        }
                        else
                        {
                            dll.Log(null, logLevel, null);
                        }
                    }
                });

                threads.Add(newThread);
            }

            // Start threads
            foreach (Thread aThread in threads)
            {
                aThread.Start();
            }

            // Join threads
            foreach (Thread aThread in threads)
            {
                aThread.Join();
            }


            Console.WriteLine("All Threads Finished");
            Console.ReadLine();
        }
    }
}
