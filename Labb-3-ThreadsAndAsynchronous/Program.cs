using Labb_3_ThreadsAndAsynchronous.Models;
using System.Runtime.CompilerServices;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace Labb_3_ThreadsAndAsynchronous
{
    internal class Program
    {
        public static double DistanceToRace = 10;

        static void Main(string[] args)
        {
            //Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            List<Car> cars = new List<Car> {
                new Car("BMW", 0, 120),
                new Car("AUDI", 0, 120),
                new Car("VOLVO", 0, 120),
                new Car("VOlKSWAGEN", 0, 120),
                new Car("SAAB", 0, 120)
            };

            AnsiConsole.Progress()
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),    // Task description
                    new ProgressBarColumn(),        // Progress bar
                    new PercentageColumn(),         // Percentage
                    //new RemainingTimeColumn(),      // Remaining time
                    new SpinnerColumn(),            // Spinner
                })
                .AutoRefresh(false)
                .Start(ctx =>
                {
                    var threads = new List<Thread>();

                    foreach (var car in cars)
                    {
                        var task = ctx.AddTask($"[bold blue]{car.Name}[/]", new ProgressTaskSettings { MaxValue = DistanceToRace });
                        threads.Add(new Thread(() => RaceCar(car, task, stopwatch)));
                    }

                    foreach (var thread in threads)
                    {
                        thread.Start();
                    }

                    while (cars.Any(d => d.CurrentDistance < DistanceToRace))
                    {
                        if (stopwatch.ElapsedMilliseconds >= 31000)
                        {
                            foreach (var car in cars.Where(d => d.CurrentDistance < DistanceToRace))
                            {
                                Console.Out.Flush();
                                car.RandomAction();
                                Console.Out.Flush();
                            }

                            stopwatch.Restart();
                        }
                        if ((stopwatch.ElapsedMilliseconds) % 100 == 0)
                        {
                            ctx.Refresh();
                        }
                    }

                    foreach (var thread in threads)
                    {
                        thread.Join();
                    }

                    ctx.Refresh();
                });

            cars.Sort((c1, c2) => c1.CurrentPlacement.CompareTo(c2.CurrentPlacement));

            foreach (var car in cars)
            {
                if (car.CurrentPlacement == 1) { AnsiConsole.Markup("[green]\t{0} took {1} place![/]\n", car.Name, car.CurrentPlacement); }
                else if (car.CurrentPlacement == 2) { AnsiConsole.Markup("[yellow]\t{0} took {1} place![/]\n", car.Name, car.CurrentPlacement); }
                else { AnsiConsole.Markup("\t[red]{0} took {1} place![/]\n", car.Name, car.CurrentPlacement); }
            }

            AnsiConsole.MarkupLine("[yellow underline]\n\tPress any key to exit![/]");
            Console.ReadKey();
        }

        static void RaceCar(Car car, ProgressTask task, Stopwatch stopwatch)
        {
            while (car.CurrentDistance < DistanceToRace)
            {
                if(car.SleepTime != 0)
                {
                    Thread.Sleep(car.SleepTime);
                    car.SleepTime = 0;
                }

                Thread.Sleep(1000);
                car.CurrentDistance += car.CurrentSpeed / 3.6 / 1000;
                task.Value = car.CurrentDistance;
            }

            car.UpdatePlacement(DistanceToRace);
        }
    }
}
