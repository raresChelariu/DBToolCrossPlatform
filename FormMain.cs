namespace DBToolCrossPlatform;

public partial class FormMain : Form
{
    public FormMain()
    {
        InitializeComponent();
    }

    private async void buttonImportTableFromCSV_Click(object sender, EventArgs e)
    {
        var ofd = new OpenFileDialog
        {
            Filter = @"CSV files (*.csv)|*.csv|All files (*.*)|*.*"
        };
        if (ofd.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        textBoxInput.Text = await InfrastructureUtils.GetFileAsString(ofd);
    }

    private void buttonFindDependencies_Click(object sender, EventArgs e)
    {
        var separator = new ValueSeparator(comboBoxValueSeparator);
        var dataTable = InfrastructureUtils.FromCsvToDataTable(textBoxInput.Text, separator.Value);
        if (dataTable == null)
        {
            MessageBox.Show(@"Input is not properly formatted!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var dependencyFinder = new DependencyFinder(dataTable);

        dependencyFinder.Run();

        textBoxFunctional.Text = dependencyFinder.FunctionalDependenciesAsString;
        textBoxMultivalued.Text = dependencyFinder.MultivaluedDependenciesAsString;
    }

    private void numericUpDownTextSize_ValueChanged(object sender, EventArgs e)
    {
        if (numericUpDowntTextSize.Value <= 0)
            return;
        UpdateTextSizeForTextbox(textBoxInput);
        UpdateTextSizeForTextbox(textBoxFunctional);
        UpdateTextSizeForTextbox(textBoxMultivalued);
    }

    private void UpdateTextSizeForTextbox(Control textBox)
    {
        textBox.Font = new Font(textBox.Font.FontFamily, (float)numericUpDowntTextSize.Value);
    }
}