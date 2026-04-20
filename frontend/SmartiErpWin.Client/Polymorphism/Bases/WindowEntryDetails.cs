namespace SmartiErpWin.Client.Polymorphism.Bases;

[Obsolete("Not currently used")]
public record WindowEntryDetails<T>(
    LocalisationService LocalisationService,
    EntryType EntryType,
    HttpClient HttpClient,
    string Endpoint,
    T Model);