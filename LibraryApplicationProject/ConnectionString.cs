namespace LibraryApplicationProject
{
    public static class ConnectionString
    {
        private const string DatabaseName = "ASP_EF_Test_03";
        public static string SqlString = $"Data Source=WALTER\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=True;Database={DatabaseName};Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
    }
}
