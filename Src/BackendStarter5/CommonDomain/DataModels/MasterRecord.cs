namespace CommonDomain.DataModels
{
    public class MasterRecord
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public bool IsExist
        {
            get
            {
                if (string.IsNullOrEmpty(Title))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
    }
}
