﻿namespace Assignment.Enitites.DTO_s
{
    public class MasterEntryModel
    {
        public string? TableName { get; set; }
        public string? ColumnNames { get; set; }
        public object? QueryParams { get; set; }
        public object? WhereParams { get; set; }
    }
}
