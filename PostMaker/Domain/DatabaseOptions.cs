namespace Domain
{
    public class DatabaseOptions
    {
#if DEBUG
        public const string DatabaseConnectionString = @"Data Source=DESKTOP-HD1FKQE\SQLEXPRESS;Initial Catalog=PostMaker;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=False;Trust Server Certificate=False;Command Timeout=0";
#endif
#if RELEASE
        public const string DatabaseConnectionString = @"Server=tcp:pad-si.database.windows.net,1433;Initial Catalog=PostMaker;Persist Security Info=False;User ID=pad;Password=Parola123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
#endif
    }
}