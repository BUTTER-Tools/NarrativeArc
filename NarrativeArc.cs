using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using PluginContracts;
using OutputHelperLib;



namespace NarrativeArc
{
    public partial class NarrativeArc : Plugin
    {


        public string[] InputType { get; } = { "Tokens" };
        public string OutputType { get; } = "OutputArray";

        public Dictionary<int, string> OutputHeaderData { get; set; } = new Dictionary<int, string>() { { 0, "TokenizedText" } };
        public bool InheritHeader { get; } = false;

        #region Plugin Details and Info

        public string PluginName { get; } = "Narrative Arc";
        public string PluginType { get; } = "Language Analysis";
        public string PluginVersion { get; } = "1.1.0";
        public string PluginAuthor { get; } = "Ryan L. Boyd (ryan@ryanboyd.io)";
        public string PluginDescription { get; } = "Calculates the Narrative Arc scores for each text. This plugin provides that actual \"trajectory\" data (i.e., staging, plot progression, and cognitive tension scores across text segments). This plugin also provides the \"narrativity\" scores for each text: how \"narrative-like\" each text is along each narrative dimension, plus the overall average." + Environment.NewLine + Environment.NewLine + 
                                                   "For more information on the Narrative Arc and underlying methods, please see the following paper and website:" + Environment.NewLine + Environment.NewLine +
                                                   "Boyd, R. L., Blackburn, K. G., & Pennebaker, J. W. (2020). The narrative arc: Revealing core narrative structures through text analysis. Science Advances, 6(32), 1-9. https://doi.org/10.1126/sciadv.aba2196" + Environment.NewLine + Environment.NewLine +
                                                   "https://www.arcofnarrative.com";
        public bool TopLevel { get; } = false;
        public string PluginTutorial { get; } = "Coming Soon";

        DictionaryMetaObject AONDict { get; set; }
        private int Segments { get; set; } = 5;
        private string scalingMethod { get; set; } = "Z-Score";
        private bool includeDataPoints { get; set; } = true;
        private bool AllowDimensionDependence { get; set; } = false;


        public Icon GetPluginIcon
        {
            get
            {
                return Properties.Resources.icon;
            }
        }

        #endregion



        private static double StagingC = 0.595390886355984;
        private static double StagingX2 = 0.0419421326672925;
        private static double StagingX = -0.352251448565414;

        private static double ProgC = -0.936729880989401;
        private static double ProgX2 = -0.0663252975838221;
        private static double ProgX = 0.555436051470576;

        private static double CogTensC = -0.529662782519794;
        private static double CogTensX2 = -0.0629503763664916;
        private static double CogTensX = 0.407393185836139;
        

        //the normative arrays from our research
        private Dictionary<string, double[]> NormativeArcs { get; set; }
        private Dictionary<string, double[]> NormativeDeltas { get; set; }

        


        public void ChangeSettings()
        {

            using (var form = new SettingsForm_NarrativeArc(Segments, scalingMethod, includeDataPoints, AllowDimensionDependence))
            {


                form.Icon = Properties.Resources.icon;
                form.Text = PluginName;


                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    scalingMethod = form.scalingMethod;
                    Segments = form.Segments;
                    includeDataPoints = form.includeDataPoints;
                    AllowDimensionDependence = form.allowOverlaps;
                }
            }

        }





