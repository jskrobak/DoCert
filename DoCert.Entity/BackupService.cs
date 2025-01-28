using System.Data.SQLite;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Data.Sqlite;

namespace DoCert.Entity;

public class BackupService
{
    private const string dbName = "docert.db";
    private const string restoreDbName = "restore.db";
    
    private readonly string _appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DoCert");


    public void Backup(string backupPath)
    {
        var backupDirPath = Path.Combine(_appDataDir, "backup");
        Directory.CreateDirectory(backupDirPath);

        var dbPath = Path.Combine(_appDataDir, dbName);
        var backupDbPath = Path.Combine(backupDirPath, dbName);

        // Perform the SQLite backup
        using var sourceConnection = new SQLiteConnection($"Data Source={dbPath};Mode=ReadOnly;");
        using var backupConnection = new SQLiteConnection($"Data Source={backupDbPath};");

        sourceConnection.Open();
        backupConnection.Open();
        sourceConnection.BackupDatabase(backupConnection, "main", "main", -1, null, 0);
        sourceConnection.Close();
        backupConnection.Close();

        using var fs = new FileStream(backupPath, FileMode.Create);
        using var arch = new ZipArchive(fs, ZipArchiveMode.Create);
        arch.CreateEntryFromFile(backupDbPath, dbName);
        
        File.Delete(backupDbPath);
    }

    public bool PrepareRestore(string backupFile)
    {
        using var arch= ZipFile.OpenRead(backupFile);
        if (arch.Entries.Count == 0)
            return false;
        
        if(arch.Entries[0].FullName != dbName)
            return false;
        
        arch.Entries[0].ExtractToFile(Path.Combine(_appDataDir, restoreDbName), true);

        return true;
    }

    public void Restore()
    {
        if(File.Exists(Path.Combine(_appDataDir, restoreDbName)))
            File.Move(Path.Combine(_appDataDir, restoreDbName), Path.Combine(_appDataDir, dbName), true);        
    }
}