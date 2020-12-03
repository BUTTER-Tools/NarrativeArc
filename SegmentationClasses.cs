using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;


namespace NarrativeArc
{


    

    internal class TextSegmenter
    {

        //private static string[] stopList { get; } = new string[] { "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(",
        //                                                            ")", "_", "+", "-", "–", "=", "[", "]", "\\", ";",
        //                                                            "'", ",", ".", "/", "{", "}", "|", ":", "\"", "<",
        //                                                            ">", "?", "..", "...", "«", "««", "»»", "“", "”",
        //                                                            "‘", "‘‘", "’", "’’", "1", "2", "3", "4", "5", "6",
        //                                                            "7", "8", "9", "0", "10", "11", "12", "13", "14",
        //                                                            "15", "16", "17", "18", "19", "20", "25", "30", "33",
        //                                                            "40", "50", "60", "66", "70", "75", "80", "90", "99",
        //                                                            "100", "123", "1000", "10000", "12345", "100000", "1000000" };

        private static string[] stopList { get; } = new string[] { "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(",
                                                                    ")", "_", "+", "-", "–", "=", "[", "]", "\\", ";",
                                                                    "'", ",", ".", "/", "{", "}", "|", ":", "\"", "<",
                                                                    ">", "?", "..", "...", "«", "««", "»»", "“", "”",
                                                                    "‘", "‘‘", "’", "’’"};

        public string[][] SegmentIncomingText(int SegmentationParameter, string[] tokens)
        {

            string[][] Segments = new string[SegmentationParameter][];

            List<string> tokensCleaned = tokens.Where(x => !stopList.Contains(x)).ToList();
            List<List<string>>SegmentedTokens = Partition<string>(tokensCleaned, SegmentationParameter);
            for (int i = 0; i < SegmentationParameter; i++) Segments[i] = SegmentedTokens[i].ToArray();
            
            return (Segments);

        }




        //modified version of
        //https://stackoverflow.com/a/3893011

        /// <summary>
        /// Partition a list of elements into a smaller group of elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="totalPartitions"></param>
        /// <returns></returns>
        public static List<List<string>> Partition<T>(List<string> list, int totalPartitions)
        {

            List<string>[] partitions = new List<string>[totalPartitions];

            //if (list == null)
            //    throw new ArgumentNullException("list");

            //if (totalPartitions < 1)
            //    throw new ArgumentOutOfRangeException("totalPartitions");

            if (list == null || totalPartitions < 1)
            {
                partitions[0] = new List<string>() { "" };
                return partitions.ToList();
            }
                


            int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            int k = 0;

            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<string>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                }
                k += maxSize;
            }

            return partitions.ToList();
        }




















        //old and busted. some of this is broken code from MEH that simply fails in some cases.
        //tried to modify, got distracted, opted for a *far* cleaner approach instead (above).

        //private List<string> Segment_Chopper(string[] array_to_split, int number_of_splits)
        //{

        //    if (array_to_split.Length < number_of_splits) number_of_splits = array_to_split.Length;

        //    string[][] Segmented_Array = new string[number_of_splits][];
        //    int Segment_Size = (int)System.Math.Ceiling((double)array_to_split.Length / number_of_splits);


        //    for (int i = 0; i < number_of_splits; i++)
        //    {

        //        int SegmentStart = i * Segment_Size;
        //        int SegmentEnd = ((i + 1) * Segment_Size);
        //        int SegmentLength = SegmentEnd - SegmentStart;

                

        //        //if ((i != number_of_splits - 1))
        //        //{

        //            Segmented_Array[i] = new string[SegmentLength];

        //            if (SegmentEnd + 1 <= array_to_split.Length)
        //            {
        //                Array.Copy(array_to_split, SegmentStart, Segmented_Array[i], 0, SegmentLength);
        //            }
        //            else
        //            {
        //                SegmentLength = Segment_Size;
        //                string[] tailstring = new string[SegmentLength];
        //                for (int j = 0; j < SegmentLength; j++) tailstring[j] = "";
        //                for (int j = SegmentStart; j < array_to_split.Length; j++) tailstring[j - SegmentStart] = array_to_split[j];
        //                Array.Copy(tailstring, SegmentStart, Segmented_Array[i], 0, SegmentLength);
        //            }



        //        //}
        //        //else
        //        //{
        //        //    SegmentLength = array_to_split.Length - SegmentStart;

        //        //    Segmented_Array[i] = new string[SegmentLength];

        //        //    if (SegmentEnd + 1 <= array_to_split.Length)
        //        //    {
        //        //        Array.Copy(array_to_split, SegmentStart, Segmented_Array[i], 0, SegmentLength);
        //        //    }
        //        //    else
        //        //    {
        //        //        string[] tailstring = new string[SegmentLength];
        //        //        for (int j = 0; j < SegmentLength; j++) tailstring[j] = "";
        //        //        for (int j = SegmentStart; j < array_to_split.Length; j++) tailstring[j - SegmentStart] = "";
        //        //        Array.Copy(tailstring, 0, Segmented_Array[i], 0, SegmentLength);
        //        //    }
        //        //}

        //    }



        //    List<string> TextSegments = new List<string>();

        //    for (int i = 0; i < number_of_splits; i++) TextSegments.Add(string.Join(" ", Segmented_Array[i]));

        //    return TextSegments;
        //}








    }


       
}
