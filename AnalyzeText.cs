using PluginContracts;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NarrativeArc
{


    public partial class NarrativeArc : Plugin
    {



        private Dictionary<string, int> AnalyzeText(DictionaryData DictData, string[] Words)
        {

            int NumberOfMatches = 0;
            int TotalStringLength = Words.Length;

            Dictionary<string, int> DictionaryResults = new Dictionary<string, int>();
            for (int i = 0; i < DictData.NumCats; i++) DictionaryResults.Add(DictData.CatValues[i], 0);

            for (int i = 0; i < TotalStringLength; i++)
            {



                //iterate over n-grams, starting with the largest possible n-gram (derived from the user's dictionary file)
                for (int NumberOfWords = DictData.MaxWords; NumberOfWords > 0; NumberOfWords--)
                {



                    //make sure that we don't overextend past the array
                    if (i + NumberOfWords - 1 >= TotalStringLength) continue;

                    //make the target string

                    string TargetString;

                    if (NumberOfWords > 1)
                    {
                        TargetString = String.Join(" ", Words.Skip(i).Take(NumberOfWords).ToArray());
                    }
                    else
                    {
                        TargetString = Words[i];
                    }


                    //look for an exact match

                    if (DictData.FullDictionary["Standards"].ContainsKey(NumberOfWords))
                    {
                        if (DictData.FullDictionary["Standards"][NumberOfWords].ContainsKey(TargetString))
                        {

                            NumberOfMatches += NumberOfWords;
                            //add in the number of words found
                            for (int j = 0; j < DictData.FullDictionary["Standards"][NumberOfWords][TargetString].Length; j++)
                            {

                                if (DictionaryResults.ContainsKey(DictData.FullDictionary["Standards"][NumberOfWords][TargetString][j])) DictionaryResults[DictData.FullDictionary["Standards"][NumberOfWords][TargetString][j]] += NumberOfWords;

                            }
                            //manually increment the for loop so that we're not testing on words that have already been picked up
                            i += NumberOfWords - 1;
                            //break out of the lower level for loop back to moving on to new words altogether
                            break;
                        }
                    }
                    //if there isn't an exact match, we have to go through the wildcards
                    if (DictData.WildCardArrays.ContainsKey(NumberOfWords))
                    {
                        for (int j = 0; j < DictData.WildCardArrays[NumberOfWords].Length; j++)
                        {
                            if (DictData.PrecompiledWildcards[DictData.WildCardArrays[NumberOfWords][j]].Matches(TargetString).Count > 0)
                            {

                                NumberOfMatches += NumberOfWords;

                                for (int k = 0; k < DictData.FullDictionary["Wildcards"][NumberOfWords][DictData.WildCardArrays[NumberOfWords][j]].Length; k++)
                                {

                                    if (DictionaryResults.ContainsKey(DictData.FullDictionary["Wildcards"][NumberOfWords][DictData.WildCardArrays[NumberOfWords][j]][k])) DictionaryResults[DictData.FullDictionary["Wildcards"][NumberOfWords][DictData.WildCardArrays[NumberOfWords][j]][k]] += NumberOfWords;

                                }
                                //manually increment the for loop so that we're not testing on words that have already been picked up
                                i += NumberOfWords - 1;
                                //break out of the lower level for loop back to moving on to new words altogether
                                break;

                            }
                        }
                    }


                }



            }


            return DictionaryResults;


        }









    }






}
