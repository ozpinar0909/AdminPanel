<script>
        // toastr ayarları
    toastr.options = {
        "closeButton": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
        };

    // TempData'dan gelen mesajları JavaScript değişkenlerine atıyoruz.
    // Bu yöntem, Razor'ın JS içinde daha güvenli ve tutarlı çalışmasını sağlar.
    var successMessage = '@(TempData["successMessage"] != null ? Html.Raw(TempData["successMessage"]) : "")';
    var errorMessage = '@(TempData["errorMessage"] != null ? Html.Raw(TempData["errorMessage"]) : "")';
    var infoMessage = '@(TempData["infoMessage"] != null ? Html.Raw(TempData["infoMessage"]) : "")';
    var warningMessage = '@(TempData["warningMessage"] != null ? Html.Raw(TempData["warningMessage"]) : "")';

    // Mesajlar boş değilse toastr ile göster
    if (successMessage) {
        toastr.success(successMessage);
        }
    if (errorMessage) {
        toastr.error(errorMessage);
        }
    if (infoMessage) {
        toastr.info(infoMessage);
        }
    if (warningMessage) {
        toastr.warning(warningMessage);
        }
</script>