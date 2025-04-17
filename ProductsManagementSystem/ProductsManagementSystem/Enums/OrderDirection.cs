using System.Text.Json.Serialization;

namespace ProductsManagementSystem.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))] // لجعل قيم ال أينم  تظهر كأسماء بدلاً من أرقام
    public enum OrderDirection
    {
        Ascending,  // تصاعدي
        Descending  // تنازلي
    }
}
