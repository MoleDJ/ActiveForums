using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace DotNetNuke.Modules.ActiveForums.Tapatalk.Classes
{
    public class Helpers
    {
        public enum ProcessModes { Normal, TextOnly, Quote }

        public static string GetSummary(string summary, string body)
        {
            var result = !string.IsNullOrWhiteSpace(summary) ? summary : body;

            result = result + string.Empty;

            result = HttpUtility.HtmlDecode(Utilities.StripHTMLTag(result));

            result = result.Length > 200 ? result.Substring(0, 200) : result;

            return result.Trim();
        }

        public static string TapatalkToHtml(string input)
        {
            input = input.Replace("<", "&lt;");
            input = input.Replace(">", "&gt;");

            input = input.Replace("\r\n", "\n");
            input = input.Trim(new[] { ' ', '\n', '\r', '\t' }).Replace("\n", "<br />");

            input = Regex.Replace(input, @"\[quote\=\'([^\]]+?)\'\]", "<blockquote class='afQuote'><span class='afQuoteTitle'>$1</span><br />", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            input = Regex.Replace(input, @"\[quote\=\""([^\]]+?)\""\]", "<blockquote class='afQuote'><span class='afQuoteTitle'>$1</span><br />", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            input = Regex.Replace(input, @"\[quote\=([^\]]+?)\]", "<blockquote class='afQuote'><span class='afQuoteTitle'>$1</span><br />", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            input = Regex.Replace(input, @"\[quote\]", "<blockquote class='afQuote'>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\[\/quote\]", "</blockquote>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            input = Regex.Replace(input, @"\[img\](.+?)\[\/img\]", "<img src='$1' />", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\[url=\'(.+?)\'\](.+?)\[\/url\]", "<a href='$1'>$2</a>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\[url=\""(.+?)\""\](.+?)\[\/url\]", "<a href='$1'>$2</a>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\[url=(.+?)\](.+?)\[\/url\]", "<a href='$1'>$2</a>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\[url\](.+?)\[\/url\]", "<a href='$1'>$1</a>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\[(\/)?b\]", "<$1strong>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            input = Regex.Replace(input, @"\[(\/)?i\]", "<$1i>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return input;
        }

        public static string HtmlToTapatalk(string input, bool returnHtml)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            input = Regex.Replace(input, @"\s+", " ", RegexOptions.Multiline);

            input = EncodeUnmatchedBrackets(input);

            var htmlBlock = new HtmlDocument();
            htmlBlock.LoadHtml(input);

            var tapatalkMarkup = new StringBuilder();

            ProcessNode(tapatalkMarkup, htmlBlock.DocumentNode, ProcessModes.Normal, returnHtml);

            return tapatalkMarkup.ToString().Trim(new[] { ' ', '\n', '\r', '\t' });
        }

        public static string HtmlToTapatalkQuote(string postedBy, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            input = Regex.Replace(input, @"\s+", " ", RegexOptions.Multiline);

            input = EncodeUnmatchedBrackets(input);

            var htmlBlock = new HtmlDocument();
            htmlBlock.LoadHtml(input);

            var tapatalkMarkup = new StringBuilder();

            ProcessNode(tapatalkMarkup, htmlBlock.DocumentNode, ProcessModes.Quote, false);

            return string.Format("[quote={0}]\r\n{1}\r\n[/quote]\r\n", postedBy, tapatalkMarkup.ToString().Trim(new[] { ' ', '\n', '\r', '\t' }));
        }

        public static void ProcessNodes(StringBuilder output, IEnumerable<HtmlNode> nodes, ProcessModes mode, bool returnHtml)
        {
            foreach (var node in nodes)
                ProcessNode(output, node, mode, returnHtml);
        }

        public static void ProcessNode(StringBuilder output, HtmlNode node, ProcessModes mode, bool returnHtml)
        {
            var lineBreak = returnHtml ? "<br />" : "\r\n"; // (mode == ProcessModes.Quote) ? "\n" : "<br /> ";

            if (node == null || output == null || (mode == ProcessModes.TextOnly && node.Name != "#text"))
                return;

            switch (node.Name)
            {
                // No action needed for these node types
                case "#text":
                    var text = HttpUtility.HtmlDecode(node.InnerHtml);
                    //if (mode != ProcessModes.Quote)
                    //    text = HttpContext.Current.Server.HtmlEncode(text);
                    text = text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                    output.Append(text);
                    return;

                case "tr":
                    ProcessNodes(output, node.ChildNodes, mode, returnHtml);
                    output.Append(lineBreak);
                    return;

                case "script":
                    return;

                case "ol":
                case "ul":

                    if (mode != ProcessModes.Normal)
                        return;

                    output.Append(lineBreak);

                    var listItemNodes = node.SelectNodes("//li");

                    for (var i = 0; i < listItemNodes.Count; i++)
                    {
                        var listItemNode = listItemNodes[i];
                        output.AppendFormat("{0} ", node.Name == "ol" ? (i + 1).ToString() : "*");
                        ProcessNodes(output, listItemNode.ChildNodes, mode, returnHtml);
                        output.Append(lineBreak);
                    }

                    return;

                case "li":

                    if (mode == ProcessModes.Quote)
                        return;

                    output.Append("* ");
                    ProcessNodes(output, node.ChildNodes, mode, returnHtml);
                    output.Append(lineBreak);
                    return;

                case "p":
                    ProcessNodes(output, node.ChildNodes, mode, returnHtml);
                    output.Append(lineBreak);
                    return;

                case "b":
                case "strong":

                    if (mode != ProcessModes.Quote)
                    {
                        output.Append("<b>");
                        ProcessNodes(output, node.ChildNodes, mode, returnHtml);
                        output.Append("</b>");
                    }
                    else
                    {
                        output.Append("[b]");
                        ProcessNodes(output, node.ChildNodes, mode, returnHtml);
                        output.Append("[/b]");
                    }

                    return;

                case "i":
                    if (mode != ProcessModes.Quote)
                    {
                        output.Append("<i>");
                        ProcessNodes(output, node.ChildNodes, mode, returnHtml);
                        output.Append("</i>");
                    }
                    else
                    {
                        output.Append("[i]");
                        ProcessNodes(output, node.ChildNodes, mode, returnHtml);
                        output.Append("[/i]");
                    }

                    return;

                case "blockquote":

                    if (mode != ProcessModes.Normal)
                        return;

                    output.Append("[quote]");
                    ProcessNodes(output, node.ChildNodes, mode, returnHtml);
                    output.Append("[/quote]" + lineBreak);
                    return;

                case "br":
                    output.Append(lineBreak);
                    return;


                case "img":

                    var src = node.Attributes["src"];
                    if (src == null || string.IsNullOrWhiteSpace(src.Value))
                        return;

                    var isEmoticon = src.Value.IndexOf("emoticon", 0, StringComparison.InvariantCultureIgnoreCase) >= 0;

                    var url = src.Value.Trim();
                    var request = HttpContext.Current.Request;

                    // Make a fully qualifed URL
                    if (!url.ToLower().StartsWith("http"))
                    {
                        var rootDirectory = url.StartsWith("/") ? string.Empty : "/";
                        url = string.Format("{0}://{1}{2}{3}", request.Url.Scheme, request.Url.Host, rootDirectory, url);
                    }

                    if (mode == ProcessModes.Quote && isEmoticon)
                        return;

                    output.AppendFormat(isEmoticon ? "<img src='{0}' />" : "[img]{0}[/img]", url);

                    return;

                case "a":

                    var href = node.Attributes["href"];
                    if (href == null || string.IsNullOrWhiteSpace(href.Value))
                        return;

                    output.AppendFormat("[url={0}]", href.Value);
                    ProcessNodes(output, node.ChildNodes, ProcessModes.TextOnly, returnHtml);
                    output.Append("[/url]");

                    return;


            }

            ProcessNodes(output, node.ChildNodes, mode, returnHtml);
        }
        public static string GetAvatarUrl(int userId)
        {
            const string urlTemplate = "{0}://{1}{2}";
            const string profilePathTemplate = "~/profilepic.ashx";

            var request = HttpContext.Current.Request;

            var profilePath = string.Format(urlTemplate, request.Url.Scheme, request.Url.Host, VirtualPathUtility.ToAbsolute(profilePathTemplate));

            return string.Format("{0}?userId={1}&w=64&h=64", profilePath, userId);
        }

        public static string EncodeUnmatchedBrackets(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var sb = new StringBuilder(input);

            var index = GetFirstUnmatchedBracket(input);

            while (index >= 0)
            {
                var bracket = sb[index];
                sb.Remove(index, 1);
                sb.Insert(index, (bracket == '<') ? "&lt;" : "&gt;");

                index = GetFirstUnmatchedBracket(sb.ToString());
            }

            return sb.ToString();
        }

        public static int GetFirstUnmatchedBracket(string text)
        {
            var m = Regex.Match(text, @"^(?>\<(?<X>)|\>(?<-X>)|(?!\<|\>).)+(?(X)(?!))");
            return m.Length < text.Length ? m.Length : -1;
        }
    }
}
