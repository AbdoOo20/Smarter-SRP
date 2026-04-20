namespace Fly.Shared.Modules.Store.Enums;

public enum RelatedInventory : int
{
    None = 0,
    Purchase = 10,
    ReturnPurchase = 11,
    Sale = 20,
    FastSale = 21,
    Transfer = 30,
    Marketer = 40,
    Disbursement = 50,
    Disbursement_Return = 51,
    Supply = 60,
    Supply_Return = 61,
    Damaged = 62,
    //MarketerSupply = 71,
    MarketerSaleSell= 73,
    MarketerDisbursement = 70,
    MarketerSalePreparation_Sale = 75,
    MarketerSalePreparation_Marketer = 76,
    MarketerSalePreparationUndo = 72,
    Maintenance = 74,
    Factoring = 75,
    ReturnSale = 80,

    QuantityCrediting_Zero = 90,
    QuantityCrediting = 91,

    MarketerMobile_SellInvoice = 100,
    MarketerMobile_DamageInvoice = 101,

}
