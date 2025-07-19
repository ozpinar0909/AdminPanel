// wwwroot/js/sidebar.js

// Sayfa tamamen yüklendiğinde bu kod bloğu çalışacak
document.addEventListener('DOMContentLoaded', function () {
    // Sidebar toggle butonunu ID'si ile bul
    const sidebarToggle = document.getElementById('sidebarToggle');
    // Sayfanın ana içeriğini ve sidebar'ı saran ana 'wrapper' div'ini bul
    const wrapper = document.getElementById('wrapper');

    // Eğer toggle butonu ve wrapper elementi sayfada varsa, olayı dinle
    if (sidebarToggle && wrapper) {
        sidebarToggle.addEventListener('click', function () {
            // Wrapper div'ine 'toggled' sınıfını ekle/kaldır
            // Bu sınıf, sidebar.css dosyasındaki stillerle sidebar'ı açıp kapatır
            wrapper.classList.toggle('toggled');
        });
    } else {
        // Eğer elementler bulunamazsa konsola hata mesajı yaz
        console.error("Sidebar toggle butonu veya wrapper elementi bulunamadı!");
    }

    // İsteğe bağlı: Sidebar menü öğelerinden aktif olanı vurgulamak için
    // Mevcut sayfa URL'sine göre menü öğesini aktif yap
    const currentPath = window.location.pathname;
    const sidebarLinks = document.querySelectorAll('.sidebar-wrapper .list-group-item');

    sidebarLinks.forEach(link => {
        // Linkin href değeri mevcut URL ile eşleşiyorsa
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active'); // 'active' sınıfını ekle
        } else {
            link.classList.remove('active'); // Diğerlerinden 'active' sınıfını kaldır
        }
    });
});