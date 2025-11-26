using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sabatex.Extensions.Diagnostics;
/// <summary>
/// Provides a trace listener that writes messages to a text file or stream, prefixing each message with the current
/// date and time.
/// </summary>
/// <remarks>Use this class to log trace or debug output with timestamps for each entry. The date and time are
/// included in the default format at the beginning of each line, which can assist in tracking the timing of events
/// during application execution. This class is useful for scenarios where chronological tracing is important, such as
/// diagnostics or auditing.</remarks>
public class TextWriterTraceListenerWithDate: TextWriterTraceListener
{
    /// <summary>
    /// Initializes a new instance of the TextWriterTraceListenerWithDate class that writes tracing or debugging output
    /// to the specified file, including date information with each entry.
    /// </summary>
    /// <param name="fileName">The name of the file to which the trace or debug output is written. If the file does not exist, it is created;
    /// otherwise, output is appended to the existing file.</param>
    public TextWriterTraceListenerWithDate(string fileName) : base(fileName) { }
    /// <summary>
    /// Writes the specified message to the output, prefixing it with the current date and time, followed by a line
    /// terminator.
    /// </summary>
    /// <remarks>The output includes the current date and time in the default format, followed by the
    /// specified message. This method is typically used for logging or tracing purposes where timestamps are
    /// required.</remarks>
    /// <param name="message">The message to write to the output. If null, only the date and time are written.</param>
    public override void WriteLine(string? message)
    {
        base.WriteLine(DateTime.Now.ToString() + " " + message);
    }
}
