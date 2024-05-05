namespace Topup; //fix name spave

public interface DebitMessage
{
    public string Trn { get; set; }
    public int Status { get; set; }
    public string UserNO { get; set; }
    public decimal Amount { get; set; }
}

