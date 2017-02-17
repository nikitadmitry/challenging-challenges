using System;
using System.Web;
using System.Web.Mvc;
using MarkdownSharp;

namespace Presentation.Legacy.Helpers
{
    /// <summary>
    /// Helper class for transforming Markdown.
    /// </summary>
    public static class MarkdownHelper
    {
        /// <summary>
        /// An instance of the Markdown class that performs the transformations.
        /// </summary>
        static Markdown markdownTransformer = new Markdown();

        /// <summary>
        /// Transforms a string of Markdown into HTML.
        /// </summary>
        /// <param name="helper">HtmlHelper - Not used, but required to make this an extension method.</param>
        /// <param name="text">The Markdown that should be transformed.</param>
        /// <returns>The HTML representation of the supplied Markdown.</returns>
        public static IHtmlString Markdown(this HtmlHelper helper, string text)
        {
            string html = String.Empty;
            try
            {
                html = markdownTransformer.Transform(text);
            }
            catch
            {
                // ignored
            }
            return new MvcHtmlString(html);
        }
    }
}
