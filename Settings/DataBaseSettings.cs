namespace PPBackend.Settings;

public class DataBaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string BooksCollectionName { get; set; } = null!;
}

public class TestsStorageSettings : DataBaseSettings{}
public class UserStorageSettings : DataBaseSettings{}
public class GroupsStorageSettings : DataBaseSettings{}