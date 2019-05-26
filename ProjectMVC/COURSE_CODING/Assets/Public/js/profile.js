document.getElementById('pf-avatar').addEventListener('click',
    function () {
        document.querySelector('#img-popup').style.height = '100%';
    });

document.querySelector('#img-close').addEventListener('click',
    function () {
        document.querySelector('#img-popup').style.height = '0';
    });



document.getElementById('intro-edit').addEventListener('click',
    function () {
        document.querySelector('#intro-popup').style.height = '100%';
    });

document.querySelector('#intro-close').addEventListener('click',
    function () {
        document.querySelector('#intro-popup').style.height = '0';
    });

document.querySelector('#cancelbtn_1').addEventListener('click',
    function () {
        document.querySelector('#intro-popup').style.height = '0';
    });

document.getElementById('about-edit').addEventListener('click',
    function () {
        document.querySelector('#about-popup').style.height = '100%';
    });

document.querySelector('#about-close').addEventListener('click',
    function () {
        document.querySelector('#about-popup').style.height = '0';
    });

document.querySelector('#cancelbtn_2').addEventListener('click',
    function () {
        document.querySelector('#about-popup').style.height = '0';
    });


