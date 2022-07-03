using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Util
{
    [DoesNotReturn]
    public static void ThrowArgumentOutOfRangeException(string paramName, string message)
        => throw new ArgumentOutOfRangeException(paramName, message);

    [DoesNotReturn]
    public static void ThrowInvalidOperationException(string message)
        => throw new InvalidOperationException(message);
}