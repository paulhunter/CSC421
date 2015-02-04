
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
        const uint THREADS = 2;

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

        static uint TotalTests = 6 * (uint)Algos.Count() * 100;
        static volatile uint CompleteTests = 0;

        static Queue<Tuple<HexagonalTileSearchProblem, ManualResetEvent, int>> tasks;
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
            output = File.Open("Output.txt", FileMode.Create);
            ManualResetEvent[] doneEvents = new ManualResetEvent[6*10];
            List<Thread> threads = new List<Thread>();
            int i = 0;
            Thread t;

            HexagonalTileSearchProblem[] Configs = new HexagonalTileSearchProblem[]
            {
                new HexagonalTileSearchProblem(4, 3, 0.2),
                new HexagonalTileSearchProblem(4, 3, 0.5),
                new HexagonalTileSearchProblem(5, 4, 0.2),
                new HexagonalTileSearchProblem(5, 4, 0.5),
                new HexagonalTileSearchProblem(6, 6, 0.2), 
                new HexagonalTileSearchProblem(6, 6, 0.5)
            };

            Tuple<HexagonalTileSearchProblem, ManualResetEvent, int> param;
            tasks = new Queue<Tuple<HexagonalTileSearchProblem, ManualResetEvent, int>>();
            foreach(HexagonalTileSearchProblem p in Configs)
            {
                for (int x = 0; x < 10; x++)
                {
                    doneEvents[i] = new ManualResetEvent(false);
                    param = new Tuple<HexagonalTileSearchProblem, ManualResetEvent, int>(
                        p.Clone(), doneEvents[i], i++
                        );
                    tasks.Enqueue(param);
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

            Log.Success("All Tests Complete - Press Enter to Exit");
            Console.ReadLine();
            output.Close();

        }

        private static void TaskThread()
        {
            Tuple<HexagonalTileSearchProblem, ManualResetEvent, int> p = null;
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
                Run10Tests(p);
            }
        }

        private static void Run10Tests(Tuple<HexagonalTileSearchProblem,ManualResetEvent,int> Param)
        {
            HexagonalTileSearchProblem problem = Param.Item1;
            ManualResetEvent doneEvent = Param.Item2;
            int ConfigurationNumber = Param.Item3;
            SearchResults[] results = new SearchResults[Algos.Count()];
            for (int i = 0; i < 10; i++)
            {
                Log.Warning(string.Format("Test({0}) - Generating Problem {1}...", ConfigurationNumber, i));
                problem.Reset();
                SearchResults sr = Algos[0].Search(problem);
                while (sr.Solved != true)
                {
                    Log.Warning(string.Format("Test({0}) - Problem {1} Not Solvable, regenerating...", ConfigurationNumber, i));
                    problem.SelectRandomStartAndGoal();
                    sr = Algos[0].Search(problem);
                }

                Log.Warning(string.Format("Test({0} : {1}) - Applying Search Algorithms...", ConfigurationNumber, i));
                int a = 0;
                foreach (ISearchAlgorithm al in Algos)
                {
                    results[a] = al.Search(problem);
                    CompleteTests++;
                    Log.Success(string.Format("# {0:000.000}% #", (CompleteTests * 1.0) / TotalTests));
                    a++;
                }
                WriteResultsToCSV(ConfigurationNumber, i, problem, results);
            }


            doneEvent.Set();

        }

        private static void WriteResultsToCSV(int Conf, int Prob, HexagonalTileSearchProblem Problem, SearchResults[] Results)
        {
            StringBuilder sb = new StringBuilder(1024);
            sb.AppendFormat("{0}-{1},{2},", Conf, Prob, Problem.ToString());
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
