using System;
using Util;

public class SheetToJsonRequest
{
    private const string BaseUrl = "docs.google.com/spreadsheets/d";
    private const string DefaultSheetId = "0";

    public EFileNames File;
    public string Path;
    public string Range;
    public string TestSheetId;
    public string TestSheetRange;

    public Uri ToUri(bool isTest = false)
    {
        UriBuilder uriBuilder = new UriBuilder("https", BaseUrl);
        bool canTestRange = isTest && TestSheetRange != DefaultSheetId && !string.IsNullOrEmpty(TestSheetRange);
        string sheetRange = canTestRange ? TestSheetRange : Range;
        uriBuilder.Path = $"{Path}/export";
        string queryParam = isTest && TestSheetId != DefaultSheetId && !string.IsNullOrEmpty(TestSheetId)
            ? $"range={sheetRange}&format=tsv&id={Path}&gid={TestSheetId}"
            : $"range={sheetRange}&format=tsv";
        uriBuilder.Query = queryParam;
        return uriBuilder.Uri;
    }
}