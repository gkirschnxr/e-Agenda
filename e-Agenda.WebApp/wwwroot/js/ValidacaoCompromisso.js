document.addEventListener('DOMContentLoaded', function () {
    const tipoSelect = document.querySelector('[name="Tipo"]');
    const divLocal = document.getElementById('divLocal');
    const divLink = document.getElementById('divLink');

    function atualizarCampos() {
        if (tipoSelect.value === 'Online') {
            divLink.style.display = 'block';
            divLocal.style.display = 'none';
        } else {
            divLink.style.display = 'none';
            divLocal.style.display = 'block';
        }
    }

    if (tipoSelect) {
        tipoSelect.addEventListener('change', atualizarCampos);
        atualizarCampos(); // Atualiza ao carregar a página
    }
});