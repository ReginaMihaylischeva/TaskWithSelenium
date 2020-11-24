using System;

namespace Test
{
    public class Program
    {
        public static void Main()
        {
            double coefficientA = Validate("Please input a:");
            double coefficientB = Validate("Please input b:");
            double coefficientC = Validate("Please input c:");
            Calculations(coefficientA, coefficientB, coefficientC);
        }

        static public string Calculations(double coefficientA, double coefficientB, double coefficientC)
        {
            double D = 0;
            double X1 = 0;
            double X2 = 0;

            if (coefficientA == 0)
            {
                X1 = -coefficientC / coefficientB;
                Console.WriteLine("X1 = {0: 0.000000}", X1);
                return $"X1 = {X1}";
            }
            else
            {
                D = Math.Pow(coefficientB, 2) - 4 * coefficientA * coefficientC;
                if (D > 0)
                {
                    X1 = (-coefficientB + Math.Sqrt(D)) / (2 * coefficientA);
                    X2 = (-coefficientB - Math.Sqrt(D)) / (2 * coefficientA);
                    Console.WriteLine("X1 = {0: 0.000000}", X1);
                    Console.WriteLine("X2 = {0: 0.000000}", X2);
                    return $"X1 = {X1} \n X2 = {X2}";
                }
                if (D == 0)
                {
                    X1 = (-coefficientB + Math.Sqrt(D)) / (2 * coefficientA);
                    Console.WriteLine("X1 = {0: 0.000000}", X1);
                    Console.WriteLine("X2 = {0: 0.000000}", X1);
                    return $"X1 = {X1} \n X2 = {X1}";
                }
                Console.WriteLine("Discriminant less than zero.");
                return "Discriminant less than zero.";
            }
        }

        static double Validate(string message)
        {
            double validValue;
            Console.WriteLine(message);
            string value = Console.ReadLine();
            while (!double.TryParse(value, out validValue))
            {
                Console.WriteLine("Please input correct value!");
                value = Console.ReadLine();
            };
            return validValue;
        }
    }
}
