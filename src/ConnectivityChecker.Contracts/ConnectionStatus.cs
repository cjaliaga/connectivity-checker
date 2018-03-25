namespace ConnectivityChecker.Contracts
{
    public class ConnectionStatus : ConnectionInfo
    {
        public ConnectionStatus()
        {
            
        }

        public ConnectionStatus(ConnectionInfo info)
        {
            Name = info.Name;
            Provider = info.Provider;
            Value = info.Value;
        }

        public string Status { get; set; } = "It works!";
        public bool IsWorking { get; set; } = true;
    }
}