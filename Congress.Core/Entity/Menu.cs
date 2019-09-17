namespace Congress.Core.Entity
{
    public class Menu : BaseEntity
    {
        /// <summary>
        /// Menü Tipi
        /// Web = 1
        /// Mobil = 2
        /// </summary>
        public int menuTypeId { get; set; }
        /// <summary>
        /// Ana Menü ise 0 Değilse Ana Menü Id
        /// </summary>
        public int parentMenuId { get; set; }
        /// <summary>
        /// Menü Adı
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Menü Iconu
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// Menü Url Adresi
        /// </summary>
        public string path { get; set; }        
    }
}
