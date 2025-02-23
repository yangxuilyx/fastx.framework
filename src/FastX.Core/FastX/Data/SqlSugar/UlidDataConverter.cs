using System.Data;
using SqlSugar;
using SqlSugar.DbConvert;
using DbType = System.Data.DbType;

namespace FastX.Data.SqlSugar;

public class UlidDataConverter : ISugarDataConverter
{
    public SugarParameter ParameterConverter<T>(object columnValue, int columnIndex)
    {
        var name = "@ulid" + columnIndex;
        if (columnValue is Ulid ulid)
            return new SugarParameter(name, ulid.ToString(), DbType.String);

        if (Ulid.TryParse(columnValue?.ToString(), out ulid))
            return new SugarParameter(name, ulid.ToString(), DbType.String);

        return new SugarParameter(name, Ulid.Empty.ToString(), DbType.String);
    }

    public T QueryConverter<T>(IDataRecord dataRecord, int dataRecordIndex)
    {
        var str = dataRecord.GetValue(dataRecordIndex).ToString() ?? string.Empty;
        if (Ulid.TryParse(str, out var ulid))
            return (T)ConvertToObject(ulid);

        return (T)ConvertToObject(Ulid.Empty);
    }

    private object ConvertToObject(object obj)
    {
        return obj;
    }
}