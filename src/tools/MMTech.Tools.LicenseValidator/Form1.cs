using Fly.Framework.Desktop.Toos;
using Mapster;

namespace MMTech.Tools.LicenseValidator;
#pragma warning disable WFO1000
public partial class Form1 : Form
{
    private PCInfo? info;
    private bool isActive = false;
    private readonly BasicSubscriber basic;

    public bool IsActive
    {
        get => isActive;
        set
        {
            button1.Enabled = !value;
            button2.Enabled = value;
            label1.BackColor = (value) ? Color.Green : Color.Red;
        }
    }

    public new string Name { get => textBox_Name.Text; set => textBox_Name.Text = value; }
    public string Caption { get => textBox_Caption.Text; set => textBox_Caption.Text = value; }
    public string InterfaceType { get => textBox_InterfaceType.Text; set => textBox_InterfaceType.Text = value; }
    public string Manufacturer { get => textBox_Manufacturer.Text; set => textBox_Manufacturer.Text = value; }
    public string Model { get => textBox_Model.Text; set => textBox_Model.Text = value; }
    public string PNPDeviceID { get => textBox_PNPDeviceID.Text; set => textBox_PNPDeviceID.Text = value; }
    public string MediaType { get => textBox_MediaType.Text; set => textBox_MediaType.Text = value; }
    public string SerialNumber { get => textBox_SerialNumber.Text; set => textBox_SerialNumber.Text = value; }
    public string SystemName { get => textBox_SystemName.Text; set => textBox_SystemName.Text = value; }
    public string DriveSize { get => textBox_Size.Text; set => textBox_Size.Text = value; }
    public string ProcessorId { get => textBox_ProcessorId.Text; set => textBox_ProcessorId.Text = value; }
    public string Description { get => textBox_Description.Text; set => textBox_Description.Text = value; }


    public Form1()
    {
        InitializeComponent();
        basic = new BasicSubscriber();

    }

    protected override void OnShown(EventArgs e)
    {
        try
        {
            info = basic.GetPCInfo();
            if (info is null)
            {
                MessageBox.Show("Cannot get PC information");
                return;
            }
            info.Adapt(this);
            IsActive = basic.SyncSub();
                
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        base.OnShown(e);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        //if (IsActive) return;
        basic.GenerateAqwasLicense();
        IsActive = true;
    }

    private void button2_Click(object sender, EventArgs e)
    {
        //if (!IsActive) return;
        basic.RemoveAqwasLicense();
        IsActive = false;
    }
}