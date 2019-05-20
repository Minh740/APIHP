namespace HPAPI.Controllers
{
    public class NewHttpRespone
    {
        public ResponeCode CodeStatus;
        public object Data;
        public string Message;
        public string CodeNumber;



        public string setData(ResponeCode _Code, string error)
        {

            string resuilt = "";
            switch (_Code)
            {
                case ResponeCode.Success:
                    resuilt = "Success!";
                    break;
                case ResponeCode.InvaliedData:
                    resuilt = "Invalied Data!";
                    break;
                case ResponeCode.Dupllicate:
                    resuilt = error + " Is Dupllicate!";
                    break;
                case ResponeCode.Error:
                    resuilt = "Error: " + error;
                    break;
                case ResponeCode.Unauthorized:
                    resuilt = "Account Unauthorized";
                    break;

            }
            return resuilt;
        }



    }




    public enum ResponeCode
    {
        NONE,
        Success,
        InvaliedData,
        Dupllicate,
        Error,
        Unauthorized,
        Code6


    }
}