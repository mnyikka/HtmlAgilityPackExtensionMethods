using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScraper
{
    public static class HtmlAgilityPackExtensionMethods
    {
        /// <summary>
        /// Possible search flags to search by
        /// </summary>
        public enum SearchFlag 
        { 
            Name=2, 
            Type=8, 
            Id=16, 
            Value=32, 
            Action=64, 
            HtmlAttributeName=128,
            CustomAttribute = 256
        }

        /// <summary>
        /// String search mode, by default is equals
        /// </summary>
        public enum StringSearchMode
        {
            Equals=2,
            StartsWith=4,
            Contains=8,
            EndsWith=16
        }


        /// <summary>
        /// Search criteria object, adds multiple search capabilities
        /// </summary>
        public class SearchCriteria
        {
            List<SearchFlag> ListSearchFlags;
            List<StringSearchMode> ListStringSearchModes;
            List<string> ListSearchStrings;
            List<string> ListAttributeNames;
            bool AndComparisonOrComparison;
            StringSearchMode DefaultSearchMode;

            /// <summary>
            /// Creates a search criteria object
            /// </summary>
            /// <param name="AndComparisonOrComparison">If true, AND condition, else OR condition</param>
            public SearchCriteria(bool AndComparisonOrComparison, 
                StringSearchMode DefaultStringSearchMode)
            {
                this.InitLists();
                this.DefaultSearchMode = DefaultStringSearchMode;
                this.AndComparisonOrComparison = AndComparisonOrComparison;
            }

            private void InitLists()
            {
                int Capacity = 10;
                this.ListSearchFlags = new List<SearchFlag>(Capacity);
                this.ListSearchStrings = new List<string>(Capacity);
                this.ListAttributeNames = new List<string>(Capacity);
                this.ListStringSearchModes = new List<StringSearchMode>(Capacity);
            }


            /// <summary>
            /// New search criteria
            /// </summary>
            /// <param name="AndComparisonOrComparison">AND for AND comparisons</param>
            /// <param name="SearchFlags">Search flags</param>
            /// <param name="Flags">SearchMode</param>
            public SearchCriteria(bool AndComparisonOrComparison, 
                StringSearchMode DefaultStringSearchMode,
                SearchFlag SearchFlags, 
                string Flags)
            {
                this.InitLists();
                this.DefaultSearchMode = DefaultStringSearchMode;
                this.AndComparisonOrComparison = AndComparisonOrComparison;
                this.AddSearchByFlag(SearchFlags, 
                    Flags);
            }

            /// <summary>
            /// Adds search by flag. Default string search mode is used
            /// </summary>
            /// <param name="SearchFlags"></param>
            /// <param name="Flags"></param>
            public SearchCriteria AddSearchByFlag(SearchFlag SearchFlags, 
                string ValueToSearchFor)
            {
                this.AddSearchCriteria(SearchFlags,
                    ValueToSearchFor,
                    null,
                    this.DefaultSearchMode);
                return this;
            }






            /// <summary>
            /// Adds search criteria
            /// </summary>
            /// <param name="SearchBy">Searchby</param>
            /// <param name="SearchValue">IfSearch equals</param>
            public SearchCriteria AddSearchCriteria(SearchFlag SearchBy, 
                string SearchValue,
                string AttributeName,
                StringSearchMode StringSearchModeToUse)
            {
                if (SearchBy.HasFlag(SearchFlag.CustomAttribute) && string.IsNullOrEmpty(AttributeName) == true)
                    throw new Exception("If searching by custom attribute we must specify attribute name");

                this.ListSearchFlags.Add(SearchBy);
                this.ListSearchStrings.Add(SearchValue);
                this.ListAttributeNames.Add(AttributeName);
                this.ListStringSearchModes.Add(StringSearchModeToUse);
                return this;
            }



            /// <summary>
            /// Is Acceptable, returns true if all conditions evaluate to true
            /// and AndComparisonOrComparison is TRUE, or if AndComparisonOrComparison is false if one condition evaluates true
            /// </summary>
            /// <param name="CurrentNode"></param>
            /// <returns></returns>
            public bool IsAcceptable(HtmlAgilityPack.HtmlNode CurrentNode)
            {
                if (this.ListSearchStrings.Count == 0)
                    return true;
                
                //if and condition return only if all OKAY
                for (int index = 0; index < this.ListSearchStrings.Count; index++)
                {
                    SearchFlag flag_ = this.ListSearchFlags[index];
                    StringSearchMode stringComparisonMode = this.ListStringSearchModes[index];
                    string searchString = this.ListSearchStrings[index];
                    bool isOkay = false;

                    if (flag_.HasFlag(SearchFlag.Id))
                    {
                        string Id = CurrentNode.Id;
                        if ( HtmlAgilityPackExtensionMethods.CompareStrings(Id, searchString, stringComparisonMode) == true )
                        {
                            isOkay = true;
                        }
                        else
                        {
                            if (AndComparisonOrComparison == true)
                                return false;
                        }
                    }
                    if (isOkay == true && AndComparisonOrComparison == false)
                        return true;


                    if (flag_.HasFlag(SearchFlag.Name))
                    {
                        string Id = CurrentNode.Name;
                        if (HtmlAgilityPackExtensionMethods.CompareStrings(Id, searchString, stringComparisonMode) == true)
                        {
                            isOkay = true;
                        }
                        else
                        {
                            if (AndComparisonOrComparison == true)
                                return false;
                        }
                    }
                    if (isOkay == true && AndComparisonOrComparison == false)
                        return true;


                    if (flag_.HasFlag(SearchFlag.Type))
                    {
                        string Id = CurrentNode.GetAttributeValue("type",null);
                        if (HtmlAgilityPackExtensionMethods.CompareStrings(Id, searchString, stringComparisonMode) == true)
                        {
                            isOkay = true;
                        }
                        else
                        {
                            if (AndComparisonOrComparison == true)
                                return false;
                        }
                    }
                    if (isOkay == true && AndComparisonOrComparison == false)
                        return true;

                    if (flag_.HasFlag(SearchFlag.Value))
                    {
                        string Id = CurrentNode.GetAttributeValue("value", null);
                        if (HtmlAgilityPackExtensionMethods.CompareStrings(Id, searchString, stringComparisonMode) == true)
                        {
                            isOkay = true;
                        }
                        else
                        {
                            if (AndComparisonOrComparison == true)
                                return false;
                        }
                    }
                    if (isOkay == true && AndComparisonOrComparison == false)
                        return true;


                    if (flag_.HasFlag(SearchFlag.Action))
                    {
                        string Id = CurrentNode.GetAttributeValue("action", null);
                        if (HtmlAgilityPackExtensionMethods.CompareStrings(Id, searchString, stringComparisonMode) == true)
                        {
                            isOkay = true;
                        }
                        else
                        {
                            if (AndComparisonOrComparison == true)
                                return false;
                        }
                    }
                    if (isOkay == true && AndComparisonOrComparison == false)
                        return true;



                    if (flag_.HasFlag(SearchFlag.HtmlAttributeName))
                    {
                        string Id = CurrentNode.GetAttributeValue("name", null);
                        if (HtmlAgilityPackExtensionMethods.CompareStrings(Id, searchString, stringComparisonMode) == true)
                        {
                            isOkay = true;
                        }
                        else
                        {
                            if (AndComparisonOrComparison == true)
                                return false;
                        }
                    }


                    if (flag_.HasFlag(SearchFlag.CustomAttribute))
                    {
                        string customattribute = this.ListAttributeNames[index];
                        if(string.IsNullOrEmpty(customattribute) == true)
                        {
                            throw new Exception("Search flag by custom attribute must specify attribute name?");
                        }

                        string Id = CurrentNode.GetAttributeValue(customattribute, 
                            null);
                        if (HtmlAgilityPackExtensionMethods.CompareStrings(Id, searchString, stringComparisonMode) == true)
                        {
                            isOkay = true;
                        }
                        else
                        {
                            if (AndComparisonOrComparison == true)
                                return false;
                        }
                    }

                    if (isOkay == true && AndComparisonOrComparison == false)
                        return true;
                }


                if (AndComparisonOrComparison == false)
                    return false;

                return true;
            }

            /// <summary>
            /// Search criteria for all input elements, including selects, input
            /// </summary>
            /// <returns></returns>
            internal static SearchCriteria NewInputElementsCriteria()
            {
                SearchCriteria New = new SearchCriteria(false,
                    StringSearchMode.Equals);
                New.AddSearchCriteria(SearchFlag.Name, "input");
                New.AddSearchCriteria(SearchFlag.Name, "select");
                return New;
            }

            /// <summary>
            /// Adds a search criteria
            /// </summary>
            /// <param name="searchFlag">Search flag to use</param>
            /// <param name="ValueToSearchFor">Value to search for</param>
            public SearchCriteria AddSearchCriteria(SearchFlag searchFlag, 
                string ValueToSearchFor)
            {
                return this.AddSearchCriteria(searchFlag, 
                    ValueToSearchFor, 
                    null,
                    this.DefaultSearchMode);
            }

            /// <summary>
            /// Adds a search criteria
            /// </summary>
            /// <param name="searchFlag">Search flag to use</param>
            /// <param name="ValueToSearchFor">Value to search for</param>
            public SearchCriteria AddSearchByCustomAttribute(string AttributeName,
                string ValueToSearchFor)
            {
                return this.AddSearchCriteria(SearchFlag.CustomAttribute,
                    ValueToSearchFor,
                    AttributeName,
                    this.DefaultSearchMode);
            }

        }

        /// <summary>
        /// Returns all descendant nodes
        /// </summary>
        /// <param name="BeginNode">The starting node</param>
        /// <param name="IncludeSelf">If include self is set to true, self is returned too</param>
        /// <returns></returns>
        public static List<HtmlNode> GetAllDescendantNodes(HtmlNode BeginNode, bool IncludeSelf)
        {
            List<HtmlNode> NodeCollection = new List<HtmlNode>(1000);
            IEnumerable<HtmlNode> AllNodes = null;
            if (IncludeSelf == true)
            {
                AllNodes = BeginNode.DescendantsAndSelf();
            }
            else
            {
                AllNodes = BeginNode.Descendants();
            }

            foreach (HtmlNode N in AllNodes)
            {
                NodeCollection.Add(N);
            }

            return NodeCollection;   
        }


        /// <summary>
        /// Returns nodes with that name,say form etc
        /// </summary>
        /// <param name="Collection">Node collection to search for</param>
        /// <param name="CriteriaToSearch">Criteria to search for</param>
        /// <param name="Limit">A limit returned</param>
        /// <returns>Returns a collection filtered</returns>
        public static List<HtmlNode> GetAllDescendantNodes(List<HtmlNode> Collection,
            SearchCriteria CriteriaToSearch, 
            int Limit)
        {
            List<HtmlNode> Returned = new List<HtmlNode>(Collection.Count);
            for (int index = 0; index < Collection.Count; index++)
            {
                if (CriteriaToSearch.IsAcceptable(Collection[index]))
                {
                    Returned.Add(Collection[index]);
                }
                if (Returned.Count == Limit)
                    return Returned;
            }
            return Returned;
        }


        /// <summary>
        /// Returns nodes with that name,say form etc
        /// </summary>
        /// <param name="Collection">Node collection to search for</param>
        /// <param name="CriteriaToSearch">Criteria to search for</param>
        /// <param name="Limit">A limit returned</param>
        /// <returns>Returns a collection filtered</returns>
        public static HtmlNode GetSingleNode(HtmlNode Node,
            SearchCriteria CriteriaToSearch,
            bool IncludeNodeItself)
        {
            List<HtmlNode> Collection = new List<HtmlNode>(1000);
            Collection = HtmlAgilityPackExtensionMethods.GetAllDescendantNodes(Node, IncludeNodeItself);
            Collection = HtmlAgilityPackExtensionMethods.GetAllDescendantNodes(Collection, CriteriaToSearch, 1);
            if (Collection.Count > 0)
                return Collection[0];
            return null;
        }


        /// <summary>
        /// Returns all the descendant nodes
        /// </summary>
        /// <param name="FormNode"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        internal static List<HtmlNode> GetAllDescendantNodes(HtmlNode FormNode, 
            SearchCriteria searchCriteria, 
            int Limit)
        {
            List<HtmlNode> Collection = new List<HtmlNode>(1000);
            Collection = HtmlAgilityPackExtensionMethods.GetAllDescendantNodes(FormNode,true);
            return HtmlAgilityPackExtensionMethods.GetAllDescendantNodes(Collection,
                searchCriteria,
                Limit);
        }

        /// <summary>
        /// Returns a single node matching that
        /// </summary>
        /// <param name="Collection1"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        internal static HtmlNode GetSingleNode(List<HtmlNode> Collection1, SearchCriteria searchCriteria)
        {
            List<HtmlNode> Collection = new List<HtmlNode>(1000);
            Collection = HtmlAgilityPackExtensionMethods.GetAllDescendantNodes(Collection1, searchCriteria, 1);
            if (Collection.Count > 0)
                return Collection[0];
            return null;
        }

        /// <summary>
        /// Replaces the entities in the POSTDATA STRING with value attributes from these elements
        /// </summary>
        /// <param name="Collection1">A collection containing html node elements</param>
        /// <param name="URLENCODEDPOSTDATA">Encoded POSTDATA</param>
        /// <param name="ReplaceOnlyIfTheElementsHaveValues">If set to true, HtmlNodes with no values are ignored</param>
        /// <returns></returns>
        internal static string ReplacePostDataEntriesWithValuesFromElements(List<HtmlNode> Collection1, 
            string URLENCODEDPOSTDATA,
            bool ReplaceOnlyIfTheElementsHaveValues,
            out int ReplaceMentsCount
            )
        {
            ReplaceMentsCount = 0;
            string[] Array = URLENCODEDPOSTDATA.Split(new char[] { '&' }, 
                StringSplitOptions.None);
            for (int index = 0; index < Array.Length; index++)
            {
                string[] Array1 = Array[index].Split(new char[] { '=' }, 2, StringSplitOptions.None);
                if (Array1.Length > 0)
                {
                    string NameToSearch = Array1[0];
                    NameToSearch = System.Net.WebUtility.UrlDecode(NameToSearch);
                    HtmlNode NodeToGet = HtmlAgilityPackExtensionMethods.GetSingleNode(Collection1,
                        new SearchCriteria(true, StringSearchMode.Equals, 
                            SearchFlag.HtmlAttributeName, 
                            NameToSearch));
                    if (NodeToGet == null)
                        continue;
                    string NodeValue = NodeToGet.GetAttributeValue("value", 
                        null);
                    if (string.IsNullOrEmpty(NodeValue) && ReplaceOnlyIfTheElementsHaveValues == true)
                        continue;
                    NodeValue = ("" + NodeValue);
                    Array[index] = Array1[0] + "=" + System.Net.WebUtility.UrlEncode(NodeValue);
                    ReplaceMentsCount = ReplaceMentsCount + 1;
                }
            }

            StringBuilder NewBuild = new StringBuilder(URLENCODEDPOSTDATA.Length);
            for (int index = 0; index < Array.Length; index++)
            {
                NewBuild.Append(Array[index]);
                if (index + 1 < Array.Length)
                    NewBuild.Append("&");
            }

            return NewBuild.ToString();
        }

        /// <summary>
        /// Returns the table element as rows and columns
        /// colspan rules arent followed, simply the tds found in every tr of the table structure
        /// </summary>
        /// <param name="ContainerNode">The table node to use</param>
        /// <returns>Returns a list of string arrays</returns>
        public static List<string[]> GetElementTRRowsAndTDColumns(HtmlAgilityPack.HtmlNode ContainerNode)
        {
            List<HtmlAgilityPack.HtmlNode> RowNodes = HtmlAgilityPackExtensionMethods.GetAllDescendantNodes(ContainerNode,
                new SearchCriteria(true, StringSearchMode.Equals).AddSearchByFlag(SearchFlag.Name,
                "tr"),
                -1);
            List<string[]> ROWS = new List<string[]>(RowNodes.Count);
            for (int index = 0; index < RowNodes.Count; index++)
            {
                List<HtmlAgilityPack.HtmlNode> ColumnNodes = HtmlAgilityPackExtensionMethods.GetAllDescendantNodes(RowNodes[index],
                new SearchCriteria(true,StringSearchMode.Equals).AddSearchByFlag(SearchFlag.Name,
                "td"),
                -1);
                string[] Columns = new string[ColumnNodes.Count];
                for (int columnindex = 0; columnindex < ColumnNodes.Count; columnindex++)
                {
                    Columns[columnindex] = HtmlHelper.HtmlStripTags(ColumnNodes[columnindex].InnerHtml, 
                        true, 
                        true).Trim();
                }
                ROWS.Add(Columns);
            }

            return ROWS;
        }

        /// <summary>
        /// Compares strings
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="searchString"></param>
        /// <param name="stringComparisonMode"></param>
        /// <returns></returns>
        internal static bool CompareStrings(string Id, 
            string searchString, 
            StringSearchMode stringComparisonMode)
        {
            if (Id == null)
                return false;

            if (stringComparisonMode.HasFlag(StringSearchMode.Equals))
            {
                if (Id.Equals(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }


            if (stringComparisonMode.HasFlag(StringSearchMode.StartsWith))
            {
                if (Id.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            if (stringComparisonMode.HasFlag(StringSearchMode.Contains))
            {
                if (Id.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1)
                    return true;
            }


            if (stringComparisonMode.HasFlag(StringSearchMode.EndsWith))
            {
                if (Id.EndsWith(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
