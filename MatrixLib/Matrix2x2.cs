using System;
using System.Text;

namespace MatrixLib
{
    /// <summary>
    /// Матриця 2x2 цілих чисел (варіант 11).
    /// Дані класу: статичний масив елементів матриці.
    /// Операції: + і * матриць, множення на число, ~ (визначник), ! (транспонування).
    /// </summary>
    public class Matrix2x2
    {
        public const int Size = 2;

        // Дані класу: статичний масив елементів 2x2
        private readonly int[,] _data = new int[Size, Size];

        // Конструктори

        /// <summary>Конструктор за замовчуванням: нульова матриця.</summary>
        public Matrix2x2() { }

        /// <summary>Конструктор з параметрами: 4 елементи по рядках.</summary>
        public Matrix2x2(int a11, int a12,
                         int a21, int a22)
        {
            _data[0, 0] = a11; _data[0, 1] = a12;
            _data[1, 0] = a21; _data[1, 1] = a22;
        }

        /// <summary>Конструктор з двовимірного масиву 2x2.</summary>
        public Matrix2x2(int[,] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.GetLength(0) != Size || values.GetLength(1) != Size)
                throw new ArgumentException("Масив має бути розміром 2x2.", nameof(values));
            Array.Copy(values, _data, values.Length);
        }

        /// <summary>Конструктор копіювання.</summary>
        public Matrix2x2(Matrix2x2 other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            Array.Copy(other._data, _data, _data.Length);
        }

        // Індексатор (доступ до елементів замість get/set-функцій)

        public int this[int row, int col]
        {
            get { Check(row, col); return _data[row, col]; }
            set { Check(row, col); _data[row, col] = value; }
        }

        private static void Check(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                throw new IndexOutOfRangeException($"Індекс [{row},{col}] поза межами матриці 2x2.");
        }

        // Функція введення

        /// <summary>Введення матриці: 2 рядки по 2 цілих числа через пробіл.
        /// З клавіатури: m.Input(Console.In).</summary>
        public void Input(System.IO.TextReader reader)
        {
            for (int i = 0; i < Size; i++)
            {
                string? line = reader.ReadLine();
                string[] parts = (line ?? "").Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != Size)
                    throw new FormatException($"Рядок {i + 1}: потрібно рівно 2 числа.");
                for (int j = 0; j < Size; j++)
                    _data[i, j] = int.Parse(parts[j]);
            }
        }

        // ToString

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                sb.Append("| ");
                for (int j = 0; j < Size; j++)
                    sb.Append(_data[i, j].ToString().PadLeft(5)).Append(' ');
                sb.Append('|');
                if (i < Size - 1) sb.AppendLine();
            }
            return sb.ToString();
        }

        // Операції з завдання

        /// <summary>Додавання матриць.</summary>
        public static Matrix2x2 operator +(Matrix2x2 a, Matrix2x2 b)
        {
            var r = new Matrix2x2();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    r._data[i, j] = a._data[i, j] + b._data[i, j];
            return r;
        }

        /// <summary>Множення матриць (рядок на стовпець).</summary>
        public static Matrix2x2 operator *(Matrix2x2 a, Matrix2x2 b)
        {
            var r = new Matrix2x2();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    int s = 0;
                    for (int k = 0; k < Size; k++)
                        s += a._data[i, k] * b._data[k, j];
                    r._data[i, j] = s;
                }
            return r;
        }

        /// <summary>Множення матриці на число.</summary>
        public static Matrix2x2 operator *(Matrix2x2 m, int k)
        {
            var r = new Matrix2x2();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    r._data[i, j] = m._data[i, j] * k;
            return r;
        }

        /// <summary>Множення числа на матрицю.</summary>
        public static Matrix2x2 operator *(int k, Matrix2x2 m) => m * k;

        /// <summary>Визначник (операція "~"): a11*a22 - a12*a21.</summary>
        public static int operator ~(Matrix2x2 m) =>
            m._data[0, 0] * m._data[1, 1] - m._data[0, 1] * m._data[1, 0];

        /// <summary>Транспонування (операція "!").</summary>
        public static Matrix2x2 operator !(Matrix2x2 m)
        {
            var r = new Matrix2x2();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    r._data[j, i] = m._data[i, j];
            return r;
        }

        // true / false
        // Інтерпретація: матриця "істинна", якщо вона невироджена (визначник != 0),
        // тобто для неї існує обернена матриця.

        public static bool operator true(Matrix2x2 m) => ~m != 0;
        public static bool operator false(Matrix2x2 m) => ~m == 0;

        // Приведення типів

        /// <summary>Неявне: ціле число k -> скалярна матриця k*E (k на головній діагоналі).</summary>
        public static implicit operator Matrix2x2(int k) =>
            new Matrix2x2(k, 0,
                          0, k);

        /// <summary>Неявне: масив int[2,2] -> матриця.</summary>
        public static implicit operator Matrix2x2(int[,] values) => new Matrix2x2(values);

        /// <summary>Явне: матриця -> копія масиву int[2,2].</summary>
        public static explicit operator int[,](Matrix2x2 m) => (int[,])m._data.Clone();

        // Порівняння
        // Для матриць відношення порядку не визначене, тому лише == та !=.

        public static bool operator ==(Matrix2x2? a, Matrix2x2? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (a._data[i, j] != b._data[i, j]) return false;
            return true;
        }

        public static bool operator !=(Matrix2x2? a, Matrix2x2? b) => !(a == b);

        public override bool Equals(object? obj) => obj is Matrix2x2 m && this == m;

        public override int GetHashCode()
        {
            var h = new HashCode();
            foreach (int v in _data) h.Add(v);
            return h.ToHashCode();
        }
    }
}
