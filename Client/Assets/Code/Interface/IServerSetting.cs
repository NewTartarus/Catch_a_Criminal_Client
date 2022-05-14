namespace ScotlandYard.Interfaces
{
    public interface IServerSetting
    {
		int Id { get; }

		string ServerName { get; }

		string ServerUrl { get; }

		string HashedPassword { get; }

		string State { get; set; }

		string LastLogin { get; set; }
	}
}
