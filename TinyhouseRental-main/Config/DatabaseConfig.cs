namespace MyProject.Config
{
    public static class DatabaseConfig
    {
        public static string ConnectionString 
        { 
            get 
            {
                return @"Data Source=localhost;Initial Catalog=TinyHouseManagementDataBase;Integrated Security=True;";
            }
        }
    }
} 