        public Payload RunPlugin(Payload Input)
        {



            Payload pData = new Payload();
            pData.FileID = Input.FileID;
            pData.SegmentID = Input.SegmentID;

            TextSegmenter splitter = new TextSegmenter();
            
            for (int i = 0; i < Input.StringArrayList.Count; i++)
            {

                //we have 4 plus the segmentation because we have to prepend additional output
                //0: token count
                //1: overall_narrativity
                //2: narrativity for staging
                //3: narrativity for plotprog
                //4: narrativity for cogtens
                string[] Output;
                if (includeDataPoints)
                {
                    Output = new string[5 + (Segments * 3)];
                }
                else
                {
                    Output = new string[5];
                }


                string[][] segmentTexts = splitter.SegmentIncomingText(Segments, Input.StringArrayList[i]);
                Dictionary<string, double[]> results = new Dictionary<string, double[]> { {"Staging", new double[Segments] }, { "PlotProg", new double[Segments] }, { "CogTension", new double[Segments] } };
                int TokenCount = 0;


                for (int segCount = 0; segCount < Segments; segCount++)
                {
                    TokenCount += segmentTexts[segCount].Length;
                    Dictionary<string, int> segResults = AnalyzeText(AONDict.DictData, segmentTexts[segCount]);
                    results["Staging"][segCount] = ((double)segResults["1"] / segmentTexts[segCount].Length) * 100;
                    results["PlotProg"][segCount] = ((double)segResults["2"] / segmentTexts[segCount].Length) * 100;
                    results["CogTension"][segCount] = ((double)segResults["3"] / segmentTexts[segCount].Length) * 100;
                }


                //z-score or Linear FS the results
                results = scaleResults(results);



                Output[0] = TokenCount.ToString();

                //calculate and store the narrativity scores
                Dictionary<string, double> narrativityScores = new Dictionary<string, double>();
                narrativityScores.Add("Staging", scoreNarrativity(results["Staging"], NormativeDeltas["Staging"]));
                narrativityScores.Add("PlotProg", scoreNarrativity(results["PlotProg"], NormativeDeltas["PlotProg"]));
                narrativityScores.Add("CogTension", scoreNarrativity(results["CogTension"], NormativeDeltas["CogTension"]));
                narrativityScores.Add("Overall", 
                                                (narrativityScores["Staging"] + 
                                                narrativityScores["PlotProg"] +
                                                narrativityScores["CogTension"]) / 3);

                //calculate the narrativity scores
                if (!Double.IsNaN(narrativityScores["Overall"])) Output[1] = narrativityScores["Overall"].ToString();
                if (!Double.IsNaN(narrativityScores["Staging"])) Output[2] = narrativityScores["Staging"].ToString();
                if (!Double.IsNaN(narrativityScores["PlotProg"])) Output[3] = narrativityScores["PlotProg"].ToString();
                if (!Double.IsNaN(narrativityScores["CogTension"])) Output[4] = narrativityScores["CogTension"].ToString();


                //start at position 5 because we have output in the first 5 elements already
                int countPosition = 5;

                //if we choose to include the actual datapoints in our output
                if (includeDataPoints)
                {
                    for (int categoryCount = 0; categoryCount < HeaderColumnSetup.Count; categoryCount++)
                    {
                        for (int segCount = 0; segCount < Segments; segCount++)
                        {

                            if (!Double.IsNaN(results[HeaderColumnSetup[categoryCount]][segCount]))
                            { Output[countPosition] = results[HeaderColumnSetup[categoryCount]][segCount].ToString(); }

                            countPosition++;

                        }

                    }
                }
                
                pData.SegmentNumber.Add(Input.SegmentNumber[i]);
                pData.StringArrayList.Add(Output);


            }

            return (pData);

        }

        


        Dictionary<int, string> HeaderColumnSetup { get; } = new Dictionary<int, string>() { { 0, "Staging" }, { 1, "PlotProg" }, { 2, "CogTension" } };

