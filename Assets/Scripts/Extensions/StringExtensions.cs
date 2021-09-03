using System;

namespace Assets.Scripts.Extensions
{
    public static class StringExtensions
    {
        public static void Deconstruct(this string[] arr, out string s1, out string s2)
        {
            if (arr.Length < 2) throw new ArgumentOutOfRangeException(nameof(arr));

            (s1, s2) = (arr[0], arr[1]);
        }

        public static void Deconstruct(this string[] arr, out string s1, out string s2, out string s3)
        {
            if (arr.Length < 3) throw new ArgumentOutOfRangeException(nameof(arr));

            (s1, s2, s3) = (arr[0], arr[1], arr[2]);
        }

        public static void Deconstruct(this string[] arr, out string s1, out string s2, out string s3, out string s4)
        {
            if (arr.Length < 4) throw new ArgumentOutOfRangeException(nameof(arr));

            (s1, s2, s3, s4) = (arr[0], arr[1], arr[2], arr[3]);
        }
    }
}
