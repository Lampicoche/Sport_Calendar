using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

class FakeTempDataProvider : ITempDataProvider
{
    private readonly Dictionary<string, object?> _store = new();
    public IDictionary<string, object?> LoadTempData(HttpContext context) => _store;
    public void SaveTempData(HttpContext context, IDictionary<string, object?> values)
    {
        _store.Clear();
        foreach (var kv in values) _store[kv.Key] = kv.Value;
    }
}
