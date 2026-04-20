using SmartErpWin.Core.Enums;

namespace SmartErpWin.Core.Helpers;

public static class TimeHelper
{
    public static unsafe void SetForStatement(in EntryType type, out DateTime start, out DateTime end)
    {
        start = default;
        end = default;

        if (type != EntryType.Add)
            return;

        fixed (DateTime* str = &start, ed = &end)
        {
            SetStart(str);
            SetEnd(ed);
        }
    }

    private static unsafe void SetStart(DateTime* start)
    {
        *start = DateTime.Today;
    }

    private static unsafe void SetEnd(DateTime* end)
    {
        *end = DateTime.Today.AddDays(1);
    }
}