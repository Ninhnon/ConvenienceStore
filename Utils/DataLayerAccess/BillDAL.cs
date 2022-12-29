namespace ConvenienceStore.Utils.DataLayerAccess
{
    public class BillDAL : DataProvider
    {
        private static BillDAL instance;

        public static BillDAL Instance
        {
            get { if (instance == null) instance = new BillDAL(); return instance; }
            private set { instance = value; }
        }
        private BillDAL()
        {

        }


    }
}
