using Fly.Shared.Modules.Accounting.Dtos;

namespace Fly.Shared.Modules.Accounting.SeededData;

public class CurrencySeed
{
    private static readonly DateTime defaultDate = new(2024, 7, 2);

    private static readonly IEnumerable<DtoCurrency>? Currencies = new[]
    {
        new DtoCurrency
        {
            Id = Guid.Parse("A7B489A2-0689-417D-BCE4-92EBFB11D987"), Name = "دينار ليبي", Letter = "LYD", CreateDate = defaultDate, CreatedByName = null, IsPrimary = true, TransfereRate = 1
        }
        , new DtoCurrency
        {
            Id = Guid.Parse("A7B489A2-0689-417D-BCE4-92EBFB11D987"), Name = "دولار امريكي", Letter = "USD", CreateDate = defaultDate, CreatedByName = null, IsPrimary = false, TransfereRate = 0.21m
        }
    };

    public static DtoCurrency? LydCurrency => Currencies?.ElementAtOrDefault(0);
    public static DtoCurrency? USsdCurrency => Currencies?.ElementAtOrDefault(1);
}