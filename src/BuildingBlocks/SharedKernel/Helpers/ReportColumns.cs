namespace SharedKernel.Helpers;

public class ReportColumns
{
    public static List<(string ColumnName, string PropertyName)> GetColumnsUsers()
    {
        var columnProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("FIRST NAME", "FirstName"),
            ("LAST NAME", "LastName"),
            ("EMAIL", "Email"),
            ("STATE", "StateDescription")
        };

        return columnProperties;
    }

    public static List<(string ColumnName, string PropertyName)> GetColumnsOrders()
    {
        var columnProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("TABLE", "TableNumber"),
            ("WAITER", "WaiterName"),
            ("STATUS", "Status"),
            ("TOTAL", "Total"),
            ("STATE", "StateDescription")
        };

        return columnProperties;
    }

    public static List<(string ColumnName, string PropertyName)> GetColumnsDishes()
    {
        var columnProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("NAME", "Name"),
            ("DESCRIPTION", "Description"),
            ("PRICE", "Price"),
            ("CATEGORY", "Category"),
            ("STATE", "StateDescription")
        };

        return columnProperties;
    }
}
