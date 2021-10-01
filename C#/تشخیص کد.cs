/// <summary>
///دوفیلد در دیتابیس داریم
/// Code = string
/// Codeint = int
/// آخرین کد وارده را انتخاب میکند
/// </summary>
void AnalysisCodeAndCodeint()
{
try
{
    var idTbl = NameDatabase.NameTbl.OrderByDescending(a => a.ID).FirstOrDefault();
    if (idTbl == null)
    {
        return "1";
    }
    else
    {
        if (string.IsNullOrEmpty(idTbl.code))
        {
            return (Convert.ToInt32(idTbl.Codeint) + 1).ToString();
            return;
        }
        string lastNumber = "";
        foreach (char charItem in idTbl.code.Reverse().ToArray())
        {
            if (char.IsDigit(charItem))
            {
                lastNumber = lastNumber.Insert(0, charItem.ToString());
                return lastNumber;
            }
            else if (lastNumber.Length > 0)
                break;
        }
        if (string.IsNullOrEmpty(lastNumber) == false)
            return (Convert.ToInt32(lastNumber) + 1).ToString();
    }
}
catch //(Exception ex)
{
    //
}
}
