using Simix.Extensions.Logging.Enums;

namespace Simix.Extensions.Logging.Helpers {
    internal static class HtmlHelper {
        internal static string CreateHeaderTitle(string text, HeaderSize headerSize = HeaderSize.H1) {
            var htmlTag = headerSize.ToString();
            return $"<{htmlTag}>{text}</{htmlTag}>";
        }

        internal static string SetBold(string text) =>
            $"<b>{text}</b>";
    }
}