        public void Initialize()
        {
            if (AllowDimensionDependence)
            {
                AONDict = new DictionaryMetaObject("AON Dict", "Description", "", Properties.Resources.AONwithDependencies);
            }
            else
            {
                AONDict = new DictionaryMetaObject("AON Dict", "Description", "", Properties.Resources.AONwithoutDependencies);
            }

            DictParser DP = new DictParser();
            AONDict.DictData = DP.ParseDict(AONDict);


            #region setup header
            OutputHeaderData = new Dictionary<int, string>();

            OutputHeaderData.Add(0, "Tokens");
            OutputHeaderData.Add(1, "Narrativity_Overall");
            OutputHeaderData.Add(2, "Narrativity_Staging");
            OutputHeaderData.Add(3, "Narrativity_PlotProg");
            OutputHeaderData.Add(4, "Narrativity_CogTension");

            if (includeDataPoints) { 
                int countPosition = OutputHeaderData.Count;
                for (int headercount = 0; headercount < HeaderColumnSetup.Count; headercount++) { 

                    for(int i = 0; i < Segments; i++)
                    {
                        OutputHeaderData.Add(countPosition,
                                              HeaderColumnSetup[headercount] + "_" + (i + 1).ToString());
                        countPosition++;
                    }
                }
            }
            #endregion



            #region Set up our normative arrays
            //here, we need to go in and actually set up our normative arrays that we'll use to calculate narrativity scores
            NormativeArcs = new Dictionary<string, double[]>();
            NormativeArcs.Add("Staging", new double[Segments]);
            NormativeArcs.Add("PlotProg", new double[Segments]);
            NormativeArcs.Add("CogTension", new double[Segments]);

            //old version that uses only 4 diff scores to calculate AON shape
            //NormativeDeltas = new Dictionary<string, double[]>();
            //NormativeDeltas.Add("Staging", new double[Segments - 1]);
            //NormativeDeltas.Add("PlotProg", new double[Segments - 1]);
            //NormativeDeltas.Add("CogTension", new double[Segments - 1]);

            NormativeDeltas = new Dictionary<string, double[]>();
            NormativeDeltas.Add("Staging", new double[Segments]);
            NormativeDeltas.Add("PlotProg", new double[Segments]);
            NormativeDeltas.Add("CogTension", new double[Segments]);

            //we want the incoming points to be on a 1 to 5 scale, since that's what the original used from the paper
            double[] segmentPointsScaled = new double[Segments];
            for (int i = 1; i <= Segments; i++) segmentPointsScaled[i-1] = Convert.ToDouble(i);
            segmentPointsScaled = RescaleArray(segmentPointsScaled, 1, 5);


            for (int i = 0; i < Segments; i++)
            {
                NormativeArcs["Staging"][i] = (StagingX2 * Math.Pow(segmentPointsScaled[i] + 1, 2)) + (StagingX * (segmentPointsScaled[i] + 1)) + StagingC;
                NormativeArcs["PlotProg"][i] = (ProgX2 * Math.Pow(segmentPointsScaled[i] + 1, 2)) + (ProgX * (segmentPointsScaled[i] + 1)) + ProgC;
                NormativeArcs["CogTension"][i] = (CogTensX2 * Math.Pow(segmentPointsScaled[i] + 1, 2)) + (CogTensX * (segmentPointsScaled[i] + 1)) + CogTensC;
            }



            //scale these out exactly like we would do for the results themselves
            NormativeArcs = scaleResults(NormativeArcs);


            //set the first "delta" point to be the same as the first point of the related dimension. basically, this is a "Delta from zero"
            NormativeDeltas["Staging"][0] = NormativeArcs["Staging"][0];
            NormativeDeltas["PlotProg"][0] = NormativeArcs["PlotProg"][0];
            NormativeDeltas["CogTension"][0] = NormativeArcs["CogTension"][0];


            for (int i = 1; i < Segments; i++)
            {
                //old version that doesn't include the first arc point as a "delta from zero"
                //NormativeDeltas["Staging"][i-1] = NormativeArcs["Staging"][i] - NormativeArcs["Staging"][i-1];
                //NormativeDeltas["PlotProg"][i-1] = NormativeArcs["PlotProg"][i] - NormativeArcs["PlotProg"][i-1];
                //NormativeDeltas["CogTension"][i-1] = NormativeArcs["CogTension"][i] - NormativeArcs["CogTension"][i-1];

                NormativeDeltas["Staging"][i] = NormativeArcs["Staging"][i] - NormativeArcs["Staging"][i - 1];
                NormativeDeltas["PlotProg"][i] = NormativeArcs["PlotProg"][i] - NormativeArcs["PlotProg"][i - 1];
                NormativeDeltas["CogTension"][i] = NormativeArcs["CogTension"][i] - NormativeArcs["CogTension"][i - 1];
            }

            #endregion



        }




        public bool InspectSettings()
        {
            return true;
        }



        public Payload FinishUp(Payload Input)
        {
            return (Input);
        }




