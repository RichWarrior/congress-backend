namespace Congress.Core.Entity
{
    public class City : BaseEntity
    {
        /// <summary>
        /// Bağlı Olduğu Ülke
        /// </summary>
        public int countryId { get; set; }
        /// <summary>
        /// İl Adı
        /// </summary>
        public string name { get; set; }
    }
}
