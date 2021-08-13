using RauchTech.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RauchTech.Common.Extensions
{
    public static class PagingExtensions
    {
        public static List<(string, bool)> ToTupleOrder(this string[] sorts, Dictionary<string, string> columnsLibrary = null)
        {
            return sorts.ToList().ToTupleOrder(columnsLibrary);
        }

        public static List<(string, bool)> ToTupleOrder(this List<string> sorts, Dictionary<string, string> columnsLibrary = null)
        {
            List<(string, bool)> orderBy;
            string[] pieces;

            orderBy = new List<(string, bool)>();

            foreach (string s in sorts.Select(x => x.ToLower()))
            {
                if (s.Contains("_"))
                {
                    pieces = s.Split("_");
                }
                else if (s.Contains(" "))
                {
                    pieces = s.Split(" ");
                }
                else
                {
                    pieces = (s + "_asc").Split("_");
                }

                if (columnsLibrary?.Count > 0)
                {
                    if (columnsLibrary.TryGetValue(pieces[0], out string dbColumn))
                    {
                        orderBy.Add((dbColumn, pieces[1] == "asc"));
                    }
                }
                else
                {
                    orderBy.Add((pieces[0], pieces[1] == "asc"));
                }
            }

            return orderBy;
        }
    }
}
