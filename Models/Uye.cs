//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace blogSitesi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Uye
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uye()
        {
            this.Makales = new HashSet<Makale>();
            this.Yorums = new HashSet<Yorum>();
        }
    
        public int uyeId { get; set; }
        [DisplayName("Kullan�c� Ad�")]
        public string kullaniciAd { get; set; }
        [DisplayName("Email Adres")]
        public string kullaniciEmail { get; set; }
        [DisplayName("�ifre")]
        public string kullaniciSifre { get; set; }
        [DisplayName("Kullan�c� Ad Soyad")]
        public string kullaniciAdSoyad { get; set; }
        [DisplayName("Foto�raf")]
        public string kullaniciFoto { get; set; }
        public Nullable<int> YetkiId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Makale> Makales { get; set; }
        public virtual Yetki Yetki { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yorum> Yorums { get; set; }
    }
}
