using System;
using System.IO;
using MatrixLib; // підключення бібліотеки класів MatrixLib.dll

namespace MatrixApp
{
    internal static class Program
    {
        private static void Show(string title, object value)
        {
            Console.WriteLine(title);
            Console.WriteLine(value);
            Console.WriteLine();
        }

        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №2. Варіант 11: \"Матриця 2x2 цілих чисел\" ===\n");

            // Конструктори
            var a = new Matrix2x2(3, 1,
                                  2, 4);
            var b = new Matrix2x2(new int[,] { { 1, 2 }, { 0, -1 } });
            var c = new Matrix2x2(a);        // копіювання
            var zero = new Matrix2x2();      // нульова

            Show("Матриця A:", a);
            Show("Матриця B:", b);
            Show("C, копія A:", c);
            Show("Нульова матриця:", zero);

            // Функція введення (з рядка, щоб програма працювала без ручного введення;
            // з клавіатури викликається d.Input(Console.In))
            var d = new Matrix2x2();
            d.Input(new StringReader("5 -2\n7 1"));
            Show("D, введена функцією Input:", d);

            // Індексатор
            Console.WriteLine($"A[1,0] = {a[1, 0]}");
            c[0, 0] = 9;
            Show("C після C[0,0] = 9 (A не змінилась):", c);

            // Операції з завдання
            Show("A + B:", a + b);
            Show("A * B:", a * b);
            Show("B * A (не дорівнює A * B):", b * a);
            Show("A * 3:", a * 3);
            Show("2 * B:", 2 * b);
            Console.WriteLine($"~A (визначник A) = {~a}");
            Console.WriteLine($"~B (визначник B) = {~b}\n");
            Show("!A (транспонована A):", !a);

            // true / false
            var s = new Matrix2x2(1, 2,
                                  2, 4);
            Show("S (вироджена, рядок 2 = 2 * рядок 1):", s);
            Console.WriteLine(a ? "A невироджена (true)" : "A вироджена (false)");
            Console.WriteLine(s ? "S невироджена (true)" : "S вироджена (false)");
            Console.WriteLine();

            // Приведення типів
            Matrix2x2 e = 1;                                       // неявне int -> одинична матриця
            Show("E = 1 (неявне приведення int -> матриця):", e);
            Matrix2x2 f = new int[,] { { 1, 1 }, { 2, 2 } };       // неявне int[,] -> матриця
            Show("F з масиву (неявне приведення int[,] -> матриця):", f);
            int[,] arr = (int[,])a;                                // явне матриця -> int[,]
            Console.WriteLine($"(int[,])A: arr[1,1] = {arr[1, 1]}, довжина = {arr.Length}\n");

            // Порівняння
            Console.WriteLine($"A == копія A ? {a == new Matrix2x2(a)}");
            Console.WriteLine($"A != B ? {a != b}");
            Console.WriteLine($"A * B == B * A ? {a * b == b * a}");
            Console.WriteLine($"A * E == A ? {a * e == a}");
            Console.WriteLine($"!!A == A ? {!!a == a}");
            Console.WriteLine($"~(A * B) == ~A * ~B ? {~(a * b) == ~a * ~b}");
        }
    }
}
