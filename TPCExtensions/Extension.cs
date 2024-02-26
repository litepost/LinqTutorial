namespace TPCExtensions;
public static class Extension
{
    public static List<T> Filter<T>(this List<T> records, Func<T, bool> func) {
        List<T> filteredList = new List<T>();

        foreach (T rec in records) {
            if (func(rec)) {
                filteredList.Add(rec);
            }
        }
        return filteredList;
    }
}
