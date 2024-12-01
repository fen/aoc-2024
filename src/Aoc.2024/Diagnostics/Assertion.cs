using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aoc.Diagnostics;

public static class Assertion
{
    [StackTraceHidden]
    [DebuggerHidden]
    public static void Assert([DoesNotReturnIf(false)] bool condition,
        [InterpolatedStringHandlerArgument(nameof(condition))]
        ref AssertionInterpolatedStringHandler message,
        [CallerArgumentExpression(nameof(condition))]
        string? conditionCallerArgumentExpression = null)
    {
        Assert(condition, message.ToStringAndClear(), conditionCallerArgumentExpression);
    }

    [StackTraceHidden]
    [DebuggerHidden]
    public static void Assert([DoesNotReturnIf(false)] bool condition, string? message = null,
        [CallerArgumentExpression(nameof(condition))]
        string? conditionCallerArgumentExpression = null)
    {
        if (!condition)
        {
            throw new AssertionException(message ?? "Assertion failure", conditionCallerArgumentExpression);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [InterpolatedStringHandler]
    public struct AssertionInterpolatedStringHandler
    {
        const int GuessedLengthPerHole = 11;
        const int MinimumArrayPoolLength = 256;

        StringBuilder? _stringBuilder;

        StringBuilder.AppendInterpolatedStringHandler _stringBuilderHandler;

        public AssertionInterpolatedStringHandler(int literalLength, int formattedCount, bool condition,
            out bool shouldAppend)
        {
            if (!condition)
            {
                _stringBuilder = StringBuilderCache.Acquire(GetDefaultLength(literalLength, formattedCount));
                _stringBuilderHandler = new StringBuilder.AppendInterpolatedStringHandler(literalLength, formattedCount,
                    _stringBuilder);
                shouldAppend = true;
            }
            else
            {
                _stringBuilder = null;
                _stringBuilderHandler = default;
                shouldAppend = false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetDefaultLength(int literalLength, int formattedCount)
        {
            return Math.Max(MinimumArrayPoolLength, literalLength + formattedCount * GuessedLengthPerHole);
        }

        internal string ToStringAndClear()
        {
            var s = _stringBuilder is not null
                ? StringBuilderCache.GetStringAndRelease(_stringBuilder)
                : string.Empty;
            _stringBuilderHandler = default;
            _stringBuilder = null;
            return s;
        }

        public void AppendLiteral(string value)
        {
            _stringBuilderHandler.AppendLiteral(value);
        }

        public void AppendFormatted<T>(T value)
        {
            _stringBuilderHandler.AppendFormatted(value);
        }

        public void AppendFormatted<T>(T value, string? format)
        {
            _stringBuilderHandler.AppendFormatted(value, format);
        }

        public void AppendFormatted<T>(T value, int alignment)
        {
            _stringBuilderHandler.AppendFormatted(value, alignment);
        }

        public void AppendFormatted<T>(T value, int alignment, string? format)
        {
            _stringBuilderHandler.AppendFormatted(value, alignment, format);
        }

        public void AppendFormatted(ReadOnlySpan<char> value)
        {
            _stringBuilderHandler.AppendFormatted(value);
        }

        public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null)
        {
            _stringBuilderHandler.AppendFormatted(value, alignment, format);
        }

        public void AppendFormatted(string? value)
        {
            _stringBuilderHandler.AppendFormatted(value);
        }

        public void AppendFormatted(string? value, int alignment = 0, string? format = null)
        {
            _stringBuilderHandler.AppendFormatted(value, alignment, format);
        }

        public void AppendFormatted(object? value, int alignment = 0, string? format = null)
        {
            _stringBuilderHandler.AppendFormatted(value, alignment, format);
        }
    }
}

[Serializable]
public class AssertionException(string message, string? conditionCallerArgumentExpression = null) : Exception(message)
{
    readonly string _message = message;

    public string? ConditionCallerArgumentExpression { get; } = conditionCallerArgumentExpression;

    public override string Message => ConditionCallerArgumentExpression is null
        ? _message
        : $"{_message} -- {ConditionCallerArgumentExpression}";
}
