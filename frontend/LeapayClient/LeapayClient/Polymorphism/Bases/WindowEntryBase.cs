using SmartErpWin.Core.Enums;

namespace Leapay.Client.Polymorphism.Bases;

public record WindowEntryModel<T>(T? Model, EntryType EntryType, Type PageType);