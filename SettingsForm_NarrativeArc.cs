using System.Windows.Forms;
using System;

namespace NarrativeArc
{
    internal partial class SettingsForm_NarrativeArc : Form
    {


        #region Get and Set Options

        public int Segments { get; set; }
        public string scalingMethod { get; set; }
        public string narrativityScoringMethod { get; set; }
        public bool includeDataPoints { get; set; }
        public bool allowOverlaps { get; set; }

        #endregion



        public SettingsForm_NarrativeArc(int segs, string scaleMeth, string scoreMeth, bool includeDataPointsincoming, bool AllowDimensionDependence)
        {
            InitializeComponent();


            SegmentsUpDown.Value = segs;

            ScalingMethodDropDown.Items.Add("Z-Score");
            ScalingMethodDropDown.Items.Add("Linear FS");
            //ScalingMethodDropDown.Items.Add("None");

            scoringMethodDropdown.Items.Add("Diff. Score Similarities");
            scoringMethodDropdown.Items.Add("Fréchet Distance");

            ScalingMethodDropDown.SelectedIndex = ScalingMethodDropDown.FindStringExact(scaleMeth);
            scoringMethodDropdown.SelectedIndex = scoringMethodDropdown.FindStringExact(scoreMeth);

            includeDataPointsCheckbox.Checked = includeDataPointsincoming;

            AllowOverlapsCheckbox.Checked = AllowDimensionDependence;


        }






        private void OKButton_Click(object sender, System.EventArgs e)
        {

            this.scalingMethod = ScalingMethodDropDown.Text;
            this.narrativityScoringMethod = scoringMethodDropdown.Text;
            this.Segments = Convert.ToInt32(SegmentsUpDown.Value);
            this.includeDataPoints = includeDataPointsCheckbox.Checked;
            this.allowOverlaps = AllowOverlapsCheckbox.Checked;
            this.DialogResult = DialogResult.OK;
            

        }
    }
}
