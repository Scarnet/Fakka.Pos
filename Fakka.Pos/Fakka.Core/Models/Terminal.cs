using Fakka.Core.Enums;

namespace Fakka.Core.Models
{
    /// <summary>
    /// Object used in ApplicationManager
    /// </summary>
    public class Terminal
    {
        public Terminal(TerminalCode terminalCode)
        {
            TerminalCode = terminalCode;
        }

        public TerminalCode TerminalCode { get; }
    }
}