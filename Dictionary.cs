using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace NarrativeArc
{


    public class DictionaryMetaObject
    {
        public string DictionaryName { get; set; }
        public string DictionaryCategoryPrefix { get; set; }
        public string DictionaryDescription { get; set; }
        public string DictionaryRawText { get; set; }
        public bool UseDictionary { get; set; }
        public DictionaryData DictData { get; set; }

        public DictionaryMetaObject(string DictName, string DictDescript, string DictPrefix, string DictContents)
        {
            DictionaryName = DictName;
            DictionaryDescription = DictDescript;
            DictionaryRawText = DictContents;
            DictionaryCategoryPrefix = DictPrefix; 
            UseDictionary = true;
            DictData = new DictionaryData();
        }

    }


    public class DictionaryData
    {

        public int NumCats { get; set; }
        public int MaxWords { get; set; }

        public string[] CatNames { get; set; }
        public string[] CatValues { get; set; }

        //yeah, we're going full inception with this variable. dictionary inside of a dictionary inside of a dictionary
        //while it might seem unnecessarily complicated (and it might be), it makes sense.
        //the first level simply differentiates the wildcard entries from the non-wildcard entries                
        //The second level is purely to refer to the word length -- does each sub-entry include 1-word entries, 2-word entries, etc?
        //the third level contains the actual entries from the user's dictionary file
        public Dictionary<string, Dictionary<int, Dictionary<string, string[]>>> FullDictionary { get; set; }

        //this dictionary simply maps the specific wildcard entries to arrays, this way we can iterate across them since we have to do a serial search
        //when we're using wildcards
        public Dictionary<int, string[]> WildCardArrays { get; set; }

        //lastly, this contains the precompiled regexes mapped to their original strings
        public Dictionary<string, Regex> PrecompiledWildcards { get; set; }

        public bool DictionaryLoaded { get; set; }


        public DictionaryData()
        {
            NumCats = 0;
            MaxWords = 0;
            CatNames = new string[] { };
            CatValues = new string[] { };
            FullDictionary = new Dictionary<string, Dictionary<int, Dictionary<string, string[]>>>();
            WildCardArrays = new Dictionary<int, string[]>();
            PrecompiledWildcards = new Dictionary<string, Regex>();
            DictionaryLoaded = false;
        }






    }
}
