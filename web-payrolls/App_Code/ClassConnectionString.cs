public static class ClassConnectionString
{
  private static readonly string ServerIP = "192.168.1.252";
  private static readonly string userid = "sa";
  private static readonly string pwd = "Vannsavong642647";
  public static string SystemType = ClassGlobalVar.ReadSystemType(); // Get Type From Cookie

  public static string GetConnectionString()
  {
    string myConnectionString = "";
    myConnectionString = "SERVER=" + ServerIP + "; DATABASE=PAYROLL_20211; UID=" + userid + "; PWD=" + pwd +
                         "; Connection Timeout=360000; pooling='true'; Max Pool Size=360000";

    //if (SystemType == "Stock")
    //{
    //    myConnectionString = "SERVER=" + ServerIP + "; DATABASE=StockDB2018; UID=" + userid + "; PWD=" + pwd + "; Connection Timeout=1;";
    //}
    //if (SystemType == "Payroll")
    //{
    //    myConnectionString = "SERVER=" + ServerIP + "; DATABASE=PRDB2018; UID=" + userid + "; PWD=" + pwd + "; Connection Timeout=1;";
    //}
    return myConnectionString;
  }
}