        //https://stackoverflow.com/a/2675216
        public double[] RescaleArray(double[] arr, double min, double max)
        {
            double m = (max - min) / (arr.Max() - arr.Min());
            double c = min - arr.Min() * m;
            var newarr = new double[arr.Length];
            for (int i = 0; i < newarr.Length; i++)
                newarr[i] = m * arr[i] + c;
            return newarr;
        }


        private double CalculateStdDev(double[] values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //Compute the Average      
                double avg = values.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }


        private double[] zScore(double[] doubleArr)
        {

            double arrMean = doubleArr.Average();
            double stdDev = CalculateStdDev(doubleArr);

            for (int i = 0; i < doubleArr.Length; i++) doubleArr[i] = (doubleArr[i] - arrMean) / stdDev;

            return (doubleArr);

        }

        private Dictionary<string, double[]> scaleResults(Dictionary<string, double[]> results)
        {

            if (scalingMethod == "Linear FS")
            {
                results["Staging"] = RescaleArray(results["Staging"], 0, 100);
                results["PlotProg"] = RescaleArray(results["PlotProg"], 0, 100);
                results["CogTension"] = RescaleArray(results["CogTension"], 0, 100);
            }
            else if (scalingMethod == "Z-Score")
            {
                results["Staging"] = zScore(results["Staging"]);
                results["PlotProg"] = zScore(results["PlotProg"]);
                results["CogTension"] = zScore(results["CogTension"]);
            }
            else if (scalingMethod == "None")
            {
                return results;
            }

            return results;
        }



        private double cosineSim(double[] arr1, double[] arr2)
        {


            //https://janav.wordpress.com/2013/10/27/tf-idf-and-cosine-similarity/
            //Cosine Similarity (d1, d2) =  Dot product(d1, d2) / ||d1|| * ||d2||
            //
            //Dot product (d1,d2) = d1[0] * d2[0] + d1[1] * d2[1] * … * d1[n] * d2[n]
            //||d1|| = square root(d1[0]2 + d1[1]2 + ... + d1[n]2)
            //||d2|| = square root(d2[0]2 + d2[1]2 + ... + d2[n]2)
            double dotproduct = 0;
            double d1 = 0;
            double d2 = 0;

            //calculate cosine similarity components
            for (int j = 0; j < arr1.Length; j++)
            {
                dotproduct += arr1[j] * arr2[j];
                d1 += Math.Pow(arr1[j], 2);
                d2 += Math.Pow(arr2[j], 2);
            }

            return(dotproduct / (Math.Sqrt(d1) * Math.Sqrt(d2)));

        }


        private double scoreNarrativity(double[] resultVector, double[] normativeDeltas)
        {

            //old version without "delta from zero
            //double[] resultDeltas = new double[resultVector.Length - 1];

            double[] resultDeltas = new double[resultVector.Length];
            resultDeltas[0] = resultVector[0];

            for (int i = 1; i < resultVector.Length; i++) resultDeltas[i] = resultVector[i] - resultVector[i - 1];

            //old version
            //for (int i = 1; i < resultVector.Length; i++) resultDeltas[i] = resultVector[i] - resultVector[i - 1];

            double narrativity = cosineSim(normativeDeltas, resultDeltas);
            return (narrativity * 100);
        }





        #region Import/Export Settings
        public void ImportSettings(Dictionary<string, string> SettingsDict)
        {
            scalingMethod = SettingsDict["scalingMethod"];
            Segments = int.Parse(SettingsDict["Segments"]);
            includeDataPoints = Boolean.Parse(SettingsDict["includeDataPoints"]);
            AllowDimensionDependence = Boolean.Parse(SettingsDict["allowDependence"]);
        }

        public Dictionary<string, string> ExportSettings(bool suppressWarnings)
        {
            Dictionary<string, string> SettingsDict = new Dictionary<string, string>();
            SettingsDict.Add("scalingMethod", scalingMethod);
            SettingsDict.Add("Segments", Segments.ToString());
            SettingsDict.Add("includeDataPoints", includeDataPoints.ToString());
            SettingsDict.Add("allowDependence", AllowDimensionDependence.ToString());


            return (SettingsDict);
        }
        #endregion



    }
}
