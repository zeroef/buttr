using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Buttr.Data.Repositories;
using Buttr.Domain;
using HtmlAgilityPack;

namespace Buttr.Service
{
    public interface IZipService
    {
        List<ZipCode> GetZipCodesByFipsCode(string fipsCode);
        List<string> GetZips(List<string> fipsCodes, List<string> excludeList, List<string> includeList);
    }

    public class ZipService : IZipService
    {
        public List<string> GetZips(List<string> fipsCodes, List<string> excludeList, List<string> includeList )
        {
            var zipCodeRepository = new ZipCodeRepository();

            var zipsFound = new List<ZipCode>();
            foreach (var code in fipsCodes)
            {
                //var html = GetWebPage(code);
                //zipsFound.AddRange(GetZipCodesFromHtml(html));

                zipsFound.AddRange(zipCodeRepository.GetZipCodesByFipsCode(code));
            }

            var includedZips = CreateIncludeList(includeList);

            if (includedZips != null)
            {
                zipsFound.RemoveAll(x => !includedZips.Contains(x.ZipCodeId));    
            }

            var excludedZips = CreateExcludeList(excludeList);            

            if (excludedZips != null)
            {
                zipsFound.RemoveAll(x => excludedZips.Contains(x.ZipCodeId));    
            }

            return zipsFound.Select(x => x.ZipCodeId).ToList();
        }

        public List<string> CreateIncludeList(List<string> includes)
        {
            return CreateExcludeList(includes);
        }

        public List<string> CreateExcludeList(List<string> excludes)
        {
            if (!excludes.Any()) { return null; }
            
            var excludeList = new List<string>();

            foreach (var exclude in excludes)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                var foo = rgx.Replace(exclude, string.Empty).Trim();

                if (foo.Contains("-"))
                {
                    AddRangeOf(foo, excludeList);
                    continue;
                }

                if (foo.Length == 5)
                {
                    excludeList.Add(foo);
                    continue;
                }

                if (foo.Length < 5)
                {
                    AddRangeStartsWith(foo, excludeList);
                }
            }

            return excludeList;
        }

        public void AddRangeStartsWith(string range, List<string> list)
        {
            var start = PadZeroes(range);
            var end = PadNines(range);

            AddRangeOf(start, end, list);
        }

        public void AddRangeOf(string start, string end, List<string> list)
        {
            int startInt;
            int endInt;
            Int32.TryParse(start, out startInt);
            Int32.TryParse(end, out endInt);

            int count = (endInt + 1) - startInt;

            if (count < 0)
            {
                throw new Exception("End zip was less than start zip in range: " + start + "-" + end);
            }

            var excludeRange = new List<string>();
            excludeRange.AddRange(Enumerable.Range(startInt, count).Select(n => n.ToString()).ToList());

            list.AddRange(excludeRange);
        }

        public void AddRangeOf(string range, List<string> list)
        {
            string[] ranges = range.Split(new []{'-'});
            var start = PadZeroes(ranges[0]);
            var end = PadNines(ranges[1]);
            
            AddRangeOf(start, end, list);
        }

        public string PadZeroes(string str)
        {
            const char pad = '0';
            return str.PadRight(5, pad);
        }
        
        public string PadNines(string str)
        {
            const char pad = '9';
            return str.PadRight(5, pad);
        }


        public List<ZipCode> GetZipCodesByFipsCode(string fipsCode)
        {
            var foo = new List<ZipCode>();

            var bar = GetWebPage(fipsCode);

            var foobar = GetZipCodesFromHtml(bar);

            return foo;
        }


        public string GetWebPage(string queryParam)
        {
            string markup;
            string url = string.Format("http://www.melissadata.com/lookups/CountyZip.asp?fips={0}", queryParam);

            using (var client = new WebClient())
            {
                markup = client.DownloadString(url);
            }

            return markup;
        }

        public List<ZipCode> GetZipCodesFromHtml(string html)
        {
             HtmlDocument doc = new HtmlDocument();
            
             doc.LoadHtml(html);

             var zips = doc.DocumentNode.SelectNodes("//table[@class='Tableresultborder' and @width='650']/tr")
                         .Skip(3)
                         .Select(tr => tr.Elements("td").Select(td => td.InnerText).ToList())
                         .Select(td => new ZipCode() { ZipCodeId = td[0], City = td[1]})
                         .ToList();

            return zips;
        }
    }
}
