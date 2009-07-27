using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TeleTwitter.Lib;

namespace Twarkee
{
    internal static class StatusFormatter
    {
        private static DateTime _lastItem = DateTime.MinValue;

        #region HTML Formatting Utilities

        public static string FormatStatusHtml(Status inputStatus)
        {
            StringBuilder output = new StringBuilder();
            string tlitemClass = "tlitem";

            // If your user name is found in the text of any status, change the style.
            if (inputStatus.Text.ToLower().Contains(Properties.Settings.Default.UserName.ToLower()))
            {
                tlitemClass = "tlitemAlert";
            }
            else if (inputStatus.User.ScreenName == TwitterManager.Instance().CurrentUser.ScreenName)
            {
                tlitemClass = "tlitemMine";
            }

            // Build the output
            output.Append("<tweet><div class=\"" + tlitemClass + "\">");
            output.Append("    <div class=\"tlimage\">");
            output.Append("        <img onclick=\"window.external.SetReply('" + inputStatus.User.ScreenName + "');\" src=\"" + inputStatus.User.ProfileImageUrl + "\" alt=\"" + inputStatus.User.Name + " (" + inputStatus.User.ScreenName + ")\" />");
            output.Append("    </div>");
            output.Append("    <div class=\"tldetails\">");
            output.Append("        <h2>" + inputStatus.User.ScreenName + "</h2>");
            output.Append("        <span class=\"tlcommands\">");
            output.Append("             <span class=\"tlcreated\">" + inputStatus.CreatedAt.ToShortTimeString() + "</span><br />");
            output.Append("             <span class=\"directReply\" onclick=\"window.external.SetReplyCommand('" + inputStatus.User.ScreenName + "','d');\">Direct Reply</span><br />");
            output.Append("             <span class=\"publicReply\" onclick=\"window.external.SetReply('" + inputStatus.User.ScreenName + "');\">Public Reply</span>");
            output.Append("        </span>");
            output.Append("        <div class=\"tltext\">" + Format(inputStatus.Text) + "</div>");
            output.Append("    </div>");
            output.Append("</div></tweet>");
            return output.ToString();
        }

        public static string FormatTimelineHtml(List<Status> statuses)
        {
            StringBuilder output = new StringBuilder();
            DateTime lastDateTime = _lastItem;

            if (statuses.Count > 0)
            {
                if (statuses[0].CreatedAt > _lastItem)
                {
                    _lastItem = statuses[0].CreatedAt;
                }
                else
                    return string.Empty;
            }
            output.Append("<tweets>");
            foreach (Status status in statuses)
            {
                if (status.CreatedAt <= lastDateTime)
                {
                    break;
                }

                output.Append(FormatStatusHtml(status));
            }
            output.Append("</tweets>");
            return output.ToString();
        }

        #endregion

        #region General Text Utilities

        public static string Format(string text)
        {
            string formattedOutput = HttpUtility.HtmlEncode(text);
            formattedOutput = FormatLinks(formattedOutput);
            return formattedOutput;
        }


        private static string FormatLinks(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            //Find any links
            string pattern = @"(\s|^)(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])(\s|$)";
            MatchCollection matchs;
            StringCollection uniqueMatches = new StringCollection();

            matchs = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (Match m in matchs)
            {
                if (!uniqueMatches.Contains(m.ToString()))
                {
                    string link = m.ToString().Trim();
                    if (link.Length > 30)
                    {
                        try
                        {
                            Uri uri = new Uri(link);
                            string absolutePath = uri.AbsolutePath.EndsWith("/")
                                            ? uri.AbsolutePath.Substring(0, uri.AbsolutePath.Length - 1)
                                            : uri.AbsolutePath;

                            int slashIndex = absolutePath.LastIndexOf("/");
                            if (slashIndex > -1)
                            {
                                absolutePath = "/..." + absolutePath.Substring(slashIndex);
                            }

                            if (absolutePath.Length > 20)
                            {
                                absolutePath = absolutePath.Substring(0, 20);
                            }

                            link = uri.Host + absolutePath;
                        }
                        catch
                        {
                        }
                    }
                    text = text.Replace(m.ToString(), " <a onclick=\" return openLinkInDefaultBrowser('" + m.ToString().Trim() + "')\" target=\"_blank\" href=\"" + "javascript:void();" + "\">" + link + "</a> ");
                    uniqueMatches.Add(m.ToString());
                }
            }

            pattern = "@*\\w";
            matchs = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (Match m in matchs)
            {
                string link = m.ToString().Trim();
               // text = text.Replace(m.ToString(), " <a onclick=\" return openPersonInDefaultBrowser('" + (m.ToString()).Substring(1).Trim() + "')\" target=\"_blank\" href=\"" + "javascript:void();" + "\">" + link + "</a> ");
            }

            return text;
        }
        
        #endregion
    }
}
