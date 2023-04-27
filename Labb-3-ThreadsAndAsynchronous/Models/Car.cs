using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace Labb_3_ThreadsAndAsynchronous.Models
{
    internal class Car
    {
        public string Name { get; set; }
        public double CurrentDistance { get; set; }
        public double CurrentSpeed { get; set; }
        public int CurrentPlacement { get; set; }
        public int SleepTime { get; set; }

        public static int Placement = 0;

        public Car(string name, double currentDistance, double currentSpeed)
        {
            Name = name;
            CurrentDistance = currentDistance;
            CurrentSpeed = currentSpeed;
        }

        public int UpdatePlacement(double distanceToRace)
        {
            Placement++;
            CurrentPlacement = Placement;
            return CurrentPlacement;
        }

        public void RandomAction()
        {
            Random random = new Random();
            int probability = random.Next(1, 51);
            if (probability == 1) { NoFuel(); }
            else if (probability <= 2) { FlatTire(); }
            else if (probability <= 5) { HitBird(); }
            else if (probability <= 10) { EngineTuning(); }
            else if (probability <= 20) { EngineFailure(); }
        }

        public void NoFuel()
        {
            AnsiConsole.MarkupLine("[red]\n\t\b{0} needs to refuel, this will take 30 seconds to fix![/]", Name);
            SleepTime = 30000;
        }

        public void FlatTire()
        {
            AnsiConsole.MarkupLine("[red]\n\t\b{0} got a flat tire, this will take 20 seconds to fix![/]", Name);
            SleepTime = 20000;
        }

        public void HitBird()
        {
            AnsiConsole.MarkupLine("[red]\n\t\b{0} hit a bird and cant see out the window, this will take 10 seconds to fix![/]", Name);
            SleepTime = 10000;
        }

        public void EngineFailure()
        {
            AnsiConsole.MarkupLine("[red]\n\t\b{0} got engine failure, the car runs 5km/h slower![/]", Name);
            CurrentSpeed -= 5;
        }

        public void EngineTuning()
        {
            AnsiConsole.MarkupLine("[green]\n\t\b{0}'s team will do a engine tuning, the car runs 50km/h faster, 10 seconds delay![/]", Name);
            SleepTime = 10000;
            CurrentSpeed += 50;
        }

    }
}
