using System;
using System.Collections.Generic;

namespace EventTypeCfg
{
/// <summary>
    /// Process level 2 Search and Replace
    /// </summary>
    class Level2Process
    {
        /*********************** Search ***********************/
        /// <summary>
        /// level 2 search criteria
        /// </summary>
        public static bool Search(string data, string criteria)
        {
            try
            {
                // if data empty and criteria empty then this is OK.
                if ((data == null || data.Trim() == string.Empty) && criteria == string.Empty) return true;

                // if data is empty and criteria is not well this is not OK.
                if ((data == null || data.Trim() == string.Empty) && criteria != string.Empty) return false;

                // remove \t, \n and \r\t chars
                criteria = RemoveUnwantedChars(criteria);

                // split by );
                string[] criteriaList = criteria.Split(new[] { ");" }, StringSplitOptions.RemoveEmptyEntries);

                bool searchValidation = true;

                foreach (string crit in criteriaList)
                {
                    if (crit.ToLower().TrimStart().StartsWith("contains("))
                        searchValidation = Contains(data, crit);
                    else if (crit.ToLower().TrimStart().StartsWith("starts_with("))
                        searchValidation = StartsWith(data, crit);
                    else if (crit.ToLower().TrimStart().StartsWith("ends_with("))
                        searchValidation = EndsWith(data, crit);

                    if (!searchValidation)
                        return false;
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool EndsWith(string data, string criteria)
        {
            try
            {
                Dictionary<string, string> crit = ProcessCriteria(criteria);

                return data.EndsWith(crit["part1"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Contains error:" + ex.Message);
            }
        }

        private static bool Contains(string data, string criteria)
        {
            try
            {
                Dictionary<string, string> crit = ProcessCriteria(criteria);

                return data.Contains(crit["part1"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Contains error:" + ex.Message);
            }
        }

        private static bool StartsWith(string data, string criteria)
        {
            try
            {
                Dictionary<string, string> crit = ProcessCriteria(criteria);

                return data.StartsWith(crit["part1"]);
            }
            catch (Exception ex)
            {
                throw new Exception("StartsWith error:" + ex.Message);
            }
        }

        /*********************** Replace Criteria ***********************/
        /// <summary>
        /// level 2 replace criteria
        /// </summary>
        public static string ReplaceCriteria(string data, string criteria)
        {
            try
            {
                if (data == null || data.Trim() == string.Empty) return string.Empty;

                // remove \t, \n and \r\t chars
                criteria = RemoveUnwantedChars(criteria);

                // split by );
                string[] criteriaList = criteria.Split(new[] { ");" }, StringSplitOptions.RemoveEmptyEntries);

                // out message
                string result = data;

                foreach (string crit in criteriaList)
                {
                    if (crit.ToLower().TrimStart().StartsWith("replace"))
                        result = Replace(result, crit);
                    else if (crit.ToLower().TrimStart().StartsWith("from_start_remove_from_to"))
                        result = FromStartRemoveFromTo(result, crit);
                    else if (crit.ToLower().TrimStart().StartsWith("from_end_remove_from_to"))
                        result = FromEndRemoveFromTo(result, crit);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Replace strings
        /// </summary>
        private static string Replace(string data, string criteria)
        {
            //"replace('NoBusiness Service '','Business Service ');"
            try
            {
                Dictionary<string, string> crit = ProcessCriteria(criteria);

                return data.Replace(crit["part1"], crit["part2"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Replace error:" + ex.Message);
            }
        }

        /// <summary>
        /// Remove from start to end
        /// </summary>
        private static string FromStartRemoveFromTo(string data, string criteria)
        {
            //"from_start_remove_from_to('NoBusiness Service '','Business Service ');"
            try
            {
                Dictionary<string, string> crit = ProcessCriteria(criteria);

                string part1 = crit["part1"];
                string part2 = crit["part2"];

                int part1Lenght = part1.Length;
                int part2Lenght = part2.Length;

                int part1Pos = data.IndexOf(part1, StringComparison.Ordinal);
                int part2Pos = data.IndexOf(part2, StringComparison.Ordinal);

                if (part2 == string.Empty)
                    return data.Remove(0, part1Pos + part1Lenght);

                //------------------------------------------------------------
                return data.Substring(0, part1Pos) + data.Substring(part2Pos + part2Lenght);
            }
            catch (Exception ex)
            {
                throw new Exception("From start remove error:" + ex.Message);
            }
        }

        /// <summary>
        /// Remove from end to start
        /// </summary>
        private static string FromEndRemoveFromTo(string data, string criteria)
        {
            //"from_end_remove_from_to('NoBusiness Service '','Business Service ');"
            try
            {
                Dictionary<string, string> crit = ProcessCriteria(criteria);

                string part1 = crit["part1"];
                string part2 = crit["part2"];

                int part1Lenght = part1.Length;
                int part2Lenght = part2.Length;

                int part1Pos = data.LastIndexOf(part1, StringComparison.Ordinal);
                int part2Pos = data.LastIndexOf(part2, StringComparison.Ordinal);

                return part2 == string.Empty
                    ? data.Remove(part1Pos)
                    : data.Remove(part2Pos, (part1Pos - part2Pos) + part1Lenght);
            }
            catch (Exception ex)
            {
                throw new Exception("From end remove error:" + ex.Message);
            }
        }

        /*********************** Generic ***********************/
        /// <summary>
        /// process criteria information
        /// </summary>
        private static Dictionary<string, string> ProcessCriteria(string criteria)
        {
            Dictionary<string, string> crit = new Dictionary<string, string>
            {
                {"part1", string.Empty},
                {"part2", string.Empty}
            };

            //"replace('NoBusiness Service '','Business Service ');"
            try
            {
                // remove operation name
                criteria = criteria.Remove(0, criteria.IndexOf('(') + 1);               //"'NoBusiness Service '','Business Service '"
                // remove last '
                criteria = criteria.Substring(0, criteria.LastIndexOf('\''));           //"'NoBusiness Service '','Business Service "
                // remove spaces until first '
                criteria = criteria.TrimStart();                                        //"'NoBusiness Service '','Business Service "

                // if does not starts with ' then leave empty
                if (!criteria.StartsWith("\'")) return crit;

                // remove first '
                criteria = criteria.Substring(1);                                       //"NoBusiness Service '','Business Service "

                // get divider
                int dividerPos = criteria.IndexOf("',", StringComparison.Ordinal);      // Get ', position

                // if no divider exists then return criteria in part1
                if (dividerPos < 0) { crit["part1"] = criteria; return crit; }

                // ------------------------------------------------------------------------
                string part1 = criteria.Substring(0, dividerPos);                   //"NoBusiness Service '" 

                //if (!part1.EndsWith("\'")) return crit;                               // if string does not end with ' then return original data
                //part1 = part1.Substring(0, part1.LastIndexOf('\''));                  //"NoBusiness Service "  

                crit["part1"] = part1;

                // ------------------------------------------------------------------------<
                string part2 = criteria.Substring(dividerPos + 2).TrimStart();          //"'Business Service "

                if (!part2.StartsWith("\'")) return crit;                               // if string does not start with ' then return original data

                part2 = part2.Substring(1);                                             //"Business Service "

                crit["part2"] = part2;

                // ------------------------------------------------------------------------<
                return crit;
            }
            catch (Exception ex)
            {
                throw new Exception("ProcessCriteria error:" + ex.Message);
            }
        }

        /// <summary>
        /// Remove spaces and tabs from string.
        /// </summary>
        private static string RemoveUnwantedChars(string data)
        {
            data = data.Replace("\r\n", " ");
            data = data.Replace("\t", " ");
            data = data.Replace("\n", " ");

            return data.Trim();
        }
    }
}
