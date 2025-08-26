using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine;
using UnityEngine.Networking;

public static class TsvLoader
{
    private static readonly CsvConfiguration TsvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        Delimiter = "\t",
        Mode = CsvMode.NoEscape,
        HasHeaderRecord = true,
        MissingFieldFound = null,
        HeaderValidated = null,
    };

    public static async Task<List<T>> LoadTableAsync<T>(string tableName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Table", tableName + ".tsv");

        string textData = null;

        // Android에서는 UnityWebRequest 사용
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            using UnityWebRequest www = UnityWebRequest.Get(filePath);
            var op = www.SendWebRequest();
            while (!op.isDone) await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[TsvLoader] 파일 로드 실패: {filePath}, {www.error}");
                return null;
            }

            textData = www.downloadHandler.text;
        }
        else
        {
            // PC/에디터에서는 File IO 사용 가능
            if (!File.Exists(filePath))
            {
                Debug.LogError($"[TsvLoader] 파일 없음: {filePath}");
                return null;
            }
            textData = File.ReadAllText(filePath);
        }

        // TSV 파싱
        using var reader = new StringReader(textData);
        using var csv = new CsvReader(reader, TsvConfig);

        var records = new List<T>();
        await foreach (var record in csv.GetRecordsAsync<T>())
        {
            records.Add(record);
        }

        return records;
    }
}