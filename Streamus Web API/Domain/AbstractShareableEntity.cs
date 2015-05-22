using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Streamus_Web_API.Domain.Interfaces;

namespace Streamus_Web_API.Domain
{
    public abstract class AbstractShareableDomainEntity : AbstractDomainEntity<Guid>, IShareableEntity
    {
        public const int MaxEntityTypeLength = 25;
        public const int MaxShortIdLength = 12;
        public const int MaxUrlFriendlyTitleLength = 40;

        public virtual string Title { get; set; }

        /// <summary>
        ///     Grab the first 40 characters of the title and then trim back to the last whole word.
        ///     Replace spaces with underscores.
        /// </summary>
        /// <returns>A URL friendly title (e.g.): as_one_of_the_few_members_of_congress_who_have </returns>
        public virtual string GetUrlFriendlyTitle()
        {
            //  Foreign characters can't be shown correctly so slugify the title to make it manageable.
            string urlFriendlyTitle = Slugify(Title);

            urlFriendlyTitle = urlFriendlyTitle.Substring(0, Math.Min(urlFriendlyTitle.Length, MaxUrlFriendlyTitleLength));

            if (urlFriendlyTitle.Length < Title.Length)
            {
                //  Might've truncated a word -- don't include that in the URL.
                int indexOfLastSpace = urlFriendlyTitle.LastIndexOf(" ");

                if (indexOfLastSpace > -1)
                {
                    urlFriendlyTitle = urlFriendlyTitle.Substring(0, indexOfLastSpace);
                }
            }
            
            return urlFriendlyTitle;
        }

        /// <summary>
        ///     Just trim off some of the Guid to make it pretty for the URL.
        ///     Essentially guaranteed unique when performing lookup with shortId + UrlFriendlyTitle
        /// </summary>
        /// <returns>12 digits of Guid without hyphen</returns>
        public virtual string GetShortId()
        {
            return Id.ToString().Replace("-", string.Empty).Substring(0, MaxShortIdLength);
        }

        public static string RemoveAccent(string text)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);

            return Encoding.ASCII.GetString(bytes);
        }

        //  Inspired by: http://stackoverflow.com/questions/3275242/how-do-you-remove-invalid-characters-when-creating-a-friendly-url-ie-how-do-you
        public static string Slugify(string text)
        {
            IdnMapping idnMapping = new IdnMapping();
            text = idnMapping.GetAscii(text);

            text = RemoveAccent(text).ToLower();

            //  Remove all invalid characters.  
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            //  Convert multiple spaces into one space
            text = Regex.Replace(text, @"\s+", " ").Trim();

            //  Replace spaces by underscores.
            text = Regex.Replace(text, @"\s", "_");

            return text;
        }
    }
}
