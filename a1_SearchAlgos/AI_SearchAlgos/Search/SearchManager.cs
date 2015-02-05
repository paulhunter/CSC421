
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace AI_SearchAlgos.Search
{
    using Utils;
    /// <summary>
    /// The Search Manager class is used to conduct experiments with each of the algorithms
    /// 
    /// </summary>

    public class SearchManager
    {
        static int THREADS = Environment.ProcessorCount;

        static ISearchAlgorithm[] Algos = new ISearchAlgorithm[]
        {
            new BreadthFirstSearch(),
            new DepthFirstSearch(),
            new IterativeDeepeningSearch(),
            new BestFirstSearch(new Heuristics.EuclidianDistance()),
            new BestFirstSearch(new Heuristics.TileDistanceHeuristic()),
            new AStarSearch(new Heuristics.EuclidianDistance()),
            new AStarSearch(new Heuristics.TileDistanceHeuristic()),
            new HillClimbSearch(new Heuristics.EuclidianDistance()),
            new HillClimbSearch(new Heuristics.TileDistanceHeuristic())
        };

        static uint TotalTests;
        static volatile uint CompleteTests = 0;

        static Queue<Tuple<HexagonalTileSearchProblem, ManualResetEvent, int, int>> tasks;
        static Stream output;

        /// <summary>
        /// In this method we run each of our search methods against
        /// 100 random instances of 4 configurations. Each 
        /// search algorithm is given the same 100 instances of each configuration
        /// and each instance is first checked to ensure it has a valid path using
        /// BFS. 
        /// </summary>
        public static void RunTestSuite()
        {
            DateTime start_time = DateTime.Now;
            output = File.Open("Output.txt", FileMode.Create);
            Log.Success(string.Format("Preparing to Utilize {0} logical processors", THREADS));
            Log.Success("Press enter to begin...");
            Console.ReadLine();

            HexagonalTileSearchProblem[] Configs = new HexagonalTileSearchProblem[]
            {
                new HexagonalTileSearchProblem(4, 3, 0.2),
                new HexagonalTileSearchProblem(4, 3, 0.5),
                new HexagonalTileSearchProblem(5, 4, 0.2),
                new HexagonalTileSearchProblem(5, 4, 0.5),
                new HexagonalTileSearchProblem(6, 6, 0.2), 
                new HexagonalTileSearchProblem(6, 6, 0.5),

                new HexagonalTileSearchProblem(10, 10, 0.2),
                new HexagonalTileSearchProblem(10, 10, 0.5),
                new HexagonalTileSearchProblem(15, 15, 0.2),
                new HexagonalTileSearchProblem(15, 15, 0.5)
            };


            TotalTests = (uint)Configs.Count() * (uint)Algos.Count() * 100;
            ManualResetEvent[] doneEvents = new ManualResetEvent[Configs.Count() * 5];
            List<Thread> threads = new List<Thread>();
            int i = 0;
            int test_num = 0;
            Thread t;

            Tuple<HexagonalTileSearchProblem, ManualResetEvent, int, int> param;
            tasks = new Queue<Tuple<HexagonalTileSearchProblem, ManualResetEvent, int, int>>();
            
            foreach(HexagonalTileSearchProblem p in Configs)
            {
                for (int x = 0; x < 5; x++)
                {
                    doneEvents[i] = new ManualResetEvent(false);
                    param = new Tuple<HexagonalTileSearchProblem, ManualResetEvent, int, int>(
                        p.Clone(), doneEvents[i], i++, test_num);
                    tasks.Enqueue(param);
                    test_num += 20;
                }
            }

            for (i = 0; i < THREADS; i++)
            {
                t = new Thread(() =>
                {
                    TaskThread();
                });
                threads.Add(t);
                t.Start();
            }
            WaitHandle.WaitAll(doneEvents);

            Log.Status("Flushing results to file...");
            output.Flush();
            output.Close();

            DateTime end_time = DateTime.Now;
            Log.Status(string.Format("Complete Test Suite executed in {0:0.000} seconds.", (end_time - start_time).TotalMilliseconds / 1000.0));
            Log.Success("All Tests Complete - Press Enter to Exit");
            Console.ReadLine();
        }

        private static void TaskThread()
        {
            Tuple<HexagonalTileSearchProblem, ManualResetEvent, int, int> p = null;
            while(true)
            {
                if(tasks.Count == 0)
                {
                    break;
                }
                lock(tasks)
                {
                    if (tasks.Count > 0)
                        p = tasks.Dequeue();
                    else
                        break;
                }
                Run20Tests(p);
            }
        }

        private static void Run20Tests(Tuple<HexagonalTileSearchProblem,ManualResetEvent,int,int> Param)
        {
            HexagonalTileSearchProblem problem = Param.Item1;
            ManualResetEvent doneEvent = Param.Item2;
            int ConfigurationNumber = Param.Item3;
            int TestSequenceNumber = Param.Item4;
            int RegenPaths = 5;
            SearchResults[] results = new SearchResults[Algos.Count()];
            for (int i = 0; i < 20; i++, TestSequenceNumber++)
            {
                RegenPaths = 5;
                Log.Status(string.Format("Test({0}) - Generating Problem {1}...", ConfigurationNumber, TestSequenceNumber));
                problem.Reset();
                SearchResults sr = Algos[0].Search(problem);
                while (sr.Solved != true)
                {
                    
                    if (RegenPaths == 0)
                    {
                        Log.Status(string.Format("Test({0}) - Problem {1} Not Solvable, regenerating...", ConfigurationNumber, TestSequenceNumber));
                        problem.Reset();
                        RegenPaths = 5;
                        
                    }
                    else
                    {
                        Log.Status(string.Format("Test({0}) - Reselecting Start/Goal...", ConfigurationNumber, TestSequenceNumber));
                        problem.SelectRandomStartAndGoal();
                        
                    }
                    
                    sr = Algos[0].Search(problem);
                    RegenPaths--;
                }

                Log.Status(string.Format("Test({0} : {1}) - Applying Search Algorithms...", ConfigurationNumber, TestSequenceNumber));
                int a = 0;
                foreach (ISearchAlgorithm al in Algos)
                {
                    results[a] = al.Search(problem);
                    CompleteTests++;
                    Log.Success(string.Format("# {0:000.000}% #", (CompleteTests * 1.0) / TotalTests * 100));
                    a++;
                }
                WriteResultsToCSV(TestSequenceNumber, problem, results);
            }


            doneEvent.Set();

        }

        private static void WriteResultsToCSV(int Prob, HexagonalTileSearchProblem Problem, SearchResults[] Results)
        {
            StringBuilder sb = new StringBuilder(1024);
            sb.AppendFormat("{0},{1},", Prob, Problem.ToString());
            foreach(SearchResults sr in Results)
            {
                sb.AppendFormat("{0},", sr.ToString());
            }
            sb.Append("\n");
            string o = sb.ToString();
            byte[] b = System.Text.Encoding.UTF8.GetBytes(o);
            lock(output)
            {
                output.Write(b, 0, b.Count());
                output.Flush();
            }
            
        }

    }
}
