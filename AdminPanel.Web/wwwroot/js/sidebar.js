document.addEventListener('DOMContentLoaded', function () {
    const sidebarToggle = document.getElementById('sidebarToggle');
    const wrapper = document.getElementById('wrapper');
    if (sidebarToggle && wrapper) {
        sidebarToggle.addEventListener('click', function () {
            wrapper.classList.toggle('toggled');
        });
    } else {
        console.error("Sidebar toggle butonu veya wrapper elementi bulunamadı!");
    }
    const currentPath = window.location.pathname;
    const sidebarLinks = document.querySelectorAll('.sidebar-wrapper .list-group-item');

    sidebarLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active'); 
        } else {
            link.classList.remove('active');
        }
    });
});