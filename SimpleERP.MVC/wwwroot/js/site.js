﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var priceField = document.getElementById("Price");

// adiciona um listener para a ação de envio do formulário
document.getElementById("createProduct").addEventListener("submit", function () {
    // pega o valor do campo de preço
    var priceValue = priceField.value;
    // substitui a vírgula pelo ponto
    var formattedPriceValue = priceValue.replace(',', '.');
    // atualiza o valor do campo de preço com o valor formatado
    priceField.value = formattedPriceValue;
});

(() => {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    const forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})()