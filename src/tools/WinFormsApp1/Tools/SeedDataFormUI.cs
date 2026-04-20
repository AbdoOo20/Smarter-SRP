using Dapper;
using Fly.Shared.Modules.Accounting.Dtos;
using Fly.Shared.Modules.PurchaseAndSales.Dtos.Misc;
using MySqlConnector;
using System.Data;
using System.Text;

namespace WinFormsApp1.Tools;

public partial class SeedDataFormUI : MaterialSkin.Controls.MaterialForm
{

    DataTable table = new DataTable();
    private bool canReadFile = false;
    private string _filePath = string.Empty;

    private int SelectedTab => materialTabControl1.SelectedIndex;
    private string selectedFile = string.Empty;

    private int GetSelectedCategory()
    {
        switch (selectedFile)
        {
            case "clients.csv":
                return 1;
            case "items.csv":
                return 2;
            case "suppliers.csv":
                return 2;
            default:
                return 0;
        }
    }

    public SeedDataFormUI()
    {
        InitializeComponent();
        materialTextBox23.Text = "Server=localhost;Database=mmtech__smartierp__20241102_test;Uid=app;Pwd=rI5AgE;";
        dataGridView1_clients.DataSource = table;
    }

    private string? GetFile()
    {
        canReadFile = false;
        selectedFile = string.Empty;
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "CSV files (*.csv)|*.csv";
        ofd.Multiselect = false;
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            var file = new FileInfo(ofd.FileName);
            _filePath = ofd.FileName;
            canReadFile = file.Exists;
            selectedFile = file.Name;
        }
        else return null;

        return _filePath;
    }

    private void CreateDataTable()
    {
        if (!canReadFile)
        {
            MessageBox.Show("No file selected");
            return;
        }
        table.Clear();
        table.Columns.Clear();

        var content = File.ReadAllLines(_filePath, Encoding.UTF8);
        bool ignoreFirst = false;
        for (int i = 0; i < content.Length; i++)
        {
            var line = content[i];
            var values = line.Split(';').Reverse().ToArray();

            if (!ignoreFirst)
            {
                for (int j = 0; j < values.Length; j++)
                {
                    table.Columns.Add(values[j]);
                }
                ignoreFirst = true;
                continue;
            }
            table.Rows.Add(values);
        }
    }

    private void ReadData()
    {
        try
        {
            if (!canReadFile)
            {
                MessageBox.Show("No file selected");
                return;
            }
            var content = File.ReadAllLines(_filePath);
            bool ignoreFirst = false;
            foreach (var line in content)
            {
                if (!ignoreFirst)
                {
                    ignoreFirst = true;
                    continue;
                }

                var values = line.Split(';');

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            //throw;
        }
    }

    private void materialButton1_Click(object sender, EventArgs e)
    {
        GetFile();
        CreateDataTable();
    }

    private void WriteData()
    {
        try
        {
            var connectionstring = materialTextBox21.Text;
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                var tr = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    if (GetSelectedCategory() == 0)
                    {
                        #region Clients

                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var e = table.Rows[i];

                            var account = new DtoAccount();
                            account.Id = Guid.NewGuid();
                            account.Name = e["Name"] as string ?? account.Id.ToString();
                            account.OpeningDebtor = e["OpeningDebtor"] as decimal? ?? 0;
                            account.OpeningCreditor = e["OpeningDebtor"] as decimal? ?? 0;
                            account.Id = connection.Insert<Guid, DtoAccount>(account);

                            var accQ = @"
                                    INSERT INTO `ACCONUT` ( `ID` , `NAME` , `OpeningDebtor` , `OpeningCreditor`, `ISDELETED`, `Guid`)
                                    VALUES ( @Id , @Name , @OpeningDebtor , @OpeningCreditor, @IsDeleted, @Guid)
                                ";

                            var client = new DtoClient();
                            client.Id = Guid.NewGuid();
                            client.AccountId = account.Id;
                            client.Name = e["Name"] as string ?? account.Name!;
                            client.Phone = e["Phone"] as string ?? "";
                            client.Email = e["Email"] as string ?? "";
                            client.Id = connection.Insert<Guid, DtoClient>(client);

                        }


                        #endregion
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    DisableButtons();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void DisableButtons(bool status = false)
    {
        materialButton1.Enabled = status;
        materialButton2.Enabled = status;
        materialButton3.Enabled = status;
        if (status)
        {

        }
        else
        {
        }
    }

    private void CheckDataBaseConnection()
    {
        DisableButtons();
        var connectionstring = materialTextBox23.Text;
        using (MySqlConnection connection = new MySqlConnection(connectionstring))
        {
            try
            {
                connection.Open();
                connection.Close();
                DisableButtons(true);
            }
            catch
            {
                DisableButtons();
            }
        }
    }


    private void materialButton4_Click(object sender, EventArgs e)
    {
        CheckDataBaseConnection();
    }
}