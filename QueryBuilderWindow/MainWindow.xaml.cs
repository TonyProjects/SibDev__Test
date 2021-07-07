using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace QueryBuilderWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "Persist Security Info=False;Integrated Security=true;Initial Catalog=Geogrphy;server=HOUM-COMP\\SQLEXPRESS";

        YOlshinAnton.QueryBuilder.TableColumn[] Columns;
        YOlshinAnton.QueryBuilder.TableTableColumn[] TableTableColumns;

        int[] ListBoxColumnsDatabaseColumns;

        List<int> selectedColumns;

        void FillColumnsListBox()
        {
            Columns = YOlshinAnton.QueryBuilder.Database.Repository.SelectTableColumns();
            TableTableColumns = YOlshinAnton.QueryBuilder.Database.Repository.SelectTableTableColumns();

            ListBoxColumnsDatabaseColumns = new int[Columns.Length];

            int i = 0;
            foreach (var table in TableTableColumns)
            {

                for (int j = 0; j < table.Columns.Length; ++j, ++i)
                {
                    ColumnsListBox.Items.Add(table.Name + "." + table.Columns[j].Name);

                    ListBoxColumnsDatabaseColumns[i] = table.Columns[j].Id; 
                }
            }
        }

        void FillQueryResultDataGrid(string query)
        {
            QueryTextBlock.Text = query;

            DataTable datatable = new DataTable();
            DataSet dataset = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        var column = new DataGridTextColumn();
                        column.Header = reader.GetName(i);
                        QueryDataGrid.Columns.Add(column);
                    }
                    /*
                    while (reader.Read())
                    {
                    }
                    */
                }
            }

        }



        public MainWindow()
        {
            YOlshinAnton.QueryBuilder.Database.Repository.SelectTables = GetTables;
            YOlshinAnton.QueryBuilder.Database.Repository.SelectTableColumns = GetTableColumns;
            YOlshinAnton.QueryBuilder.Database.Repository.SelectDatabaseGraphEdges = GetDatabaseGraphEdges;


            InitializeComponent();

            FillColumnsListBox();

            TestQuieryBuilderLibrary();

        }



        static YOlshinAnton.QueryBuilder.Table[] GetTables()
        {
            return new YOlshinAnton.QueryBuilder.Table[]
            {
                new YOlshinAnton.QueryBuilder.Table() { Id=0, Name="Country", Order=0, Shortcut="C" },
                new YOlshinAnton.QueryBuilder.Table() { Id=1, Name="Government", Order=1, Shortcut="G" },
                new YOlshinAnton.QueryBuilder.Table() { Id=2, Name="City", Order=2, Shortcut="Ct"}
            };
        }

        static YOlshinAnton.QueryBuilder.TableColumn[] GetTableColumns()
        {
            return new YOlshinAnton.QueryBuilder.TableColumn[]
            {
                new YOlshinAnton.QueryBuilder.TableColumn() {Id=0, TableId=0, Name="id" },
                new YOlshinAnton.QueryBuilder.TableColumn() {Id=1, TableId=0, Name="Name" },
                new YOlshinAnton.QueryBuilder.TableColumn() {Id=2, TableId=0, Name="GovernmentId" },
                new YOlshinAnton.QueryBuilder.TableColumn() {Id=3, TableId=2, Name="Id" },
                new YOlshinAnton.QueryBuilder.TableColumn() {Id=4, TableId=2, Name="Name" },
                new YOlshinAnton.QueryBuilder.TableColumn() {Id=5, TableId=2, Name="CountryId"},
                new YOlshinAnton.QueryBuilder.TableColumn() {Id=6, TableId=1, Name="Id" },
                new YOlshinAnton.QueryBuilder.TableColumn() {Id=7, TableId=1, Name="Name" }
            };
        }

        static YOlshinAnton.QueryBuilder.DatabaseGraphEdge[] GetDatabaseGraphEdges()
        {
            return new YOlshinAnton.QueryBuilder.DatabaseGraphEdge[]
            {
                new YOlshinAnton.QueryBuilder.DatabaseGraphEdge()
                {
                    Id=0, Table1=0, Table2=2,
                    JoinCondition="left join City Ct on C.Id = Ct.CountryId"
                },
                new YOlshinAnton.QueryBuilder.DatabaseGraphEdge()
                {
                    Id=1, Table1=0, Table2=1,
                    JoinCondition="left join Government G on C.GovernmentId = G.Id"
                }
            };
        }

        void TestQuieryBuilderLibrary()
        {
            var columns = new List<int> { 0, 1 };

            string query = YOlshinAnton.QueryBuilder.QueryBuilder.BuildQuery(columns);

            FillQueryResultDataGrid(query);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            selectedColumns = new List<int>();

            if (ColumnsListBox.SelectedItems.Count == 0)
                return;

            foreach(var item in ColumnsListBox.SelectedItems)
            {
                int itemIndex = ColumnsListBox.Items.IndexOf(item);
                int columnIndex = ListBoxColumnsDatabaseColumns[itemIndex];

                selectedColumns.Add(columnIndex);
            }

            QueryTextBlock.Text = YOlshinAnton.QueryBuilder.QueryBuilder.BuildQuery(selectedColumns);
        }
    }
}
