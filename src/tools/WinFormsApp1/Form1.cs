using MySqlConnector;
using System.Data;

namespace WinFormsApp1;

public partial class Form1 : MaterialSkin.Controls.MaterialForm
{
    public Form1()
    {
        InitializeComponent();

        CheckboxForm("Server=localhost;Database=mmtech__SmartiERP__010_test;Uid=app;Pwd=rI5AgE;");
    }

    private void AddPermissions()
    {
        using var mySqlConnection = new MySqlConnection("Server=localhost;Database=mmtech__SmartiERP__010_test;Uid=app;Pwd=rI5AgE;");
        mySqlConnection.Open();
        var dataset = new DataSet();
        var command = mySqlConnection.CreateCommand();
        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        adapter.Fill(dataset);


        mySqlConnection.Close();
    }

    private ListView listView;
    private string connectionString;
    private List<(Guid id, string key, string name)> values;

    public void CheckboxForm(string connectionString)
    {
        this.connectionString = connectionString;
        this.Controls.Add(listView);
        this.listView = materialListView1;
        this.listView.CheckBoxes = true;
        this.listView.Columns.Add("Item Name");

        // Get data from database
        values = GetItemsFromDatabase();

        // Populate ListView with items
        foreach (var item in values)
        {
            listView.Items.Add(key: item.id.ToString(), text: item.name, 0);
        }
    }

    private List<(Guid id, string key, string name)> GetItemsFromDatabase()
    {
        List<(Guid id, string key, string name)> items = new();

        using (MySqlConnection connection = new(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM trole"; // Replace with your actual query
            MySqlCommand command = new(query, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(new(reader.GetGuid(0), reader.GetString(2), reader.GetString(1)));

                }
            }
        }

        return items;
    }

    public ListView GetListView()
    {
        return listView;
    }

    // Add a method to handle checkbox selection and database updates (implementation omitted for brevity)
    public void HandleCheckboxSelection()
    {
        // ...
    }

    private void materialFloatingActionButton1_Click(object sender, EventArgs e)
    {
        /*



         */
        var selected = listView.SelectedIndices;
        if (selected.Count > 0)
        {


            using (MySqlConnection connection = new(connectionString))
            {
                connection.Open();
                foreach (dynamic item in listView.Items)
                {
                    if(item.Selected == false)
                    {
                        continue;
                    }
                    var l00 = item.Index as int? ?? -1 ;
                    string query = $" INSERT INTO `trolegroupdetail` (`Id`, `RoleGroupId`, `RoleId`, `Guid`, `CreateDate`, `IsDeleted`, `TenantId`) VALUES ('{Guid.NewGuid()}', '8E0BD0AA-A828-456D-9B02-D1C079A66FEF', '{values[l00].id}', '1F1028D4-7D01-4572-A2BD-9B33952CCEDC', '2024-10-07 01:01:47.079601', 0, 'A2E6B208-394D-4475-BBD9-F679656DCC49')";
                    MySqlCommand command = new(query, connection);
                    var a = command.ExecuteNonQuery();
                        
                }
            }
        }
    }

}